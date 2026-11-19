using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Fluent_API_Configurations;

public class CategoryConfiguration :
       IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(c => c.Description)
            .HasMaxLength(300);

        // Self-Referencing Relationship
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // One Category has many Courses
        builder.HasMany(c => c.Courses)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Data
        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Programming",
                Description = "Programming Topics",
                ParentCategoryId = null
            },

            new Category
            {
                Id = 2,
                Name = "Backend",
                Description = "Backend Development",
                ParentCategoryId = 1
            },

            new Category
            {
                Id = 3,
                Name = "Frontend",
                Description = "Frontend Development",
                ParentCategoryId = 1
            },

            new Category
            {
                Id = 4,
                Name = "ASP.NET",
                Description = "ASP.NET Core",
                ParentCategoryId = 2
            },

            new Category
            {
                Id = 5,
                Name = "Databases",
                Description = "Database Technologies",
                ParentCategoryId = 2
            }
        );
    }
}