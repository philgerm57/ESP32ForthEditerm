using System.IO.Ports;

namespace ForthEditor;

/// <summary>Paramètres du terminal série (3 onglets : Port, Texte et couleurs, Envoi). Interface : TerminalSettingsForm.Designer.cs</summary>
public partial class TerminalSettingsForm : Form
{
    static readonly string[] EncodingKeys = { "ASCII", "Latin1", "UTF8" };
    static readonly string[] EolKeys = { "CRLF", "CR", "LF" };
    static readonly string[] ModeKeys = { "Delay", "WaitPrompt" };

    /// <summary>Copie de travail, renvoyée à l'appelant si l'on valide par OK.</summary>
    public TerminalConfig Config { get; } = new();

    public TerminalSettingsForm() { InitializeComponent(); }

    public TerminalSettingsForm(TerminalConfig source, bool connected, int tabIndex) : this()
    {
        Config = source.Clone();

        cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
        cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
        cmbDataBits.Items.AddRange(new object[] { 5, 6, 7, 8 });
        cmbBaudRate.Items.AddRange(new object[]
        {
            110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 230400, 256000
        });
        cmbEncoding.Items.AddRange(new object[] { "ASCII", "Latin-1", "UTF-8" });
        cmbEol.Items.AddRange(new object[] { "CR+LF", "CR", "LF" });
        cmbSendMode.Items.AddRange(new object[] { "Pause fixe", "Attendre l'invite" });

        LoadPortList(Config.PortName);
        LoadControls();
        UpdateSendModeUi();

        // Port ouvert : les paramètres de la liaison ne peuvent pas changer
        lblConnected.Visible = connected;
        cmbPort.Enabled = btnRefresh.Enabled = cmbBaudRate.Enabled = cmbDataBits.Enabled =
            cmbParity.Enabled = cmbStopBits.Enabled = cmbEncoding.Enabled = !connected;

        tabs.SelectedIndex = Math.Clamp(tabIndex, 0, tabs.TabCount - 1);
    }

    static int IndexOfKey(string[] keys, string value) =>
        Math.Max(0, Array.FindIndex(keys, k => string.Equals(k, value, StringComparison.OrdinalIgnoreCase)));

    // ---------- Ports ----------
    void LoadPortList(string selected)
    {
        cmbPort.Items.Clear();
        string[] ports = SerialPort.GetPortNames();
        Array.Sort(ports, StringComparer.OrdinalIgnoreCase);
        cmbPort.Items.AddRange(ports);
        if (!string.IsNullOrEmpty(selected) && cmbPort.Items.Contains(selected)) cmbPort.SelectedItem = selected;
        else if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
    }

    void btnRefresh_Click(object sender, EventArgs e) => LoadPortList(cmbPort.SelectedItem as string ?? Config.PortName);

    // ---------- Valeurs <-> contrôles ----------
    void LoadControls()
    {
        cmbBaudRate.Text = Config.BaudRate.ToString();
        cmbDataBits.SelectedItem = cmbDataBits.Items.Contains(Config.DataBits) ? Config.DataBits : 8;
        cmbParity.SelectedItem = cmbParity.Items.Contains(Config.Parity) ? Config.Parity : "None";
        cmbStopBits.SelectedItem = cmbStopBits.Items.Contains(Config.StopBits) ? Config.StopBits : "One";

        cmbEncoding.SelectedIndex = IndexOfKey(EncodingKeys, Config.EncodingName);
        cmbEol.SelectedIndex = IndexOfKey(EolKeys, Config.EolMode);
        ShowColor(btnBackColor, Config.BackColorHex, Color.Black);
        ShowColor(btnForeColor, Config.ForeColorHex, Color.LimeGreen);

        cmbSendMode.SelectedIndex = IndexOfKey(ModeKeys, Config.SendMode);
        txtPrompt.Text = string.IsNullOrWhiteSpace(Config.Prompt) ? "ok" : Config.Prompt;
        nudTimeout.Value = Math.Clamp(Config.PromptTimeoutMs, (int)nudTimeout.Minimum, (int)nudTimeout.Maximum);
        nudDelay.Value = Math.Clamp(Config.LineDelayMs, (int)nudDelay.Minimum, (int)nudDelay.Maximum);
        chkSkipComments.Checked = Config.SkipComments;
    }

    void btnOk_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(cmbBaudRate.Text, out int baud) || baud <= 0)
        {
            MessageBox.Show(this, "Vitesse (bauds) invalide.", "Terminal série", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabs.SelectedIndex = 0;
            cmbBaudRate.Focus();
            DialogResult = DialogResult.None;        // empêche la fermeture
            return;
        }
        Config.PortName = cmbPort.SelectedItem as string ?? Config.PortName;
        Config.BaudRate = baud;
        if (cmbDataBits.SelectedItem is int db) Config.DataBits = db;
        if (cmbParity.SelectedItem is string parity) Config.Parity = parity;
        if (cmbStopBits.SelectedItem is string stop) Config.StopBits = stop;

        Config.EncodingName = EncodingKeys[Math.Max(0, cmbEncoding.SelectedIndex)];
        Config.EolMode = EolKeys[Math.Max(0, cmbEol.SelectedIndex)];
        // les couleurs sont déjà dans Config (mises à jour au choix)

        Config.SendMode = ModeKeys[Math.Max(0, cmbSendMode.SelectedIndex)];
        Config.Prompt = string.IsNullOrWhiteSpace(txtPrompt.Text) ? "ok" : txtPrompt.Text.Trim();
        Config.PromptTimeoutMs = (int)nudTimeout.Value;
        Config.LineDelayMs = (int)nudDelay.Value;
        Config.SkipComments = chkSkipComments.Checked;
    }

    // ---------- Envoi ----------
    void cmbSendMode_SelectedIndexChanged(object sender, EventArgs e) => UpdateSendModeUi();

    void UpdateSendModeUi()
    {
        bool wait = cmbSendMode.SelectedIndex == 1;
        txtPrompt.Enabled = nudTimeout.Enabled = wait;
    }

    // ---------- Couleurs ----------
    static void ShowColor(Button btn, string html, Color fallback)
    {
        Color c;
        try { c = ColorTranslator.FromHtml(html); } catch { c = fallback; }
        btn.BackColor = c;
    }

    void btnBackColor_Click(object sender, EventArgs e) => PickColor(btnBackColor, v => Config.BackColorHex = v);
    void btnForeColor_Click(object sender, EventArgs e) => PickColor(btnForeColor, v => Config.ForeColorHex = v);

    void PickColor(Button btn, Action<string> store)
    {
        using var dlg = new ColorDialog { Color = btn.BackColor, FullOpen = true };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        btn.BackColor = dlg.Color;
        store(ColorTranslator.ToHtml(dlg.Color));
    }
}
