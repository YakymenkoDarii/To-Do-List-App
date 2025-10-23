using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataClasses;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize]
public class TodoListController : Controller
{
    private ITodoListWebApiService service;

    public TodoListController(ITodoListWebApiService service)
    {
        this.service = service;
    }

    public async Task<IActionResult> Index()
    {
        var lists = await this.service.GetAllAsync();
        return this.View(lists.Where(list => list.OwnerId == this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }

    public IActionResult Create() => this.View();

    [HttpPost]
    public async Task<IActionResult> Create(TodoListWebApiModel model) // Use ViewModel here
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        // Map from ViewModel to Data Class
        var todoList = new TodoList
        {
            Title = model.Title,
            Description = model.Description,
            OwnerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
        };

        await this.service.CreateAsync(todoList);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var list = await this.service.GetByIdAsync(id);
        if (list == null)
        {
            return this.NotFound();
        }

        // Map from Data Class to ViewModel for the view
        var model = new TodoListWebApiModel
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
        };

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TodoListWebApiModel model) // Use ViewModel here
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        // Map from ViewModel to Data Class
        var todoList = new TodoList
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
        };

        await this.service.UpdateAsync(todoList);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await this.service.DeleteAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var todoListWithTasks = await this.service.GetByIdAsync(id); // Assuming this returns the list with its tasks

        if (todoListWithTasks == null)
        {
            return NotFound();
        }

        // 2. Map the data to your new, specific ViewModel
        var viewModel = new TodoListDetailsViewModel
        {
            Id = todoListWithTasks.Id,
            Title = todoListWithTasks.Title,
            Description = todoListWithTasks.Description,
            Tasks = todoListWithTasks.Tasks, // Pass the tasks along
        };

        // 3. Pass the new ViewModel to the view
        return this.View(viewModel);
    }
}
