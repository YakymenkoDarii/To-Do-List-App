using WebApp.DataClasses;
using WebApp.Services.Base;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class TodoListWebApiService : AbstractService, ITodoListWebApiService
{
    private const string ApiBaseRoute = "api/todolist";

    public TodoListWebApiService(IHttpClientFactory factory, IHttpContextAccessor httpAccessor)
        : base(factory, httpAccessor)
    {
    }

    public async Task<IEnumerable<TodoList>> GetAllAsync()
    {
        using var httpClient = this.CreateClient();
        var result = await httpClient.GetFromJsonAsync<IEnumerable<TodoList>>(ApiBaseRoute);
        return result ?? Enumerable.Empty<TodoList>();
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        using var httpClient = this.CreateClient();
        return await httpClient.GetFromJsonAsync<TodoList>($"{ApiBaseRoute}/{id}");
    }

    public async Task CreateAsync(TodoList todoList)
    {
        using var httpClient = this.CreateClient();
        await httpClient.PostAsJsonAsync(ApiBaseRoute, todoList);
    }

    public async Task UpdateAsync(TodoList todoList)
    {
        using var httpClient = this.CreateClient();
        await httpClient.PutAsJsonAsync($"{ApiBaseRoute}/{todoList.Id}", todoList);
    }

    public async Task DeleteAsync(int id)
    {
        using var httpClient = this.CreateClient();
        await httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
    }
}
