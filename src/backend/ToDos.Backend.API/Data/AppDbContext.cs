using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDos.Backend.API.Models;

namespace ToDos.Backend.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
