namespace TaskManager.Models;

public enum Priority
{
    Low,
    Medium,
    High
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Medium;
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; private set; }

    public TaskItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public bool IsOverdue =>
        DueDate.HasValue &&
        DueDate.Value.Date < DateTime.Today &&
        Status != TaskStatus.Completed;

    public override string ToString() => Title;
}