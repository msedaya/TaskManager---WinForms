using TaskManager.Models;

namespace TaskManager.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();

    public event EventHandler? TasksChanged;

    public void AddTask(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Task title cannot be empty.");

        _tasks.Add(task);
        OnTasksChanged();
    }

    public void UpdateTask(TaskItem updatedTask)
    {
        ArgumentNullException.ThrowIfNull(updatedTask);

        var existing = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id)
            ?? throw new InvalidOperationException("Task not found.");

        var index = _tasks.IndexOf(existing);
        _tasks[index] = updatedTask;
        OnTasksChanged();
    }

    public void DeleteTask(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new InvalidOperationException("Task not found.");

        _tasks.Remove(task);
        OnTasksChanged();
    }

    public IReadOnlyList<TaskItem> GetTasks(Models.TaskStatus? filter = null)
    {
        if (filter is null)
            return _tasks.AsReadOnly();

        return _tasks
            .Where(t => t.Status == filter)
            .ToList()
            .AsReadOnly();
    }

    public TaskItem? GetById(Guid id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    private void OnTasksChanged() =>
        TasksChanged?.Invoke(this, EventArgs.Empty);
}