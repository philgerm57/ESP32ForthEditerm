using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ForthEditor;

/// <summary>RichTextBox : texte brut au collage, indentation auto, zoom Ctrl+molette, dépôt de fichiers.</summary>
[DesignerCategory("Code")]
public class CodeBox : RichTextBox
{
    public CodeBox()
    {
        WordWrap = false; AcceptsTab = true; DetectUrls = false; BorderStyle = BorderStyle.None;
        ScrollBars = RichTextBoxScrollBars.ForcedBoth; HideSelection = false;
        AllowDrop = true;
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AutoIndent { get; set; } = true;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int IndentSize { get; set; } = 4;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IndentAfterColon { get; set; } = true;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool TabUsesSpaces { get; set; }
    /// <summary>Permet à l'éditeur d'intercepter une touche avant tout traitement (complétion).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<KeyEventArgs, bool> KeyIntercept { get; set; }

    public event Action<int> ZoomRequested;
    public event Action<string[]> FilesDropped;

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (KeyIntercept != null && KeyIntercept(e)) { e.Handled = e.SuppressKeyPress = true; return; }

        // Tab / Maj+Tab : indenter / désindenter les lignes touchées par la sélection
        if (e.KeyCode == Keys.Tab && !e.Control && !e.Alt)
        {
            if (e.Shift)
            {
                IndentLines(true);
                e.Handled = e.SuppressKeyPress = true;
                return;
            }
            if (SelectionLength > 0 && SelectedText.Contains('\n'))
            {
                IndentLines(false);
                e.Handled = e.SuppressKeyPress = true;
                return;
            }
            if (TabUsesSpaces)
            {
                SelectedText = new string(' ', IndentSize);
                e.Handled = e.SuppressKeyPress = true;
                return;
            }
        }

        if (AutoIndent && e.KeyCode == Keys.Enter && !e.Control && !e.Alt && !e.Shift)
        {
            string all = Text;
            int start = SelectionStart == 0 ? 0 : all.LastIndexOf('\n', SelectionStart - 1) + 1;
            string before = all.Substring(start, SelectionStart - start);
            int n = 0;
            while (n < before.Length && (before[n] == ' ' || before[n] == '\t')) n++;
            string indent = before.Substring(0, n);
            string trimmed = before.Substring(n);
            if (IndentAfterColon && trimmed.Length > 1 && trimmed[0] == ':' && char.IsWhiteSpace(trimmed[1]) && !trimmed.Contains(';'))
                indent += new string(' ', IndentSize);
            e.Handled = e.SuppressKeyPress = true;
            SelectedText = "\n" + indent;
            ScrollToCaret();
            return;
        }
        base.OnKeyDown(e);
    }

    /// <summary>
    /// Indente (Tab) ou désindente (Maj+Tab) toutes les lignes touchées par la sélection, ou la ligne du curseur.
    /// Désindenter retire une tabulation, ou au plus IndentSize espaces, au début de chaque ligne.
    /// </summary>
    void IndentLines(bool unindent)
    {
        string all = Text;
        int selStart = SelectionStart, selEnd = SelectionStart + SelectionLength;
        int firstLineStart = selStart == 0 ? 0 : all.LastIndexOf('\n', selStart - 1) + 1;
        // une sélection qui finit juste après un saut de ligne ne touche pas la ligne suivante
        int effEnd = selEnd > selStart && all[selEnd - 1] == '\n' ? selEnd - 1 : selEnd;
        int lastLineEnd = all.IndexOf('\n', effEnd);
        if (lastLineEnd < 0) lastLineEnd = all.Length;

        string block = all.Substring(firstLineStart, lastLineEnd - firstLineStart);
        string[] lines = block.Split('\n');
        string unit = TabUsesSpaces ? new string(' ', IndentSize) : "\t";
        int firstDelta = 0, totalDelta = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            int delta;
            if (!unindent)
            {
                if (lines[i].Length == 0 && lines.Length > 1) delta = 0;          // pas d'espaces sur les lignes vides
                else { lines[i] = unit + lines[i]; delta = unit.Length; }
            }
            else
            {
                int remove = 0;
                if (lines[i].StartsWith('\t')) remove = 1;
                else while (remove < IndentSize && remove < lines[i].Length && lines[i][remove] == ' ') remove++;
                lines[i] = lines[i].Substring(remove);
                delta = -remove;
            }
            if (i == 0) firstDelta = delta;
            totalDelta += delta;
        }

        string result = string.Join("\n", lines);
        if (result == block) return;
        Select(firstLineStart, block.Length);
        SelectedText = result;
        int newStart = Math.Max(firstLineStart, selStart + firstDelta);
        int newEnd = Math.Max(newStart, selEnd + totalDelta);
        Select(newStart, newEnd - newStart);
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_MOUSEWHEEL = 0x020A;
        if (m.Msg == WM_MOUSEWHEEL && (ModifierKeys & Keys.Control) == Keys.Control)
        {
            short delta = (short)(((long)m.WParam >> 16) & 0xFFFF);
            ZoomRequested?.Invoke(delta > 0 ? 1 : -1);
            return;
        }
        base.WndProc(ref m);
    }

    protected override void OnDragEnter(DragEventArgs e)
    {
        if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) { e.Effect = DragDropEffects.Copy; return; }
        base.OnDragEnter(e);
    }

    protected override void OnDragDrop(DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files) { FilesDropped?.Invoke(files); return; }
        base.OnDragDrop(e);
    }

    public void PasteText() { if (Clipboard.ContainsText()) SelectedText = Clipboard.GetText(); }

    protected override bool ProcessCmdKey(ref Message m, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.V) || keyData == (Keys.Shift | Keys.Insert)) { PasteText(); return true; }
        return base.ProcessCmdKey(ref m, keyData);
    }
}

/// <summary>Bloc repliable « : nom ... ; ». Le texte masqué est [HideStart, HideEnd).</summary>
public class FoldRegion
{
    public int HeaderLine, ClosingLine, HideStart, HideEnd;
    public bool Folded;
}

/// <summary>Entrée du plan des définitions.</summary>
public class OutlineItem
{
    public string Name = "", Kind = ":";
    public int Line;   // 0-based
    public override string ToString() => Kind == ":" ? Name : $"{Name}  ({Kind.ToLowerInvariant()})";
}

/// <summary>Liste de suggestions qui ne prend jamais le focus (le clic est géré à la main).</summary>
[DesignerCategory("Code")]
class CompletionList : ListBox
{
    public event Action<int> ItemChosen;

    public CompletionList()
    {
        TabStop = false; IntegralHeight = false; BorderStyle = BorderStyle.FixedSingle; Visible = false;
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_LBUTTONDOWN = 0x0201, WM_LBUTTONDBLCLK = 0x0203;
        if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK)
        {
            long lp = m.LParam.ToInt64();
            int idx = IndexFromPoint((short)(lp & 0xFFFF), (short)((lp >> 16) & 0xFFFF));
            if (idx >= 0) { SelectedIndex = idx; ItemChosen?.Invoke(idx); }
            return;
        }
        base.WndProc(ref m);
    }
}

/// <summary>Numéros de ligne + marqueurs de repliement.</summary>
[DesignerCategory("Code")]
class LineNumberPanel : Control
{
    const int MarkerArea = 18;
    readonly RichTextBox _box;
    readonly Func<List<int>> _newlines;     // positions des '\n' calculées depuis le texte (numérotation logique)
    Dictionary<int, FoldRegion> _byHeader = new();
    public event Action<int> FoldClicked;

    public LineNumberPanel(RichTextBox box, Func<List<int>> newlines)
    {
        _box = box; _newlines = newlines; Dock = DockStyle.Left; Width = 60;
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    public void SetRegions(List<FoldRegion> regions)
    {
        var d = new Dictionary<int, FoldRegion>();
        foreach (var r in regions) d.TryAdd(r.HeaderLine, r);
        _byHeader = d;
        Invalidate();
    }

    int _lineH;
    Font _lineHFont;

    static int LineOf(List<int> nl, int idx) { int k = nl.BinarySearch(idx); return k < 0 ? ~k : k; }
    static int StartOf(List<int> nl, int line) => line <= 0 ? 0 : nl[Math.Min(line, nl.Count) - 1] + 1;

    /// <summary>
    /// Lignes visibles avec leur position verticale. La numérotation est LOGIQUE (calculée depuis le texte) :
    /// le RichTextBox renumérote ses lignes selon l'affichage quand du texte est masqué, on ne s'y fie donc pas.
    /// Les lignes masquées par un repli sont sautées ; la position Y est calculée de proche en proche.
    /// </summary>
    List<(int Line, int Y, int H)> LayoutLines()
    {
        var res = new List<(int Line, int Y, int H)>();
        var nl = _newlines();
        int total = nl.Count + 1;

        int first = LineOf(nl, _box.GetCharIndexFromPosition(new Point(0, 0)));
        foreach (var r in _byHeader.Values)                                  // sécurité : jamais dans une zone masquée
            if (r.Folded && first > r.HeaderLine && first <= r.ClosingLine) first = r.HeaderLine;
        int y = _box.GetPositionFromCharIndex(StartOf(nl, first)).Y;

        if (!ReferenceEquals(_lineHFont, _box.Font)) { _lineH = 0; _lineHFont = _box.Font; }
        bool firstFolded = _byHeader.TryGetValue(first, out var fr) && fr.Folded;
        if (!firstFolded && first + 1 < total)
        {
            int d = _box.GetPositionFromCharIndex(StartOf(nl, first + 1)).Y - y;
            if (d > 0 && d < 300) _lineH = d;
        }
        int h = _lineH > 0 ? _lineH : _box.Font.Height;

        for (int i = first; i < total && y <= Height; i++)
        {
            res.Add((i, y, h));
            y += h;
            if (_byHeader.TryGetValue(i, out var r) && r.Folded) i = r.ClosingLine;   // lignes masquées : non numérotées
        }
        return res;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics; g.Clear(BackColor);
        int total = _newlines().Count + 1;
        int w = TextRenderer.MeasureText(new string('9', Math.Max(3, total.ToString().Length)), _box.Font).Width + 14 + MarkerArea;
        if (Width != w) { Width = w; return; }

        using var pen = new Pen(ForeColor);
        foreach (var (line, y, h) in LayoutLines())
        {
            TextRenderer.DrawText(g, (line + 1).ToString(), _box.Font, new Rectangle(0, y, Width - MarkerArea - 4, h),
                ForeColor, TextFormatFlags.Right | TextFormatFlags.NoPadding | TextFormatFlags.Top);

            if (_byHeader.TryGetValue(line, out var r))
            {
                var bx = new Rectangle(Width - MarkerArea + 2, y + (h - 9) / 2, 9, 9);
                g.DrawRectangle(pen, bx);
                g.DrawLine(pen, bx.X + 2, bx.Y + 4, bx.Right - 2, bx.Y + 4);
                if (r.Folded) g.DrawLine(pen, bx.X + 4, bx.Y + 2, bx.X + 4, bx.Bottom - 2);
            }
        }
        using var dot = new Pen(ForeColor) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
        g.DrawLine(dot, Width - 1, 0, Width - 1, Height);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && e.X >= Width - MarkerArea)
        {
            foreach (var (line, y, h) in LayoutLines())
                if (e.Y >= y && e.Y < y + h)
                {
                    if (_byHeader.ContainsKey(line)) FoldClicked?.Invoke(line);
                    break;
                }
        }
        base.OnMouseDown(e);
    }
}

/// <summary>Éditeur Forth : numéros de ligne, coloration, repliement, plan des définitions, complétion.</summary>
[DesignerCategory("Code")]
public class ForthEditorControl : UserControl
{
    const int WM_SETREDRAW = 0x000B, EM_GETEVENTMASK = 0x043B, EM_SETEVENTMASK = 0x0445,
              EM_GETSCROLLPOS = 0x04DD, EM_SETSCROLLPOS = 0x04DE, EM_SETCHARFORMAT = 0x0444,
              SCF_SELECTION = 1;
    const uint CFM_HIDDEN = 0x100, CFE_HIDDEN = 0x100;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct CHARFORMAT2
    {
        public int cbSize; public uint dwMask; public uint dwEffects; public int yHeight; public int yOffset; public int crTextColor;
        public byte bCharSet; public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string szFaceName;
        public ushort wWeight; public short sSpacing; public int crBackColor; public int lcid; public uint dwReserved;
        public short sStyle; public short wKerning; public byte bUnderlineType; public byte bAnimation; public byte bRevAuthor; public byte bUnderlineColor;
    }

    [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr h, int m, IntPtr w, IntPtr l);
    [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr h, int m, IntPtr w, ref Point p);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr SendMessage(IntPtr h, int m, IntPtr w, ref CHARFORMAT2 cf);

    readonly CodeBox _box = new() { Dock = DockStyle.Fill };
    readonly LineNumberPanel _numbers;
    readonly CompletionList _list = new();
    readonly System.Windows.Forms.Timer _timer = new() { Interval = 250 };

    SyntaxTheme _theme;
    bool _busy, _loading, _accepting, _structureDirty, _anyFolded;
    int _prevLen, _compStart;
    List<FoldRegion> _regions = new();
    List<OutlineItem> _outline = new();
    List<string> _docWords = new(), _keywords = new();

    public CodeBox Box => _box;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UseCrLf { get; set; } = true;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AutoCompleteEnabled { get; set; } = true;
    public IReadOnlyList<OutlineItem> Outline => _outline;

    public event EventHandler ContentChanged;
    public event Action<int> ZoomRequested;
    public event Action<string[]> FilesDropped;
    /// <summary>Le plan des définitions a été recalculé.</summary>
    public event Action StructureChanged;

    public ForthEditorControl()
    {
        _numbers = new LineNumberPanel(_box, Newlines);
        Controls.Add(_box); Controls.Add(_numbers); Controls.Add(_list);
        _box.BringToFront(); // le contrôle Fill doit être au premier plan pour le docking
        _list.Font = new Font("Segoe UI", 9f);
        _list.ItemChosen += _ => AcceptCompletion();
        _box.KeyIntercept = HandleKey;

        _timer.Tick += (s, e) =>
        {
            _timer.Stop(); Highlight();
            if (_structureDirty) RefreshStructure();
        };
        _box.TextChanged += (s, e) =>
        {
            _nlDirty = true;
            int len = _box.TextLength;
            bool typed = len == _prevLen + 1;
            _prevLen = len;
            if (_busy || _loading) return;
            if (_anyFolded) ClearAllFolds();           // l'édition invalide les positions : on déplie tout
            ContentChanged?.Invoke(this, EventArgs.Empty);
            _structureDirty = true;
            _timer.Stop(); _timer.Start();
            _numbers.Invalidate();
            if (_accepting) return;
            if (typed && AutoCompleteEnabled) ShowCompletion(false); else HideCompletion();
        };
        _box.SelectionChanged += (s, e) => OnSelectionChanged();
        _box.VScroll += (s, e) => { _numbers.Invalidate(); HideCompletion(); };
        _box.Resize += (s, e) => _numbers.Invalidate();
        _box.HandleCreated += (s, e) => { _timer.Stop(); _timer.Start(); };
        _box.LostFocus += (s, e) => HideCompletion();
        _box.MouseDown += (s, e) => HideCompletion();
        _box.ZoomRequested += d => ZoomRequested?.Invoke(d);
        _box.FilesDropped += f => FilesDropped?.Invoke(f);
        _numbers.FoldClicked += ToggleFoldAtLine;
    }

    // ===================== Numérotation logique des lignes =====================
    readonly List<int> _nl = new();
    bool _nlDirty = true;

    /// <summary>Positions des '\n' (recalculées si le texte a changé).</summary>
    List<int> Newlines()
    {
        if (_nlDirty)
        {
            _nl.Clear();
            string t = _box.Text;
            for (int i = 0; i < t.Length; i++) if (t[i] == '\n') _nl.Add(i);
            _nlDirty = false;
        }
        return _nl;
    }

    public int LineCount => Newlines().Count + 1;
    /// <summary>Numéro de ligne (0-based) du caractère donné, indépendant de l'affichage (repli).</summary>
    public int LineFromIndex(int idx) { var nl = Newlines(); int k = nl.BinarySearch(idx); return k < 0 ? ~k : k; }
    public int LineStart(int line) { var nl = Newlines(); line = Math.Clamp(line, 0, nl.Count); return line == 0 ? 0 : nl[line - 1] + 1; }

    // ===================== Texte / thème =====================
    public void LoadText(string text)
    {
        _loading = true; _box.Text = text; _loading = false;
        _nlDirty = true;
        _prevLen = _box.TextLength;
        _box.ClearUndo();
        Highlight();
        RefreshStructure();
    }

    public string GetTextForFile()
    {
        var t = _box.Text.Replace("\r\n", "\n");
        return UseCrLf ? t.Replace("\n", "\r\n") : t;
    }

    public void ApplyTheme(SyntaxTheme theme)
    {
        _theme = theme;
        _busy = true;
        _box.Font = theme.Font; _box.BackColor = theme.Background; _box.ForeColor = theme.Foreground;
        _box.AutoIndent = theme.AutoIndent; _box.IndentSize = theme.IndentSize; _box.IndentAfterColon = theme.IndentAfterColon;
        _box.TabUsesSpaces = theme.TabUsesSpaces;
        _numbers.BackColor = theme.NumberBackground; _numbers.ForeColor = theme.NumberForeground;
        _list.BackColor = theme.NumberBackground; _list.ForeColor = theme.Foreground;
        _keywords = theme.Keywords;
        _busy = false;
        HideCompletion();
        Highlight();
    }

    /// <summary>Exécute un travail de mise en forme sans événements, sans scintillement, en conservant sélection et défilement.</summary>
    void Silent(Action work, int? selStart = null, int selLen = 0)
    {
        if (!_box.IsHandleCreated) return;
        bool wasBusy = _busy; _busy = true;
        var h = _box.Handle;
        int s = _box.SelectionStart, l = _box.SelectionLength;
        var scroll = new Point();
        SendMessage(h, EM_GETSCROLLPOS, IntPtr.Zero, ref scroll);
        SendMessage(h, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        var mask = SendMessage(h, EM_GETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
        SendMessage(h, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
        try { work(); }
        finally
        {
            _box.Select(selStart ?? s, selStart.HasValue ? selLen : l);
            SendMessage(h, EM_SETSCROLLPOS, IntPtr.Zero, ref scroll);
            SendMessage(h, EM_SETEVENTMASK, IntPtr.Zero, mask);
            SendMessage(h, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            _box.Invalidate(); _box.Update();
            _numbers.Invalidate();
            if (IsHandleCreated) BeginInvoke(new Action(() => _numbers.Invalidate()));
            _busy = wasBusy;
        }
    }

    void Highlight()
    {
        if (_theme == null || _busy || !_box.IsHandleCreated) return;
        Silent(() =>
        {
            string text = _box.Text;
            _box.Select(0, text.Length);
            _box.SelectionColor = _theme.Foreground;
            _box.SelectionFont = _theme.Font;
            foreach (var rule in _theme.Rules)
                foreach (Match m in rule.Regex.Matches(text))
                {
                    if (m.Length == 0) continue;
                    _box.Select(m.Index, m.Length);
                    _box.SelectionColor = rule.Color;
                    if (rule.Font != _theme.Font) _box.SelectionFont = rule.Font;
                }
        });
    }

    // ===================== Structure : plan + zones repliables =====================
    void EnsureStructure() { if (_structureDirty) RefreshStructure(); }

    void RefreshStructure()
    {
        _structureDirty = false;
        string text = _box.Text;
        var nl = _nl; nl.Clear();
        for (int i = 0; i < text.Length; i++) if (text[i] == '\n') nl.Add(i);
        _nlDirty = false;
        int LineAt(int idx) { int k = nl.BinarySearch(idx); return k < 0 ? ~k : k; }

        // Analyse « Forth » : commentaires, chaînes (." … ") et mots cités sont ignorés
        var scan = ForthScanner.Scan(text);
        var items = scan.Definitions
            .Select(d => new OutlineItem { Name = d.Name, Kind = d.Kind, Line = LineAt(d.Index) })
            .ToList();

        var colons = scan.Definitions.Where(d => d.Kind == ":").ToList();
        var regions = new List<FoldRegion>();
        for (int c = 0; c < colons.Count; c++)
        {
            var d = colons[c];
            if (!d.AtLineStart) continue;                               // seules les définitions en début de ligne se replient
            int h1 = text.IndexOf('\n', d.Index);
            if (h1 < 0) continue;
            int k = scan.Semicolons.BinarySearch(d.NameEnd);
            if (k < 0) k = ~k;
            if (k >= scan.Semicolons.Count) continue;                   // pas de « ; »
            int endIdx = scan.Semicolons[k];
            if (endIdx < h1) continue;                                  // définition sur une seule ligne
            if (c + 1 < colons.Count && endIdx > colons[c + 1].Index) continue;   // définition non terminée
            int e = text.IndexOf('\n', endIdx);
            if (e < 0) e = text.Length;
            regions.Add(new FoldRegion { HeaderLine = LineAt(d.Index), ClosingLine = LineAt(endIdx), HideStart = h1, HideEnd = e });
        }
        foreach (var r in regions)
            r.Folded = _regions.Any(o => o.Folded && o.HideStart == r.HideStart && o.HideEnd == r.HideEnd);

        _regions = regions;
        _anyFolded = regions.Any(r => r.Folded);
        _outline = items;
        _docWords = items.Select(i => i.Name).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        _numbers.SetRegions(regions);
        StructureChanged?.Invoke();
    }

    public void GoToLine(int line)
    {
        _box.Select(LineStart(line), 0);
        _box.ScrollToCaret();
        _box.Focus();
    }

    // ===================== Repliement =====================
    void SetHidden(bool hidden)
    {
        var cf = new CHARFORMAT2
        {
            cbSize = Marshal.SizeOf<CHARFORMAT2>(), dwMask = CFM_HIDDEN, dwEffects = hidden ? CFE_HIDDEN : 0u, szFaceName = ""
        };
        SendMessage(_box.Handle, EM_SETCHARFORMAT, (IntPtr)SCF_SELECTION, ref cf);
    }

    void SetFolded(IEnumerable<FoldRegion> regions, bool fold)
    {
        if (!_box.IsHandleCreated) return;
        var list = regions.Where(r => r.Folded != fold).ToList();
        if (list.Count == 0) return;
        int caret = _box.SelectionStart;
        int? newSel = null;
        if (fold)
            foreach (var r in list)
                if (caret > r.HideStart && caret < r.HideEnd) { newSel = r.HideStart; break; }
        Silent(() =>
        {
            foreach (var r in list) { _box.Select(r.HideStart, r.HideEnd - r.HideStart); SetHidden(fold); }
        }, newSel, 0);
        foreach (var r in list) r.Folded = fold;
        _anyFolded = _regions.Any(r => r.Folded);
        _numbers.Invalidate();
    }

    /// <summary>Retire tout masquage, même si les régions mémorisées sont périmées.</summary>
    void ClearAllFolds()
    {
        Silent(() => { _box.SelectAll(); SetHidden(false); });
        foreach (var r in _regions) r.Folded = false;
        _anyFolded = false;
        _numbers.Invalidate();
    }

    public void UnfoldAll() { if (_anyFolded) ClearAllFolds(); }
    public void FoldAll() { EnsureStructure(); SetFolded(_regions, true); }

    public void ToggleFoldAtCaret()
    {
        EnsureStructure();
        int line = LineFromIndex(_box.SelectionStart);
        var r = _regions.FirstOrDefault(x => x.HeaderLine == line)
                ?? _regions.FirstOrDefault(x => x.HeaderLine < line && line <= x.ClosingLine);
        if (r != null) SetFolded(new[] { r }, !r.Folded);
    }

    void ToggleFoldAtLine(int line)
    {
        EnsureStructure();
        var r = _regions.FirstOrDefault(x => x.HeaderLine == line);
        if (r != null) SetFolded(new[] { r }, !r.Folded);
    }

    void OnSelectionChanged()
    {
        if (_busy || _loading) return;
        if (_anyFolded)
        {
            int p = _box.SelectionStart, q = p + _box.SelectionLength;
            var hit = _regions.Where(r => r.Folded &&
                ((p > r.HideStart && p < r.HideEnd) || (q > r.HideStart && q < r.HideEnd))).ToList();
            if (hit.Count > 0) SetFolded(hit, false);     // le curseur entre dans un bloc replié : on le révèle
        }
        if (_list.Visible)
        {
            string pre = WordBeforeCaret(out int st);
            if (pre.Length == 0 || st != _compStart) HideCompletion();
        }
    }

    // ===================== Complétion =====================
    string WordBeforeCaret(out int start)
    {
        start = -1;
        if (_box.SelectionLength > 0) return "";
        int caret = _box.SelectionStart;
        string text = _box.Text;
        if (caret < text.Length && !char.IsWhiteSpace(text[caret])) return "";      // le curseur doit être en fin de mot
        int i = caret;
        while (i > 0 && !char.IsWhiteSpace(text[i - 1])) i--;
        start = i;
        return text.Substring(i, caret - i);
    }

    List<string> Candidates(string prefix)
    {
        bool lower = prefix == prefix.ToLowerInvariant() && prefix.Any(char.IsLetter);
        var res = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { prefix };
        foreach (var w in _docWords)
            if (w.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && seen.Add(w)) res.Add(w);
        foreach (var k in _keywords)
            if (k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && seen.Add(k)) res.Add(lower ? k.ToLowerInvariant() : k);
        return res;
    }

    void ShowCompletion(bool force)
    {
        string prefix = WordBeforeCaret(out int start);
        if (prefix.Length < (force ? 1 : 2)) { HideCompletion(); return; }
        var matches = Candidates(prefix);
        if (matches.Count == 0) { HideCompletion(); return; }

        _compStart = start;
        _list.BeginUpdate();
        _list.Items.Clear();
        _list.Items.AddRange(matches.Take(300).Cast<object>().ToArray());
        _list.SelectedIndex = 0;
        _list.EndUpdate();

        var pt = _box.GetPositionFromCharIndex(start);
        int w = Math.Min(340, Math.Max(140, matches.Take(300).Max(s => TextRenderer.MeasureText(s, _list.Font).Width) + 32));
        int h = Math.Min(8, matches.Count) * _list.ItemHeight + 4;
        int x = _box.Left + pt.X, y = _box.Top + pt.Y + _box.Font.Height + 2;
        if (y + h > Height) y = _box.Top + pt.Y - h - 2;
        if (x + w > Width) x = Math.Max(0, Width - w);
        _list.SetBounds(x, y, w, h);
        _list.Visible = true;
        _list.BringToFront();
    }

    void HideCompletion() { if (_list.Visible) _list.Visible = false; }

    void AcceptCompletion()
    {
        if (!_list.Visible || _list.SelectedItem is not string word) { HideCompletion(); return; }
        int caret = _box.SelectionStart;
        _accepting = true;
        _box.Select(_compStart, caret - _compStart);
        _box.SelectedText = word;
        _accepting = false;
        HideCompletion();
    }

    bool HandleKey(KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Space) { ShowCompletion(true); return true; }
        if (!_list.Visible) return false;
        switch (e.KeyCode)
        {
            case Keys.Down: MoveSelection(1); return true;
            case Keys.Up: MoveSelection(-1); return true;
            case Keys.PageDown: MoveSelection(8); return true;
            case Keys.PageUp: MoveSelection(-8); return true;
            case Keys.Enter:
            case Keys.Tab:
                if (e.Shift && e.KeyCode == Keys.Tab) { HideCompletion(); return false; }   // Maj+Tab : désindentation
                AcceptCompletion(); return true;
            case Keys.Escape: HideCompletion(); return true;
        }
        return false;
    }

    void MoveSelection(int delta)
    {
        int n = _list.Items.Count;
        if (n > 0) _list.SelectedIndex = Math.Clamp(_list.SelectedIndex + delta, 0, n - 1);
    }
}
