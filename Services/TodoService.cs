using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService(AppDbContext context) : ITodoService
{
    public async Task<Todo> CreateTodoAsync(int userId, string title)
    {
        var todo = new Todo
        {
            UserId = userId,
            Title = title
        };
        await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return todo;
    }

    public async Task<IEnumerable<Todo>> GetTodosByUserAsync(int userId)
    {
        var todos = context.Todos.Where(t => t.UserId == userId).ToListAsync();
        return await todos;
    }

    public async Task<Todo?> GetTodoByIdAsync(int id, int userId)
    {
        
        var todo =  await context.Todos.FirstOrDefaultAsync(t => t.Id == id  && t.UserId == userId);
        return todo;
    }



    public async Task<Todo?> UpdateTodoAsync(int id, int userId, string title, bool isCompleted)
    {
        var todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null) return null;
        todo.UserId = userId;
        todo.IsCompleted = isCompleted;
        todo.Title = title;
        await context.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> DeleteTodoAsync(int id, int userId)
    {
        var todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id  && t.UserId == userId);
        if (todo == null) return false;
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
        return true;
    }
}