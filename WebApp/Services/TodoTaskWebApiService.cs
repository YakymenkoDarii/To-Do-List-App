using WebApp.DataClasses;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class TodoTaskWebApiService : ITodoTaskWebApiService
{
    private const string ApiBaseRoute = "api/todotask";
    private readonly HttpClient httpClient;

    public TodoTaskWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoTask>> GetAllAsync()
    {
        var result = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoTask>>(ApiBaseRoute);
        return result ?? Enumerable.Empty<TodoTask>();
    }

    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        return await this.httpClient.GetFromJsonAsync<TodoTask>($"{ApiBaseRoute}/{id}");
    }

    public async Task CreateAsync(TodoTask todoTask)
    {
        await this.httpClient.PostAsJsonAsync(ApiBaseRoute, todoTask);
    }

    public async Task UpdateAsync(TodoTask todoTask)
    {
        await this.httpClient.PutAsJsonAsync($"{ApiBaseRoute}/{todoTask.Id}", todoTask);
    }

    public async Task DeleteAsync(int id)
    {
        await this.httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
    }
}
