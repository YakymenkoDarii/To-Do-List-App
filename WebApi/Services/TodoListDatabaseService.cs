using Microsoft.EntityFrameworkCore;
using WebApi.DataClass;
using WebApi.DbContexes;
using WebApi.Entities;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private TodoListDbContext context;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public void Add(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        TodoListEntity entity = new TodoListEntity
        {
            Title = todoList.Title,
            Description = todoList.Description,
            Tasks = todoList.Tasks.Select(t => new TodoTaskEntity
            {
                Title = t.Title,
                Description = t.Description,
                DueTo = t.DueTo,
                IsOverdue = t.IsOverdue,
            }).ToList(),
        };

        _ = this.context.Add(entity);
        _ = this.context.SaveChanges();

        todoList.Id = entity.Id;
        foreach (var pair in todoList.Tasks.Zip(entity.Tasks))
        {
            pair.First.Id = pair.Second.Id;
        }
    }

    public bool Delete(int id)
    {
        var entity = this.context.TodoLists.Find(id);
        if (entity != null)
        {
            _ = this.context.TodoLists.Remove(entity);
            _ = this.context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerable<TodoList> GetAll()
    {
        return this.context.TodoLists
            .Include(l => l.Tasks)
            .Select(l => new TodoList
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                Tasks = l.Tasks.Select(t => new TodoTask
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueTo = t.DueTo,
                    IsOverdue = t.IsOverdue,
                    TodoListId = t.TodoListId,
                }).ToList(),
            }).ToList();
    }

    public TodoList? GetById(int id)
    {
        var list = this.context.TodoLists
            .Include(l => l.Tasks)
            .FirstOrDefault(l => l.Id == id);

        if (list == null)
        {
            return null;
        }

        return new TodoList
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
            Tasks = list.Tasks.Select(t => new TodoTask
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueTo = t.DueTo,
                IsOverdue = t.IsOverdue,
                TodoListId = t.TodoListId,
            }).ToList(),
        };
    }

    public void Update(TodoList todoList)
    {
        TodoListEntity entity = new TodoListEntity
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,
            Tasks = todoList.Tasks.Select(t => new TodoTaskEntity
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueTo = t.DueTo,
                IsOverdue = t.IsOverdue,
            }).ToList(),
        };

        this.context.TodoLists.Update(entity);
        this.context.SaveChanges();
    }
}
