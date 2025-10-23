using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataClass;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService todoListDatabaseService;

    public TodoListController(ITodoListDatabaseService databaseService)
    {
        this.todoListDatabaseService = databaseService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoListModel>> GetAll()
    {
        var data = this.todoListDatabaseService.GetAll();
        var models = data.Select(item => new TodoListModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            OwnerId = item.OwnerId,
            Tasks = item.Tasks.Select(t => new TodoTaskModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueTo = t.DueTo,
                TodoListId = t.TodoListId,
                AssignedToId = t.AssignedToId,
            }).ToList(),
        }).ToList();

        return this.Ok(models);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TodoListModel> GetById(int id)
    {
        var todoList = this.todoListDatabaseService.GetById(id);
        if (todoList == null)
        {
            return this.NotFound();
        }

        var model = new TodoListModel
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,
            OwnerId = todoList.OwnerId,
            Tasks = todoList.Tasks.Select(t => new TodoTaskModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueTo = t.DueTo,
                TodoListId = t.TodoListId,
                AssignedToId = t.AssignedToId,
            }).ToList(),
        };
        return this.Ok(model);
    }

    [HttpPost]
    public ActionResult<TodoListModel> Create([FromBody] TodoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todo = new TodoList
        {
            Title = model.Title,
            Description = model.Description,
            OwnerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
        };

        this.todoListDatabaseService.Add(todo);

        var resultModel = new TodoListModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            OwnerId = todo.OwnerId,
        };

        return this.CreatedAtAction(nameof(this.GetById), new { id = todo.Id }, resultModel);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        bool deleted = this.todoListDatabaseService.Delete(id);
        if (!deleted)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] TodoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todo = new TodoList
        {
            Id = id,
            Title = model.Title,
            Description = model.Description,
        };

        this.todoListDatabaseService.Update(todo);

        return this.NoContent();
    }
}
