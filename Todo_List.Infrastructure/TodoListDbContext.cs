using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure.Entities.Commitments;
using Todo_List.Infrastructure.Entities.Commitments.Abstract;

namespace Todo_List.Infrastructure
{
    public class TodoListDbContext : DbContext
    {
        public virtual DbSet<Commitment> Tasks { get; set; }
        public virtual DbSet<OneTimeCommitment> OneTimeCommitments { get; set; }
        public virtual DbSet<RecurringCommitment> RecurringCommitments { get; set; }
        public virtual DbSet<UnscheduledCommitment> UnScheduledCommitments { get; set; }

        public TodoListDbContext(DbContextOptions options) :base(options)
        {

        }
    }
}


