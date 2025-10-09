using WebApp.DataClasses;

namespace WebApp.Services.Interfaces;

public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoList>> GetAllAsync();

    Task<TodoList?> GetByIdAsync(int id);

    Task CreateAsync(TodoList todoList);

    Task UpdateAsync(TodoList todoList);

    Task DeleteAsync(int id);
}
