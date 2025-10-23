using System.ComponentModel.DataAnnotations;

namespace WebApp.DataClasses;

public enum TaskStatus
{
    [Display(Name = "Not Started")]
    NotStarted,

    [Display(Name = "In Progress")]
    InProgress,

    Completed
}

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue => DueTo < DateTime.UtcNow && Status != TaskStatus.Completed;

    public DateTime CreatedAt { get; set; }

    public TaskStatus Status { get; set; }

    public string AssignedToId { get; set; }

    public int TodoListId { get; set; }

    public TodoList TodoList { get; set; }
}
