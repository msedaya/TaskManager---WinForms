using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.Services;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Forms;

public partial class MainForm : Form
{
    // ── Service ────────────────────────────────────
    private readonly TaskService _taskService = new();

    // ── Selected task ──────────────────────────────
    private TaskItem? _selectedTask;

    public MainForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    // ── Event wiring ───────────────────────────────
    private void WireUpEvents()
    {
        _btnAdd.Click      += BtnAdd_Click;
        _btnEdit.Click     += BtnEdit_Click;
        _btnDelete.Click   += BtnDelete_Click;
        _btnMarkDone.Click += BtnMarkDone_Click;

        _cmbFilter.SelectedIndexChanged += (_, _) => RefreshList();
        _listBox.DrawItem               += ListBox_DrawItem;
        _listBox.SelectedIndexChanged   += ListBox_SelectionChanged;

        _taskService.TasksChanged += (_, _) => RefreshList();

        Load += (_, _) =>
        {
            _split.Panel2MinSize    = 280;
            _split.SplitterDistance = 340;
            SeedData();
            RefreshList();
        };
    }

    // ── Seed demo data ─────────────────────────────
    private void SeedData()
    {
        _taskService.AddTask(new TaskItem
        {
            Title    = "Design homepage layout",
            Priority = Priority.High,
            Status   = TaskStatus.InProgress,
            DueDate  = DateTime.Today.AddDays(3)
        });
        _taskService.AddTask(new TaskItem
        {
            Title    = "Write unit tests",
            Priority = Priority.Medium,
            Status   = TaskStatus.Pending
        });
        _taskService.AddTask(new TaskItem
        {
            Title    = "Review pull requests",
            Priority = Priority.Low,
            Status   = TaskStatus.Pending,
            DueDate  = DateTime.Today.AddDays(1)
        });
        _taskService.AddTask(new TaskItem
        {
            Title    = "Fix login bug",
            Priority = Priority.High,
            Status   = TaskStatus.Completed
        });
    }

    // ── Refresh list ───────────────────────────────
    private void RefreshList()
    {
        var savedId = _selectedTask?.Id;

        var filterStatus = _cmbFilter.SelectedIndex switch
        {
            1 => (TaskStatus?)TaskStatus.Pending,
            2 => TaskStatus.InProgress,
            3 => TaskStatus.Completed,
            _ => null
        };

        var tasks = _taskService.GetTasks(filterStatus);

        _listBox.BeginUpdate();
        _listBox.Items.Clear();
        foreach (var task in tasks)
            _listBox.Items.Add(task);
        _listBox.EndUpdate();

        if (savedId.HasValue)
        {
            var match = _listBox.Items
                .Cast<TaskItem>()
                .FirstOrDefault(t => t.Id == savedId);
            if (match != null)
                _listBox.SelectedItem = match;
        }

        UpdateStatusBar();
    }

    // ── Update status bar ──────────────────────────
    private void UpdateStatusBar()
    {
        var all     = _taskService.GetTasks();
        var done    = all.Count(t => t.Status == TaskStatus.Completed);
        var overdue = all.Count(t => t.IsOverdue);

        _statusLabel.Text =
            $"Total: {all.Count}   " +
            $"Completed: {done}   " +
            $"Overdue: {overdue}";
    }

    // ── Add ────────────────────────────────────────
    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var dlg = new TaskDialog(_taskService);
        dlg.ShowDialog(this);
    }

    // ── Edit ───────────────────────────────────────
    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (_selectedTask is null)
        {
            MessageBox.Show("Please select a task to edit.",
                "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new TaskDialog(_taskService, _selectedTask);
        dlg.ShowDialog(this);
    }

    // ── Delete ─────────────────────────────────────
    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_selectedTask is null) return;

        var confirm = MessageBox.Show(
            $"Delete \"{_selectedTask.Title}\"?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

        if (confirm != DialogResult.Yes) return;

        try
        {
            _taskService.DeleteTask(_selectedTask.Id);
            _selectedTask = null;
            ShowDetail(null);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Mark Done ──────────────────────────────────
    private void BtnMarkDone_Click(object? sender, EventArgs e)
    {
        if (_selectedTask is null) return;

        _selectedTask.Status = TaskStatus.Completed;

        try
        {
            _taskService.UpdateTask(_selectedTask);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Selection changed ──────────────────────────
    private void ListBox_SelectionChanged(object? sender, EventArgs e)
    {
        _selectedTask = _listBox.SelectedItem as TaskItem;
        ShowDetail(_selectedTask);
    }

    // ── Show detail ────────────────────────────────
    private void ShowDetail(TaskItem? task)
    {
        if (task is null)
        {
            _lblTitle.Text       = "Select a task";
            _lblDescription.Text = "";
            _lblPriority.Text    = "";
            _lblStatus.Text      = "";
            _lblDueDate.Text     = "";
            return;
        }

        _lblTitle.Text       = task.Title;
        _lblDescription.Text = string.IsNullOrWhiteSpace(task.Description)
            ? "(no description)"
            : task.Description;

        _lblPriority.Text      = task.Priority.ToString();
        _lblPriority.ForeColor = task.Priority switch
        {
            Priority.High   => Color.FromArgb(185, 28,  28),
            Priority.Medium => Color.FromArgb(180, 83,   9),
            Priority.Low    => Color.FromArgb(21,  128, 61),
            _               => Color.FromArgb(55,   65, 81)
        };

        _lblStatus.Text       = task.Status.ToString();
        _lblDueDate.Text      = task.DueDate.HasValue
            ? task.DueDate.Value.ToString("MMM dd, yyyy") +
              (task.IsOverdue ? "  ⚠ Overdue" : "")
            : "—";
        _lblDueDate.ForeColor = task.IsOverdue
            ? Color.FromArgb(185, 28, 28)
            : Color.FromArgb(55, 65, 81);
    }

    // ── Draw list row ──────────────────────────────
    private void ListBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        var task     = (TaskItem)_listBox.Items[e.Index]!;
        var g        = e.Graphics;
        var bounds   = e.Bounds;
        var selected = (e.State & DrawItemState.Selected) != 0;

        g.FillRectangle(
            new SolidBrush(selected ? Color.FromArgb(238, 242, 255) : Color.White),
            bounds);

        var stripColor = task.Priority switch
        {
            Priority.High   => Color.FromArgb(220, 38,  38),
            Priority.Medium => Color.FromArgb(245, 158, 11),
            Priority.Low    => Color.FromArgb(34,  197, 94),
            _               => Color.Transparent
        };
        g.FillRectangle(
            new SolidBrush(stripColor),
            bounds.X, bounds.Y + 4, 4, bounds.Height - 8);

        var titleFont = new Font("Segoe UI", 9.5f,
            task.Status == TaskStatus.Completed
                ? FontStyle.Strikeout
                : FontStyle.Regular);

        var titleColor = task.Status == TaskStatus.Completed
            ? Color.FromArgb(156, 163, 175)
            : Color.FromArgb(17, 24, 39);

        g.DrawString(
            task.Title,
            titleFont,
            new SolidBrush(titleColor),
            new RectangleF(bounds.X + 14, bounds.Y, bounds.Width - 80, bounds.Height),
            new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming      = StringTrimming.EllipsisCharacter
            });

        var (badgeText, badgeBg, badgeFg) = task.Status switch
        {
            TaskStatus.Completed  => ("Done",    Color.FromArgb(220, 252, 231), Color.FromArgb(22,  101,  52)),
            TaskStatus.InProgress => ("Active",  Color.FromArgb(219, 234, 254), Color.FromArgb(30,   64, 175)),
            _                     => ("Pending", Color.FromArgb(243, 244, 246), Color.FromArgb(75,   85,  99))
        };

        var badgeRect = new RectangleF(bounds.Right - 72, bounds.Y + 9, 60, 18);
        g.FillRectangle(new SolidBrush(badgeBg), badgeRect);
        g.DrawString(
            badgeText,
            new Font("Segoe UI", 7.5f, FontStyle.Bold),
            new SolidBrush(badgeFg),
            badgeRect,
            new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });

        g.DrawLine(
            new Pen(Color.FromArgb(243, 244, 246)),
            bounds.Left, bounds.Bottom - 1,
            bounds.Right, bounds.Bottom - 1);
    }
}