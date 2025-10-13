using Microsoft.AspNetCore.Mvc;
using WebApp.DataClasses;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService service;

    public TodoTaskController(ITodoTaskWebApiService service)
    {
        this.service = service;
    }

    // GET: /TodoTask/Index/5 (or /TodoTask/Details/5)
    // Shows the details for a single task
    public async Task<IActionResult> Index(int id)
    {
        var dataTask = await this.service.GetByIdAsync(id);
        if (dataTask == null)
        {
            return this.NotFound();
        }

        // Map the data object to the view model
        var viewModel = new TodoTaskWebApiModel
        {
            Id = dataTask.Id,
            Title = dataTask.Title,
            Description = dataTask.Description,
            DueTo = dataTask.DueTo,
            IsOverdue = dataTask.IsOverdue,
            TodoListId = dataTask.TodoListId,
        };

        return this.View(viewModel);
    }

    // GET: /TodoTask/Create?todoListId=1
    public IActionResult Create(int todoListId)
    {
        // Pass the parent TodoListId to the view so it knows which list this new task belongs to
        var viewModel = new TodoTaskWebApiModel
        {
            TodoListId = todoListId,
            DueTo = DateTime.Today, // Pre-populate the date to today
        };
        return this.View(viewModel);
    }

    // POST: /TodoTask/Create
    [HttpPost]
    public async Task<IActionResult> Create(TodoTaskWebApiModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        // Map the view model back to the data class before sending to the service
        var dataTask = new TodoTask
        {
            Title = model.Title,
            Description = model.Description,
            DueTo = model.DueTo,
            TodoListId = model.TodoListId,
        };

        await this.service.CreateAsync(dataTask);

        // Redirect back to the parent list's details page
        return this.RedirectToAction("Details", "TodoList", new { id = model.TodoListId });
    }

    // GET: /TodoTask/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var dataTask = await this.service.GetByIdAsync(id);
        if (dataTask == null)
        {
            return this.NotFound();
        }

        // Map the data object to the view model
        var viewModel = new TodoTaskWebApiModel
        {
            Id = dataTask.Id,
            Title = dataTask.Title,
            Description = dataTask.Description,
            DueTo = dataTask.DueTo,
            IsOverdue = dataTask.IsOverdue,
            TodoListId = dataTask.TodoListId,
        };

        return this.View(viewModel);
    }

    // POST: /TodoTask/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(TodoTaskWebApiModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        // Map the view model back to the data class
        var dataTask = new TodoTask
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            DueTo = model.DueTo,
            TodoListId = model.TodoListId,
        };

        await this.service.UpdateAsync(dataTask);

        // Redirect back to the parent list's details page
        return this.RedirectToAction("Details", "TodoList", new { id = model.TodoListId });
    }

    // GET: /TodoTask/Delete/5
    // Shows a confirmation page before deleting
    public async Task<IActionResult> Delete(int id)
    {
        // 1. Get the task from the service to find its parent list ID.
        var taskToDelete = await this.service.GetByIdAsync(id);
        if (taskToDelete == null)
        {
            return this.NotFound();
        }

        // 2. Perform the delete operation.
        await this.service.DeleteAsync(id);

        // 3. Redirect back to the correct parent list's details page.
        return this.RedirectToAction("Details", "TodoList", new { id = taskToDelete.TodoListId });
    }
}
