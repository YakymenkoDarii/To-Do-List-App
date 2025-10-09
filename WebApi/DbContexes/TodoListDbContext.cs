using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DbContexes
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TodoListEntity> TodoLists => this.Set<TodoListEntity>();

        public DbSet<TodoTaskEntity> TodoTasks => this.Set<TodoTaskEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoListEntity>()
                .HasMany(list => list.Tasks)
                .WithOne(task => task.TodoList)
                .HasForeignKey(task => task.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
