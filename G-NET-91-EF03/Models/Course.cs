using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Models;

public class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public CourseLevel Level { get; set; }

    public bool IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }

    // Foreign Keys
    public int InstructorId { get; set; }

    public int CategoryId { get; set; }

    // Navigation Properties
    public Instructor Instructor { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}