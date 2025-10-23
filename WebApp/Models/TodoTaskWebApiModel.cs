using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class TodoTaskWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueTo { get; set; }

    public bool IsOverdue => DueTo < DateTime.UtcNow && Status != DataClasses.TaskStatus.Completed;

    public DateTime CreatedAt { get; set; }

    public DataClasses.TaskStatus Status { get; set; }

    [Required]
    [Display(Name = "Assign To")]
    public string AssignedToUserName { get; set; }

    public IEnumerable<SelectListItem> Users { get; set; }

    public int TodoListId { get; set; }
}
