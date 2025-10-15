namespace WebApi.Models;

public class TodoTaskModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue => DueTo < DateTime.UtcNow && Status != Entities.TaskStatus.Completed;

    public DateTime CreatedAt { get; set; }

    public Entities.TaskStatus Status { get; set; }

    public int TodoListId { get; set; }
}
