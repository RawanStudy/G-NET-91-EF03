using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Fluent_API_Configurations;

public class InstructorConfiguration :
       IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(i => i.Email)
            .IsUnique();

        builder.Property(i => i.Bio)
            .HasMaxLength(500);

        builder.Property(i => i.JoinedAt)
            .IsRequired();

        // One Instructor has many Courses
        builder.HasMany(i => i.Courses)
            .WithOne(c => c.Instructor)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Data
        builder.HasData(
            new Instructor
            {
                Id = 1,
                FullName = "Ahmed Mostafa",
                Email = "ahmed@example.com",
                Bio = "Backend Developer",
                JoinedAt = new DateTime(2024, 1, 10)
            },

            new Instructor
            {
                Id = 2,
                FullName = "Mariam Hassan",
                Email = "mariam@example.com",
                Bio = "Database Instructor",
                JoinedAt = new DateTime(2024, 2, 15)
            },

            new Instructor
            {
                Id = 3,
                FullName = "Omar Khaled",
                Email = "omar@example.com",
                Bio = null,
                JoinedAt = new DateTime(2024, 3, 20)
            }
        );
    }
}