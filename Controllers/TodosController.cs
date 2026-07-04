using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController(ITodoService todoService, ILogger<TodosController> logger) : ControllerBase
{
    // Helper to extract the UserId from the JWT token
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");

        return int.Parse(userIdClaim.Value);
    }

    // GET: /api/todos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        logger.LogInformation("User {UserId} requested all todos", userId);

        var todos = await todoService.GetTodosByUserAsync(userId);
        return Ok(todos);
    }

    // GET: /api/todos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        var todo = await todoService.GetTodoByIdAsync(id, userId);

        if (todo == null)
            return NotFound(new { message = "Todo not found" });

        return Ok(todo);
    }

    // POST: /api/todos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required" });

        var userId = GetUserId();
        var todo = await todoService.CreateTodoAsync(userId, request.Title);

        logger.LogInformation("User {UserId} created todo {TodoId}", userId, todo.Id);

        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    // PUT: /api/todos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoRequest request)
    {
        var userId = GetUserId();
        var todo = await todoService.UpdateTodoAsync(id, userId, request.Title, request.IsCompleted);

        if (todo == null)
            return NotFound(new { message = "Todo not found" });

        return Ok(todo);
    }

    // DELETE: /api/todos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var deleted = await todoService.DeleteTodoAsync(id, userId);

        if (!deleted)
            return NotFound(new { message = "Todo not found" });

        return NoContent();
    }
}

// Request DTOs
public class CreateTodoRequest
{
    public string Title { get; set; } = string.Empty;
}

public class UpdateTodoRequest
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}