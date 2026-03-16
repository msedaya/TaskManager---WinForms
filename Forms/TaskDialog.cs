using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.Services;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Forms;

public partial class TaskDialog : Form
{
    // ── State ──────────────────────────────────────
    private readonly TaskService _taskService;
    private readonly TaskItem?   _existing;
    private readonly bool        _isEdit;

    public TaskDialog(TaskService taskService, TaskItem? existing = null)
    {
        _taskService = taskService;
        _existing    = existing;
        _isEdit      = existing != null;

        InitializeComponent();
        WireUpEvents();

        if (_isEdit) PopulateFields(_existing!);
    }

    // ── Event wiring ───────────────────────────────
    private void WireUpEvents()
    {
        _chkDueDate.CheckedChanged += (_, _) =>
            _dtpDue.Enabled = _chkDueDate.Checked;

        _btnSave.Click   += BtnSave_Click;
        _btnCancel.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        AcceptButton = _btnSave;
        CancelButton = _btnCancel;
    }

    // ── Populate fields ────────────────────────────
    private void PopulateFields(TaskItem task)
    {
        _txtTitle.Text            = task.Title;
        _txtDescription.Text      = task.Description;
        _cmbPriority.SelectedItem = task.Priority.ToString();
        _cmbStatus.SelectedItem   = task.Status.ToString();

        if (task.DueDate.HasValue)
        {
            _chkDueDate.Checked = true;
            _dtpDue.Value       = task.DueDate.Value;
        }
    }

    // ── Save ───────────────────────────────────────
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtTitle.Text))
        {
            MessageBox.Show("Please enter a task title.",
                "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _txtTitle.Focus();
            return;
        }

        try
        {
            var priority = Enum.Parse<Priority>(_cmbPriority.SelectedItem!.ToString()!);
            var status   = Enum.Parse<TaskStatus>(_cmbStatus.SelectedItem!.ToString()!);
            var dueDate  = _chkDueDate.Checked ? _dtpDue.Value.Date : (DateTime?)null;

            if (_isEdit)
            {
                _existing!.Title       = _txtTitle.Text.Trim();
                _existing.Description  = _txtDescription.Text.Trim();
                _existing.Priority     = priority;
                _existing.Status       = status;
                _existing.DueDate      = dueDate;
                _taskService.UpdateTask(_existing);
            }
            else
            {
                _taskService.AddTask(new TaskItem
                {
                    Title       = _txtTitle.Text.Trim(),
                    Description = _txtDescription.Text.Trim(),
                    Priority    = priority,
                    DueDate     = dueDate
                });
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}