using WebApp.DataClasses;

namespace WebApp.Models;

public class TodoListDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<TodoTask> Tasks { get; set; } = new();
}
