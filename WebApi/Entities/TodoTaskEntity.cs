using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class TodoTaskEntity
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue { get; set; }

    [ForeignKey("TodoList")]
    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; }
}
