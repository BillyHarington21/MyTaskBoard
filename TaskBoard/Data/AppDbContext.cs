using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Models;

namespace TaskBoard.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TaskWork> Tasks { get; set; }
        public DbSet<TaskFile> Files { get; set; }
        
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("AppUser");

            modelBuilder.Entity<Sprint>()
               .HasOne(s => s.Project)
               .WithMany(p => p.Sprints)
               .HasForeignKey(s => s.ProjectName);

            modelBuilder.Entity<Sprint>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sprints)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<TaskWork>()
                .HasOne(s => s.Sprint)
                .WithMany(t =>t.Tasks)
                .HasForeignKey(w =>w.SprintId);

            modelBuilder.Entity<TaskWork>()
                .HasMany(t => t.Files)
                .WithOne(t => t.Task)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

   
}
