using TaskManager.Helpers;
using TaskManager.Models;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Forms;

public partial class TaskDialog
{
    // ── Control fields ─────────────────────────────
    private TextBox        _txtTitle       = null!;
    private TextBox        _txtDescription = null!;
    private ComboBox       _cmbPriority    = null!;
    private ComboBox       _cmbStatus      = null!;
    private CheckBox       _chkDueDate     = null!;
    private DateTimePicker _dtpDue         = null!;
    private Button         _btnSave        = null!;
    private Button         _btnCancel      = null!;

    private void InitializeComponent()
    {
        // ── Form ──────────────────────────────────
        Text            = _isEdit ? "Edit Task" : "New Task";
        Size            = new Size(420, 400);
        MinimumSize     = Size;
        MaximumSize     = Size;
        StartPosition   = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        BackColor       = Color.White;
        Padding         = new Padding(20);

        // ── Controls ──────────────────────────────
        _txtTitle       = UIFactory.CreateTextBox("Enter task title...");
        _txtDescription = UIFactory.CreateTextBox(
            "Enter description...", TextBoxType.MultiLine, 250, 70);

        _cmbPriority = UIFactory.CreateComboBox(Enum.GetNames<Priority>());
        _cmbStatus   = UIFactory.CreateComboBox(Enum.GetNames<TaskStatus>());
        _cmbStatus.Enabled = _isEdit;

        _chkDueDate = new CheckBox
        {
            Text     = "Set due date",
            Font     = new Font("Segoe UI", 9.5f),
            AutoSize = true
        };

        _dtpDue = new DateTimePicker
        {
            Format  = DateTimePickerFormat.Long,
            Width   = 200,
            Enabled = false,
            Font    = new Font("Segoe UI", 9.5f)
        };

        _btnSave   = UIFactory.CreateButton(
            _isEdit ? "Save Changes" : "Add Task",
            ButtonType.Primary, 120);

        _btnCancel = UIFactory.CreateButton(
            "Cancel", ButtonType.Secondary, 80);

        // ── Layout ────────────────────────────────
        var layout = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            RowCount    = 7,
            BackColor   = Color.White
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  100f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));

        layout.Controls.Add(UIFactory.CreateLabel("Title *",  LabelType.Muted), 0, 0);
        layout.Controls.Add(_txtTitle,       1, 0);
        layout.Controls.Add(UIFactory.CreateLabel("Notes",    LabelType.Muted), 0, 1);
        layout.Controls.Add(_txtDescription, 1, 1);
        layout.Controls.Add(UIFactory.CreateLabel("Priority", LabelType.Muted), 0, 2);
        layout.Controls.Add(_cmbPriority,    1, 2);
        layout.Controls.Add(UIFactory.CreateLabel("Status",   LabelType.Muted), 0, 3);
        layout.Controls.Add(_cmbStatus,      1, 3);
        layout.Controls.Add(UIFactory.CreateLabel("Due date", LabelType.Muted), 0, 4);
        layout.Controls.Add(_chkDueDate,     1, 4);
        layout.Controls.Add(new Label(),     0, 5);
        layout.Controls.Add(_dtpDue,         1, 5);

        // ── Buttons row ───────────────────────────
        var btnPanel = new FlowLayoutPanel
        {
            Dock          = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            BackColor     = Color.White,
            WrapContents  = false
        };
        btnPanel.Controls.Add(_btnCancel);
        btnPanel.Controls.Add(_btnSave);

        layout.Controls.Add(btnPanel, 0, 6);
        layout.SetColumnSpan(btnPanel, 2);

        Controls.Add(layout);
    }
}