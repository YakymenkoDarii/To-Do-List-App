using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DataClass;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskDatabaseService todoTaskDatabaseService;

    public TodoTaskController(ITodoTaskDatabaseService todoTaskDatabaseService)
    {
        this.todoTaskDatabaseService = todoTaskDatabaseService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoTaskModel>> GetAll()
    {
        var data = this.todoTaskDatabaseService.GetAll();
        var models = data.Select(item => new TodoTaskModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            DueTo = item.DueTo,
            CreatedAt = item.CreatedAt,
            Status = item.Status,
            TodoListId = item.TodoListId,
            AssignedToId = item.AssignedToId,
        }).ToList();

        return this.Ok(models);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TodoTaskModel> GetById(int id)
    {
        var todoTask = this.todoTaskDatabaseService.GetById(id);

        if (todoTask == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskModel
        {
            Id = todoTask.Id,
            Title = todoTask.Title,
            Description = todoTask.Description,
            DueTo = todoTask.DueTo,
            CreatedAt = todoTask.CreatedAt,
            Status = todoTask.Status,
            TodoListId = todoTask.TodoListId,
            AssignedToId = todoTask.AssignedToId,
        };

        return this.Ok(model);
    }

    [HttpGet("/api/todolist/{listId:int}/tasks")]
    public ActionResult<IEnumerable<TodoTaskModel>> GetTasksForList(int listId)
    {
        var tasks = this.todoTaskDatabaseService.GetByListId(listId);

        if (tasks == null)
        {
            return this.NotFound();
        }

        var models = tasks.Select(model => new TodoTaskModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            DueTo = model.DueTo,
            CreatedAt = model.CreatedAt,
            Status = model.Status,
            TodoListId = model.TodoListId,
            AssignedToId = model.AssignedToId,
        }).ToList();

        return this.Ok(models);
    }

    [HttpPost]
    public ActionResult<TodoTaskModel> Create(TodoTaskModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var task = new TodoTask
        {
            Title = model.Title,
            Description = model.Description,
            DueTo = model.DueTo,
            IsOverdue = model.IsOverdue,
            CreatedAt = model.CreatedAt,
            Status = model.Status,
            TodoListId = model.TodoListId,
            AssignedToId = model.AssignedToId,
        };

        this.todoTaskDatabaseService.Add(task);
        model.Id = task.Id;

        return this.CreatedAtAction(nameof(this.GetById), new { id = task.Id }, model);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        bool deleted = this.todoTaskDatabaseService.Delete(id);
        if (!deleted)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, TodoTaskModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todo = new TodoTask
        {
            Id = id,
            Title = model.Title,
            Description = model.Description,
            DueTo = model.DueTo,
            IsOverdue = model.IsOverdue,
            CreatedAt = model.CreatedAt,
            Status = model.Status,
            TodoListId = model.TodoListId,
            AssignedToId = model.AssignedToId,
        };

        this.todoTaskDatabaseService.Update(todo);

        return this.NoContent();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TodoTask>>> GetTasksForUser(string userId)
    {
        var tasks = this.todoTaskDatabaseService.GetAll();

        var models = tasks.Select(item => new TodoTaskModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            DueTo = item.DueTo,
            CreatedAt = item.CreatedAt,
            Status = item.Status,
            TodoListId = item.TodoListId,
            AssignedToId = item.AssignedToId,
        }).Where(task => task.AssignedToId == userId).ToList();

        return this.Ok(models);
    }
}
