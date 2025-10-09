using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataClasses;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

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
        return this.View(lists);
    }

    public IActionResult Create() => this.View();

    [HttpPost]
    public async Task<IActionResult> Create(TodoList model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.service.CreateAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var list = await this.service.GetByIdAsync(id);
        return list == null ? this.NotFound() : this.View(list);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TodoList model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.service.UpdateAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await this.service.DeleteAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }
}
