using WebApi.DataClass;

namespace WebApi.Services.Interfaces;

public interface ITodoListDatabaseService
{
    IEnumerable<TodoList> GetAll();

    TodoList GetById(int id);

    void Add(TodoList todoList);

    bool Delete(int id);

    void Update(TodoList todoList);
}
