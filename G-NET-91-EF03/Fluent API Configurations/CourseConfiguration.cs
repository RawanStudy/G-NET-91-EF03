using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Fluent_API_Configurations;

public class CourseConfiguration :
        IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Price)
            .HasPrecision(10, 2);

        builder.Property(c => c.Level)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.IsPublished)
            .IsRequired();

        builder.Property(c => c.PublishedAt);

        // Seed Data
        builder.HasData(
            new Course
            {
                Id = 1,
                Title = "C# Fundamentals",
                Description = "Learn C# Basics",
                Price = 500,
                Level = CourseLevel.Beginner,
                IsPublished = true,
                PublishedAt = new DateTime(2025, 1, 10),
                InstructorId = 1,
                CategoryId = 2
            },

            new Course
            {
                Id = 2,
                Title = "ASP.NET Core Web API",
                Description = "Build REST APIs",
                Price = 900,
                Level = CourseLevel.Intermediate,
                IsPublished = true,
                PublishedAt = new DateTime(2025, 2, 15),
                InstructorId = 1,
                CategoryId = 4
            },

            new Course
            {
                Id = 3,
                Title = "Advanced EF Core",
                Description = "Learn EF Core",
                Price = 1100,
                Level = CourseLevel.Advanced,
                IsPublished = false,
                PublishedAt = null,
                InstructorId = 2,
                CategoryId = 4
            },

            new Course
            {
                Id = 4,
                Title = "SQL Server Basics",
                Description = "Learn SQL Server",
                Price = 600,
                Level = CourseLevel.Beginner,
                IsPublished = true,
                PublishedAt = new DateTime(2025, 3, 1),
                InstructorId = 2,
                CategoryId = 5
            },

            new Course
            {
                Id = 5,
                Title = "HTML and CSS",
                Description = "Frontend Basics",
                Price = 450,
                Level = CourseLevel.Beginner,
                IsPublished = false,
                PublishedAt = null,
                InstructorId = 3,
                CategoryId = 3
            },

            new Course
            {
                Id = 6,
                Title = "Clean Architecture",
                Description = "Software Architecture",
                Price = 1300,
                Level = CourseLevel.Advanced,
                IsPublished = true,
                PublishedAt = new DateTime(2025, 4, 5),
                InstructorId = 3,
                CategoryId = 2
            }
        );
    }
}