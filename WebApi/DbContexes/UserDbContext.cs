using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DbContexes;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => this.Set<UserEntity>();


}
