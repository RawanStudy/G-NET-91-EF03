using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Fluent_API_Configurations;

public class EnrollmentConfiguration :
       IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        // Composite Primary Key
        builder.HasKey(e => new
        {
            e.StudentId,
            e.CourseId
        });

        builder.Property(e => e.EnrolledAt)
            .IsRequired();

        builder.Property(e => e.Progress)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.FinalGrade)
            .HasPrecision(5, 2);

        // Check Constraints
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Enrollment_Progress",
                "[Progress] >= 0 AND [Progress] <= 100");

            t.HasCheckConstraint(
                "CK_Enrollment_FinalGrade",
                "[FinalGrade] IS NULL OR [Status] = 'Completed'");
        });

        // Course Relationship
        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}