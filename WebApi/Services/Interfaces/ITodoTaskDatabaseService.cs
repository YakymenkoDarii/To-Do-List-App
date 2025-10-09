using WebApi.DataClass;

namespace WebApi.Services.Interfaces;

public interface ITodoTaskDatabaseService
{
    IEnumerable<TodoTask> GetAll();

    IEnumerable<TodoTask> GetByListId(int listId);

    TodoTask GetById(int id);

    void Add(TodoTask todoTask);

    bool Delete(int id);

    void Update(TodoTask todoTask);
}
