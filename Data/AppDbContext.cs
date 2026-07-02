using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
namespace TodoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Todo> Todos { get; set; } 


}