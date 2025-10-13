using Microsoft.EntityFrameworkCore;
using WebApi.DataClass;
using WebApi.DbContexes;
using WebApi.Entities;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private TodoListDbContext context;

    public TodoTaskDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public void Add(TodoTask todoTask)
    {
        TodoTaskEntity entity = new TodoTaskEntity
        {
            Title = todoTask.Title,
            Description = todoTask.Description,
            IsOverdue = todoTask.IsOverdue,
            DueTo = todoTask.DueTo,
            TodoListId = todoTask.TodoListId,
        };

        this.context.Add(entity);
        this.context.SaveChanges();

        todoTask.Id = entity.Id;
    }

    public bool Delete(int id)
    {
        var entity = this.context.TodoTasks.Find(id);
        if (entity != null)
        {
            _ = this.context.TodoTasks.Remove(entity);
            _ = this.context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerable<TodoTask> GetAll()
    {
        return this.context.TodoTasks.Select(t => new TodoTask
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsOverdue = t.IsOverdue,
            DueTo = t.DueTo,
            TodoListId = t.TodoListId,
        }).ToList();
    }

    public TodoTask GetById(int id)
    {
        var entity = this.context.TodoTasks.Find(id);
        return new TodoTask
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueTo = entity.DueTo,
            IsOverdue = entity.IsOverdue,
            TodoListId = entity.TodoListId,
        };
    }

    public IEnumerable<TodoTask> GetByListId(int listId)
    {
        var list = this.context.TodoLists
            .Include(l => l.Tasks)
            .FirstOrDefault(l => l.Id == listId);

        return list.Tasks.Select(t => new TodoTask
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueTo = t.DueTo,
            IsOverdue = t.IsOverdue,
            TodoListId = t.TodoListId,
        }).ToList();
    }

    public void Update(TodoTask todoTask)
    {
        TodoTaskEntity entity = new TodoTaskEntity
        {
            Id = todoTask.Id,
            Title = todoTask.Title,
            Description = todoTask.Description,
            IsOverdue = todoTask.IsOverdue,
            DueTo = todoTask.DueTo,
            TodoListId = todoTask.TodoListId,
        };

        this.context.TodoTasks.Update(entity);
        this.context.SaveChanges();
    }
}
