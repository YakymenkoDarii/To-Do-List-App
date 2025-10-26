using WebApp.DataClasses;

namespace WebApp.Services.Interfaces;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTask>> GetAllAsync();

    Task<TodoTask?> GetByIdAsync(int id);

    Task CreateAsync(TodoTask todoTask);

    Task UpdateAsync(TodoTask todoTask);

    Task DeleteAsync(int id);

    Task<List<TodoTask>> GetTasksForUserAsync(string userId);
}
