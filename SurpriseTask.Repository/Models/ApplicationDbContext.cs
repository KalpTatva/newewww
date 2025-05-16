using Microsoft.EntityFrameworkCore;

namespace SurpriseTask.Repository.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public ApplicationDbContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=SurpriseTask;Username=postgres;password=tatva123");
        }
    }

    public DbSet<Users> Users { get; set; }   
    public DbSet<Course> Courses { get; set; }   
    public DbSet<UserCourseMapping> UserCourseMapping { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCourseMapping>()
                    .HasKey(x => new {x.MappingId});

        modelBuilder.Entity<UserCourseMapping>()
                    .HasOne(x => x.Users)
                    .WithMany(x => x.UserCourseMapping) 
                    .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<UserCourseMapping>()
                    .HasOne(x => x.Courses)
                    .WithMany(x => x.UserCourseMapping) 
                    .HasForeignKey(x => x.CourseId);

        modelBuilder.Entity<Course>()
                    .Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

        modelBuilder.Entity<Course>()
                    .Property(x => x.CreatedById)
                    .HasDefaultValue(0);

        modelBuilder.Entity<Course>()
                    .Property(x => x.EditedById)
                    .HasDefaultValue(0);

        modelBuilder.Entity<Course>()
                    .Property(x => x.DeletedById)
                    .HasDefaultValue(0);
        base.OnModelCreating(modelBuilder);
    }
}