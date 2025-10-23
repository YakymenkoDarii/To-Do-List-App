namespace WebApi.DataClass;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue { get; set; }

    public DateTime CreatedAt { get; set; }

    public Entities.TaskStatus Status { get; set; }

    public string AssignedToId { get; set; }

    public int TodoListId { get; set; }
}
