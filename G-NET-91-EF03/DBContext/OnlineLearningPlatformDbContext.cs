using G_NET_91_EF03.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;
namespace G_NET_91_EF03.DDContext;

public class OnlineLearningPlatformDbContext :DbContext
{
    public DbSet<Instructor> Instructors => Set<Instructor>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=OnlineLearningPlatformDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OnlineLearningPlatformDbContext).Assembly);
    }
}