namespace WpfWorkManager.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TaskContext : DbContext
    {
        public TaskContext()
            : base("name=TaskContext")
        {
        }

        public virtual DbSet<Command> Commands { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueCondition> IssueConditions { get; set; }
        public virtual DbSet<LoginModel> LoginModels { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .HasMany(e => e.IssueConditions)
                .WithRequired(e => e.Issue)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sprint>()
                .HasMany(e => e.Issues)
                .WithRequired(e => e.Sprint)
                .WillCascadeOnDelete(false);
        }
    }
}
