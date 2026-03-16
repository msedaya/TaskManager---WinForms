namespace TaskManager.Helpers;

public enum ButtonType  { Primary, Secondary, Danger }
public enum LabelType   { Heading, Normal, Muted }
public enum TextBoxType { SingleLine, MultiLine }

internal static class UIFactory
{
    // ── Design tokens ──────────────────────────────
    private static readonly Font  DefaultFont  = new("Segoe UI", 9.5f);
    private static readonly Font  BoldFont     = new("Segoe UI", 9.5f, FontStyle.Bold);
    private static readonly Font  SmallFont    = new("Segoe UI", 8.5f);

    private static readonly Color AccentColor  = Color.FromArgb(79,  70,  229);
    private static readonly Color DangerColor  = Color.FromArgb(220, 38,   38);
    private static readonly Color BorderColor  = Color.FromArgb(229, 231, 235);
    private static readonly Color TextMuted    = Color.FromArgb(107, 114, 128);
    private static readonly Color TextNormal   = Color.FromArgb(55,   65,  81);
    private static readonly Color TextDark     = Color.FromArgb(17,   24,  39);
    private static readonly Color SurfaceColor = Color.FromArgb(249, 250, 251);

    // ── Button ─────────────────────────────────────
    public static Button CreateButton(
        string text,
        ButtonType type = ButtonType.Primary,
        int width       = 110)
    {
        var backColor = type switch
        {
            ButtonType.Primary => AccentColor,
            _                  => Color.White
        };

        var foreColor = type switch
        {
            ButtonType.Primary   => Color.White,
            ButtonType.Secondary => TextNormal,
            ButtonType.Danger    => DangerColor,
            _                    => TextNormal
        };

        var borderColor = type switch
        {
            ButtonType.Primary   => AccentColor,
            ButtonType.Secondary => BorderColor,
            ButtonType.Danger    => Color.FromArgb(254, 202, 202),
            _                    => BorderColor
        };

        var btn = new Button
        {
            Text      = text,
            Width     = width,
            Height    = 32,
            BackColor = backColor,
            ForeColor = foreColor,
            FlatStyle = FlatStyle.Flat,
            Cursor    = Cursors.Hand,
            Font      = BoldFont,
            FlatAppearance =
            {
                BorderColor = borderColor,
                BorderSize  = type == ButtonType.Primary ? 0 : 1
            }
        };

        btn.Paint += (_, e) =>
        {
            e.Graphics.FillRectangle(
                new SolidBrush(btn.BackColor),
                btn.ClientRectangle);

            var sf = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(
                btn.Text,
                btn.Font,
                new SolidBrush(btn.ForeColor),
                btn.ClientRectangle,
                sf);

            if (type != ButtonType.Primary)
            {
                var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                e.Graphics.DrawRectangle(new Pen(borderColor), rect);
            }
        };

        return btn;
    }

    // ── Label ──────────────────────────────────────
    public static Label CreateLabel(string text, LabelType type = LabelType.Normal)
    {
        var foreColor = type switch
        {
            LabelType.Heading => TextDark,
            LabelType.Normal  => TextNormal,
            LabelType.Muted   => TextMuted,
            _                 => TextNormal
        };

        var font = type switch
        {
            LabelType.Heading => new Font("Segoe UI", 11f, FontStyle.Bold),
            LabelType.Normal  => DefaultFont,
            LabelType.Muted   => SmallFont,
            _                 => DefaultFont
        };

        return new Label
        {
            Text      = text,
            Font      = font,
            ForeColor = foreColor,
            AutoSize  = true
        };
    }

    // ── TextBox ────────────────────────────────────
    public static TextBox CreateTextBox(
        string placeholder  = "",
        TextBoxType type    = TextBoxType.SingleLine,
        int width           = 200,
        int height          = 28)
    {
        var txt = new TextBox
        {
            Font            = DefaultFont,
            Width           = width,
            Height          = height,
            BorderStyle     = BorderStyle.FixedSingle,
            BackColor       = Color.White,
            ForeColor       = TextNormal,
            PlaceholderText = placeholder
        };

        if (type == TextBoxType.MultiLine)
        {
            txt.Multiline  = true;
            txt.Height     = 80;
            txt.ScrollBars = ScrollBars.Vertical;
        }

        return txt;
    }

    // ── ComboBox ───────────────────────────────────
    public static ComboBox CreateComboBox(string[] items, int width = 140)
    {
        var cmb = new ComboBox
        {
            Font          = DefaultFont,
            Width         = width,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle     = FlatStyle.Flat,
            BackColor     = Color.White,
            ForeColor     = TextNormal
        };

        cmb.Items.AddRange(items);

        if (cmb.Items.Count > 0)
            cmb.SelectedIndex = 0;

        return cmb;
    }

    // ── ListBox ────────────────────────────────────
    public static ListBox CreateListBox()
    {
        return new ListBox
        {
            Font        = DefaultFont,
            Dock        = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor   = Color.White,
            ForeColor   = TextNormal,
            ItemHeight  = 36,
            DrawMode    = DrawMode.OwnerDrawFixed
        };
    }

    // ── Toolbar ────────────────────────────────────
    public static FlowLayoutPanel CreateToolbar()
    {
        return new FlowLayoutPanel
        {
            Dock          = DockStyle.Top,
            Height        = 50,
            BackColor     = SurfaceColor,
            Padding       = new Padding(10, 9, 0, 0),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents  = false
        };
    }

    // ── SplitContainer ─────────────────────────────
    public static SplitContainer CreateSplitContainer()
    {
        return new SplitContainer
        {
            Dock          = DockStyle.Fill,
            SplitterWidth = 4,
            BackColor     = BorderColor,
            Panel1MinSize = 220
        };
    }

    // ── Panel header ───────────────────────────────
    public static Panel CreatePanelHeader(string title)
    {
        var header = new Panel
        {
            Dock      = DockStyle.Top,
            Height    = 36,
            BackColor = SurfaceColor
        };

        header.Controls.Add(new Label
        {
            Text      = title,
            Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = TextMuted,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock      = DockStyle.Fill,
            Padding   = new Padding(12, 0, 0, 0)
        });

        return header;
    }

    // ── Detail grid ────────────────────────────────
    public static TableLayoutPanel CreateDetailGrid(int rows)
    {
        var table = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            RowCount    = rows,
            BackColor   = Color.White
        };

        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90f));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  100f));

        for (int i = 0; i < rows; i++)
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));

        return table;
    }

    // ── Status bar ─────────────────────────────────
    public static (StatusStrip bar, ToolStripStatusLabel label) CreateStatusBar()
    {
        var label = new ToolStripStatusLabel
        {
            Spring    = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Font      = SmallFont,
            ForeColor = TextMuted
        };

        var bar = new StatusStrip
        {
            BackColor  = SurfaceColor,
            SizingGrip = false,
            Dock       = DockStyle.Bottom
        };

        bar.Items.Add(label);

        return (bar, label);
    }
}