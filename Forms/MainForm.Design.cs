using TaskManager.Helpers;

namespace TaskManager.Forms;

public partial class MainForm
{
    // ── Control fields ─────────────────────────────
    private Button               _btnAdd         = null!;
    private Button               _btnEdit        = null!;
    private Button               _btnDelete      = null!;
    private Button               _btnMarkDone    = null!;
    private ComboBox             _cmbFilter      = null!;
    private ListBox              _listBox        = null!;
    private Label                _lblTitle       = null!;
    private Label                _lblDescription = null!;
    private Label                _lblPriority    = null!;
    private Label                _lblStatus      = null!;
    private Label                _lblDueDate     = null!;
    private SplitContainer       _split          = null!;
    private ToolStripStatusLabel _statusLabel    = null!;

    private void InitializeComponent()
    {
        // ── Form ──────────────────────────────────
        Text          = "Task Manager";
        Size          = new Size(900, 620);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor     = Color.White;

        // ── Buttons ───────────────────────────────
        _btnAdd      = UIFactory.CreateButton("+ Add",     ButtonType.Primary,   100);
        _btnEdit     = UIFactory.CreateButton("Edit",      ButtonType.Secondary,  80);
        _btnDelete   = UIFactory.CreateButton("Delete",    ButtonType.Danger,     80);
        _btnMarkDone = UIFactory.CreateButton("Mark Done", ButtonType.Secondary,  95);

        // ── Filter ────────────────────────────────
        _cmbFilter = UIFactory.CreateComboBox(
            ["All tasks", "Pending", "In Progress", "Completed"]);

        // ── Toolbar ───────────────────────────────
        var toolbar = UIFactory.CreateToolbar();
        toolbar.Controls.Add(_btnAdd);
        toolbar.Controls.Add(_btnEdit);
        toolbar.Controls.Add(_btnDelete);
        toolbar.Controls.Add(_btnMarkDone);
        toolbar.Controls.Add(UIFactory.CreateLabel("  Filter:", LabelType.Muted));
        toolbar.Controls.Add(_cmbFilter);

        // ── Split container ───────────────────────
        _split = UIFactory.CreateSplitContainer();

        // ── Left panel ────────────────────────────
        _split.Panel1.BackColor = Color.White;

        _listBox = UIFactory.CreateListBox();

        _split.Panel1.Controls.Add(_listBox);
        _split.Panel1.Controls.Add(UIFactory.CreatePanelHeader("Tasks"));

        // ── Right panel ───────────────────────────
        _split.Panel2.BackColor = Color.White;
        _split.Panel2.Padding   = new Padding(20, 16, 20, 16);

        _lblTitle          = UIFactory.CreateLabel("Select a task", LabelType.Heading);
        _lblTitle.Dock     = DockStyle.Top;
        _lblTitle.Height   = 40;
        _lblTitle.AutoSize = false;

        var grid = UIFactory.CreateDetailGrid(4);

        _lblDescription = UIFactory.CreateLabel("", LabelType.Normal);
        _lblPriority    = UIFactory.CreateLabel("", LabelType.Normal);
        _lblStatus      = UIFactory.CreateLabel("", LabelType.Normal);
        _lblDueDate     = UIFactory.CreateLabel("", LabelType.Normal);

        grid.Controls.Add(UIFactory.CreateLabel("Description", LabelType.Muted), 0, 0);
        grid.Controls.Add(_lblDescription, 1, 0);
        grid.Controls.Add(UIFactory.CreateLabel("Priority",    LabelType.Muted), 0, 1);
        grid.Controls.Add(_lblPriority,    1, 1);
        grid.Controls.Add(UIFactory.CreateLabel("Status",      LabelType.Muted), 0, 2);
        grid.Controls.Add(_lblStatus,      1, 2);
        grid.Controls.Add(UIFactory.CreateLabel("Due date",    LabelType.Muted), 0, 3);
        grid.Controls.Add(_lblDueDate,     1, 3);

        _split.Panel2.Controls.Add(grid);
        _split.Panel2.Controls.Add(_lblTitle);

        // ── Status bar ────────────────────────────
        var (statusBar, statusLabel) = UIFactory.CreateStatusBar();
        _statusLabel = statusLabel;

        // ── Add to form ───────────────────────────
        Controls.Add(_split);
        Controls.Add(toolbar);
        Controls.Add(statusBar);
    }
}