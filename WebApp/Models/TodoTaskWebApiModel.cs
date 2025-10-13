namespace WebApp.Models;

public class TodoTaskWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue { get; set; }

    public int TodoListId { get; set; }
}
