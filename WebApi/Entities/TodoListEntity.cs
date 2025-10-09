namespace WebApi.Entities;

public class TodoListEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ICollection<TodoTaskEntity> Tasks { get; set; } = new List<TodoTaskEntity>();
}
