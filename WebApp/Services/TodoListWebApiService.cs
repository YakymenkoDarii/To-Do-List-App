using WebApp.DataClasses;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class TodoListWebApiService : ITodoListWebApiService
{
    private const string ApiBaseRoute = "api/todolist";
    private readonly HttpClient httpClient;

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoList>> GetAllAsync()
    {
        var result = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoList>>(ApiBaseRoute);
        return result ?? Enumerable.Empty<TodoList>();
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await this.httpClient.GetFromJsonAsync<TodoList>($"{ApiBaseRoute}/{id}");
    }

    public async Task CreateAsync(TodoList todoList)
    {
        await this.httpClient.PostAsJsonAsync(ApiBaseRoute, todoList);
    }

    public async Task UpdateAsync(TodoList todoList)
    {
        await this.httpClient.PutAsJsonAsync($"{ApiBaseRoute}/{todoList.Id}", todoList);
    }

    public async Task DeleteAsync(int id)
    {
        await this.httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
    }
}
