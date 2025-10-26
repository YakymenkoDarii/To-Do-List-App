using WebApp.DataClasses;
using WebApp.Services.Base;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class TodoTaskWebApiService : AbstractService, ITodoTaskWebApiService
    {
        private const string ApiBaseRoute = "api/todotask";

        public TodoTaskWebApiService(IHttpClientFactory factory, IHttpContextAccessor httpAccessor)
            : base(factory, httpAccessor)
        {
        }

        public async Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            using var httpClient = this.CreateClient();
            var result = await httpClient.GetFromJsonAsync<IEnumerable<TodoTask>>(ApiBaseRoute);
            return result ?? Enumerable.Empty<TodoTask>();
        }

        public async Task<TodoTask?> GetByIdAsync(int id)
        {
            using var httpClient = this.CreateClient();
            return await httpClient.GetFromJsonAsync<TodoTask>($"{ApiBaseRoute}/{id}");
        }

        public async Task CreateAsync(TodoTask todoTask)
        {
            using var httpClient = this.CreateClient();
            await httpClient.PostAsJsonAsync(ApiBaseRoute, todoTask);
        }

        public async Task UpdateAsync(TodoTask todoTask)
        {
            using var httpClient = this.CreateClient();
            await httpClient.PutAsJsonAsync($"{ApiBaseRoute}/{todoTask.Id}", todoTask);
        }

        public async Task DeleteAsync(int id)
        {
            using var httpClient = this.CreateClient();
            await httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
        }

        public async Task<List<TodoTask>> GetTasksForUserAsync(string userId)
        {
            using var httpClient = this.CreateClient();

            var userTasks = await httpClient.GetFromJsonAsync<List<TodoTask>>($"{ApiBaseRoute}/user/{userId}");

            return userTasks ?? new List<TodoTask>();
        }
    }
}
