using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetTodosByUserAsync(int userId);
    Task<Todo?> GetTodoByIdAsync(int id, int userId);
    Task<Todo> CreateTodoAsync(int userId, string title);
    Task<Todo?> UpdateTodoAsync(int id, int userId, string title, bool isCompleted);
    Task<bool> DeleteTodoAsync(int id, int userId);
}