using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities;

namespace Todo_List.Infrastructure
{
    public class TodoListDbContext : DbContext
    {
        public virtual DbSet<Commitment> Tasks { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
    }
}
