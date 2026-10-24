// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace ForthEditor;

/// <summary>
/// Terminal série (RS-232 / USB-série), intégré à ForthEditor. Interface : TerminalForm.Designer.cs
/// La fenêtre ne garde que les commandes (ouvrir, fermer, envoyer l'éditeur, effacer) ;
/// tous les réglages sont dans le menu Paramètres (TerminalSettingsForm).
/// </summary>
public partial class TerminalForm : Form
{
    SerialPort _port;
    TerminalConfig _config;
    CancellationTokenSource _sendCts;

    // Au-delà de cette position, le texte est la saisie en cours ; en deçà, l'historique reçu est protégé.
    int _inputStart;

    // Réception (thread du port) : décodage, normalisation des fins de ligne, détection de l'invite
    readonly object _rxLock = new();
    Decoder _decoder;
    bool _lastWasCr;
    readonly StringBuilder _waitBuf = new();
    readonly SemaphoreSlim _okSignal = new(0);
    Regex _promptRegex;
    volatile bool _waiting;

    // Suppression de l'écho distant : texte envoyé dont on attend le retour, à ne pas afficher
    volatile bool _suppressEcho;
    string _echoExpected = "";
    DateTime _echoDeadline;

    /// <summary>Fournie par la fenêtre principale : texte de l'éditeur à envoyer (sélection, sinon fichier entier).</summary>
    public Func<string> GetEditorText { get; set; }

    public TerminalForm()
    {
        InitializeComponent();
        _config = TerminalConfig.Load();
        ApplyColors();
        ApplyEchoSetting();
        UpdateButtonsState(false);
        ShowIdleStatus();
    }

    // ---------- Réglages courants ----------
    string Eol => _config.EolMode switch { "CR" => "\r", "LF" => "\n", _ => "\r\n" };

    Encoding CurrentEncoding => _config.EncodingName switch
    {
        "Latin1" => Encoding.Latin1,
        "UTF8" => new UTF8Encoding(false),
        _ => Encoding.ASCII
    };

    string EncodingLabel => _config.EncodingName switch { "Latin1" => "Latin-1", "UTF8" => "UTF-8", _ => "ASCII" };

    string Summary()
    {
        Enum.TryParse(_config.Parity, out Parity p);
        Enum.TryParse(_config.StopBits, out StopBits s);
        return $"{_config.PortName} — {_config.BaudRate} bauds, {_config.DataBits}{ParityShort(p)}{StopBitsShort(s)}, {EncodingLabel}";
    }

    void ShowIdleStatus() => lblStatus.Text = "Déconnecté — " + Summary();

    void ApplyColors()
    {
        txtTerminal.BackColor = SafeColor(_config.BackColorHex, Color.Black);
        txtTerminal.ForeColor = SafeColor(_config.ForeColorHex, Color.LimeGreen);
    }

    static Color SafeColor(string html, Color fallback)
    {
        try { return ColorTranslator.FromHtml(html); } catch { return fallback; }
    }

    // ---------- Menu Paramètres ----------
    void mnuPortSettings_Click(object sender, EventArgs e) => ShowSettings(0);
    void mnuTextSettings_Click(object sender, EventArgs e) => ShowSettings(1);
    void mnuSendSettings_Click(object sender, EventArgs e) => ShowSettings(2);

    bool ShowSettings(int tab)
    {
        bool connected = _port != null && _port.IsOpen;
        using var dlg = new TerminalSettingsForm(_config, connected, tab);
        if (dlg.ShowDialog(this) != DialogResult.OK) return false;
        _config = dlg.Config;
        _config.Save();
        ApplyColors();
        ApplyEchoSetting();
        if (!connected) ShowIdleStatus();
        return true;
    }

    // ---------- Ouverture / fermeture ----------
    void BtnOpen_Click(object sender, EventArgs e)
    {
        if (_port != null && _port.IsOpen) return;

        // Port absent (premier lancement, câble débranché…) : on ouvre directement les paramètres du port
        if (!SerialPort.GetPortNames().Contains(_config.PortName, StringComparer.OrdinalIgnoreCase))
        {
            Warn(SerialPort.GetPortNames().Length == 0
                ? "Aucun port série détecté. Branchez l'appareil puis choisissez le port dans Paramètres → Port série."
                : $"Le port « {_config.PortName} » n'est pas disponible. Choisissez un autre port.");
            if (!ShowSettings(0)) return;
            if (!SerialPort.GetPortNames().Contains(_config.PortName, StringComparer.OrdinalIgnoreCase)) return;
        }

        try
        {
            if (!Enum.TryParse(_config.Parity, out Parity parity)) parity = Parity.None;
            if (!Enum.TryParse(_config.StopBits, out StopBits stopBits)) stopBits = StopBits.One;
            var encoding = CurrentEncoding;

            _port = new SerialPort(_config.PortName, _config.BaudRate, parity, _config.DataBits, stopBits)
            {
                Encoding = encoding, ReadTimeout = 500, WriteTimeout = 500
            };
            lock (_rxLock) { _decoder = encoding.GetDecoder(); _lastWasCr = false; }
            _port.DataReceived += SerialPort_DataReceived;
            _port.Open();

            UpdateButtonsState(true);
            lblStatus.Text = "Connecté — " + Summary();

            txtTerminal.Focus();
            _inputStart = txtTerminal.TextLength;
            txtTerminal.SelectionStart = txtTerminal.TextLength;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Impossible d'ouvrir le port : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _port?.Dispose();
            _port = null;
            UpdateButtonsState(false);
        }
    }

    void BtnClose_Click(object sender, EventArgs e) => ClosePort();

    void ClosePort()
    {
        _sendCts?.Cancel();
        if (_port != null)
        {
            try
            {
                _port.DataReceived -= SerialPort_DataReceived;
                if (_port.IsOpen) _port.Close();
            }
            catch { /* ignorer les erreurs de fermeture */ }
            finally { _port.Dispose(); _port = null; }
        }
        UpdateButtonsState(false);
        if (!IsDisposed) ShowIdleStatus();
    }

    void UpdateButtonsState(bool connected)
    {
        btnOpen.Enabled = !connected;
        btnClose.Enabled = connected;
        btnSendFile.Enabled = connected || _sendCts != null;
    }

    // ---------- Réception ----------
    void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var port = _port;
        if (port == null || !port.IsOpen) return;
        try
        {
            int n = port.BytesToRead;
            if (n <= 0) return;
            var buffer = new byte[n];
            int read = port.Read(buffer, 0, n);

            string display;
            lock (_rxLock)
            {
                var chars = new char[_decoder.GetCharCount(buffer, 0, read)];
                int count = _decoder.GetChars(buffer, 0, read, chars, 0);
                string text = new string(chars, 0, count);
                FeedPromptDetector(text);
                display = NormalizeEol(text);
                if (_suppressEcho) display = StripEcho(display);
            }
            if (display.Length > 0 && !IsDisposed && txtTerminal.IsHandleCreated)
                txtTerminal.BeginInvoke(new Action(() => AppendReceivedText(display)));
        }
        catch { /* port fermé entre-temps */ }
    }

    /// <summary>CR, LF et CR+LF (même coupé entre deux paquets) deviennent un seul saut de ligne.</summary>
    string NormalizeEol(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (char c in text)
        {
            if (c == '\r') { sb.Append('\n'); _lastWasCr = true; }
            else if (c == '\n') { if (!_lastWasCr) sb.Append('\n'); _lastWasCr = false; }
            else { sb.Append(c); _lastWasCr = false; }
        }
        return sb.ToString();
    }

    void AppendReceivedText(string text)
    {
        if (IsDisposed) return;
        txtTerminal.AppendText(text);
        _inputStart = txtTerminal.TextLength;
        txtTerminal.SelectionStart = txtTerminal.TextLength;
        txtTerminal.ScrollToCaret();
    }

    // ---------- Saisie et émission ----------
    void TxtTerminal_KeyDown(object sender, KeyEventArgs e)
    {
        // Copier / coller en texte brut (sans mise en forme)
        if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.Insert))
        {
            CopySelection();
            e.SuppressKeyPress = e.Handled = true;
            return;
        }
        if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
        {
            PasteIntoInput();
            e.SuppressKeyPress = e.Handled = true;
            return;
        }

        // Interdit de modifier l'historique reçu : le curseur est ramené en fin de zone de saisie.
        if (txtTerminal.SelectionStart < _inputStart)
        {
            bool navigation = e.KeyCode is Keys.Left or Keys.Right or Keys.Up or Keys.Down or Keys.Home or Keys.End or Keys.PageUp or Keys.PageDown
                              || (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.A));
            if (!navigation)
            {
                txtTerminal.SelectionStart = txtTerminal.TextLength;
                txtTerminal.SelectionLength = 0;
            }
        }

        if (e.KeyCode == Keys.Back && txtTerminal.SelectionStart <= _inputStart)
        {
            e.SuppressKeyPress = true;
            return;
        }

        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;

            int len = Math.Max(0, txtTerminal.TextLength - _inputStart);
            string line = txtTerminal.Text.Substring(_inputStart, len);

            if (_port != null && _port.IsOpen)
            {
                try { ExpectEcho(line); _port.Write(line + Eol); }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Erreur d'émission : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            txtTerminal.AppendText(Environment.NewLine);
            _inputStart = txtTerminal.TextLength;
            txtTerminal.SelectionStart = txtTerminal.TextLength;
            txtTerminal.ScrollToCaret();
        }
    }

    // ---------- Écho de la cible ----------
    void mnuSuppressEcho_Click(object sender, EventArgs e)
    {
        _config.SuppressEcho = mnuSuppressEcho.Checked;
        _config.Save();
        ApplyEchoSetting();
    }

    void ApplyEchoSetting()
    {
        _suppressEcho = _config.SuppressEcho;
        mnuSuppressEcho.Checked = _config.SuppressEcho;
        lock (_rxLock) { _echoExpected = ""; }
    }

    /// <summary>À appeler AVANT l'écriture sur le port : le retour de la cible peut arriver immédiatement.</summary>
    void ExpectEcho(string line)
    {
        if (!_suppressEcho) return;
        lock (_rxLock)
        {
            _echoExpected += line + "\n";
            _echoDeadline = DateTime.UtcNow.AddSeconds(3);
        }
    }

    /// <summary>
    /// Retire du texte reçu (fins de ligne déjà normalisées) ce que la cible renvoie à l'identique de ce qui vient d'être envoyé.
    /// Dès qu'un caractère ne correspond plus, l'attente est abandonnée et le reste s'affiche normalement :
    /// une cible qui n'écho pas, ou autrement, ne perd donc jamais de réponse. Appelé sous _rxLock.
    /// </summary>
    string StripEcho(string text)
    {
        if (_echoExpected.Length == 0) return text;
        if (DateTime.UtcNow > _echoDeadline) { _echoExpected = ""; return text; }
        int i = 0;
        while (i < text.Length && _echoExpected.Length > 0)
        {
            if (text[i] != _echoExpected[0]) { _echoExpected = ""; break; }
            _echoExpected = _echoExpected.Substring(1);
            i++;
        }
        return text.Substring(i);
    }

    // ---------- Envoi du contenu de l'éditeur ----------
    static bool IsCommentLine(string trimmed) =>
        trimmed == "\\" || trimmed.StartsWith("\\ ") || trimmed.StartsWith("\\\t");

    /// <summary>Prépare l'attente de l'invite : vide le tampon, avant l'envoi de la ligne.</summary>
    void PrepareWait(string prompt)
    {
        lock (_rxLock)
        {
            _waitBuf.Clear();
            while (_okSignal.Wait(0)) { }
            _promptRegex = new Regex(@"(?<!\S)" + Regex.Escape(prompt) + @"\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            _waiting = true;
        }
    }

    /// <summary>Appelé par le thread de réception : signale quand le texte reçu se termine par l'invite (« ok »).</summary>
    void FeedPromptDetector(string text)
    {
        if (!_waiting) return;
        _waitBuf.Append(text);
        if (_waitBuf.Length > 4096) _waitBuf.Remove(0, _waitBuf.Length - 4096);
        if (_promptRegex != null && _promptRegex.IsMatch(_waitBuf.ToString()))
        {
            _waiting = false;
            _okSignal.Release();
        }
    }

    async Task<bool> WaitForPromptAsync(int timeoutMs, CancellationToken token)
    {
        try { return await _okSignal.WaitAsync(timeoutMs, token); }
        finally { _waiting = false; }
    }

    /// <summary>
    /// Envoie la sélection (ou tout le fichier) de l'éditeur ligne par ligne, selon les réglages de Paramètres → Envoi.
    /// </summary>
    async void BtnSendFile_Click(object sender, EventArgs e)
    {
        if (_sendCts != null) { _sendCts.Cancel(); return; }          // 2e clic = annuler
        if (_port == null || !_port.IsOpen) { Warn("Ouvrez d'abord le port série."); return; }

        string text = GetEditorText?.Invoke() ?? "";
        var all = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        bool waitPrompt = _config.SendMode == "WaitPrompt";
        bool skip = _config.SkipComments;
        string prompt = string.IsNullOrWhiteSpace(_config.Prompt) ? "ok" : _config.Prompt;
        int timeout = _config.PromptTimeoutMs;
        int delay = _config.LineDelayMs;
        string eol = Eol;

        var lines = new List<(int No, string Text)>();
        for (int i = 0; i < all.Length; i++)
        {
            string trimmed = all[i].Trim();
            if ((skip || waitPrompt) && trimmed.Length == 0) continue;     // une ligne vide ne provoque pas toujours d'invite
            if (skip && IsCommentLine(trimmed)) continue;
            lines.Add((i, all[i]));
        }
        if (lines.Count == 0) { Warn("Rien à envoyer."); return; }

        _sendCts = new CancellationTokenSource();
        var token = _sendCts.Token;
        btnSendFile.Text = "Annuler l'envoi";
        string previous = lblStatus.Text;
        int sent = 0;
        bool stopped = false;

        try
        {
            foreach (var (no, line) in lines)
            {
                token.ThrowIfCancellationRequested();
                var port = _port;
                if (port == null || !port.IsOpen) break;

                if (waitPrompt) PrepareWait(prompt);
                ExpectEcho(line);
                await Task.Run(() => port.Write(line + eol), token);
                sent++;
                lblStatus.Text = $"Envoi… {sent}/{lines.Count} lignes";

                if (waitPrompt && !await WaitForPromptAsync(timeout, token))
                {
                    var r = MessageBox.Show(this,
                        $"Pas de réponse « {prompt} » en {timeout} ms à la ligne {no + 1} :\n\n{line}\n\nContinuer l'envoi ?",
                        "Terminal série", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (r != DialogResult.Yes) { stopped = true; break; }
                }
                if (delay > 0) await Task.Delay(delay, token);
            }
            lblStatus.Text = stopped ? $"Envoi interrompu après {sent} ligne(s) (pas de réponse)" : $"Envoi terminé : {sent} ligne(s)";
        }
        catch (OperationCanceledException) { lblStatus.Text = $"Envoi annulé après {sent} ligne(s)"; }
        catch (Exception ex)
        {
            lblStatus.Text = previous;
            MessageBox.Show(this, $"Erreur d'émission : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _waiting = false;
            _sendCts?.Dispose();
            _sendCts = null;
            if (!IsDisposed) { btnSendFile.Text = "Envoyer l'éditeur"; UpdateButtonsState(_port != null && _port.IsOpen); }
        }
    }

    // ---------- Menu contextuel : copier / coller ----------
    static string ToClipboardText(string text) => text.Replace("\r\n", "\n").Replace("\n", "\r\n");

    static void SetClipboard(string text)
    {
        if (text.Length == 0) return;
        try { Clipboard.SetText(ToClipboardText(text)); }
        catch (System.Runtime.InteropServices.ExternalException) { /* presse-papiers occupé par une autre application */ }
    }

    /// <summary>Copie la sélection en texte brut.</summary>
    void CopySelection() => SetClipboard(txtTerminal.SelectedText);

    /// <summary>Colle à la position de saisie : l'historique reçu reste protégé.</summary>
    void PasteIntoInput()
    {
        if (!Clipboard.ContainsText()) return;
        if (txtTerminal.SelectionStart < _inputStart)
        {
            txtTerminal.SelectionStart = txtTerminal.TextLength;
            txtTerminal.SelectionLength = 0;
        }
        txtTerminal.SelectedText = Clipboard.GetText().Replace("\r\n", "\n");
    }

    void ctxTerminal_Opening(object sender, CancelEventArgs e)
    {
        ctxTermCopy.Enabled = txtTerminal.SelectionLength > 0;
        ctxTermCopyAll.Enabled = ctxTermSelectAll.Enabled = ctxTermClear.Enabled = txtTerminal.TextLength > 0;
        ctxTermPaste.Enabled = Clipboard.ContainsText();
    }

    void ctxTermCopy_Click(object sender, EventArgs e) => CopySelection();
    void ctxTermCopyAll_Click(object sender, EventArgs e) => SetClipboard(txtTerminal.Text);
    void ctxTermSelectAll_Click(object sender, EventArgs e) { txtTerminal.Focus(); txtTerminal.SelectAll(); }
    void ctxTermPaste_Click(object sender, EventArgs e) { txtTerminal.Focus(); PasteIntoInput(); }
    void ctxTermClear_Click(object sender, EventArgs e) => BtnClear_Click(sender, e);

    void BtnClear_Click(object sender, EventArgs e)
    {
        txtTerminal.Clear();
        _inputStart = 0;
    }

    void TerminalForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        _config.Save();
        ClosePort();
    }

    // ---------- Utilitaires ----------
    void Warn(string message) => MessageBox.Show(this, message, "Terminal série", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    static string ParityShort(Parity p) => p switch
    {
        Parity.None => "N", Parity.Odd => "O", Parity.Even => "E", Parity.Mark => "M", Parity.Space => "S", _ => p.ToString()
    };

    static string StopBitsShort(StopBits s) => s switch
    {
        StopBits.One => "1", StopBits.OnePointFive => "1.5", StopBits.Two => "2", _ => s.ToString()
    };
}
