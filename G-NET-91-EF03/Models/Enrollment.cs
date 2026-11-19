using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Models;

public class Enrollment
{

    // Composite Primary Key
    public int StudentId { get; set; }

public int CourseId { get; set; }

public DateTime EnrolledAt { get; set; }

public decimal Progress { get; set; }

public EnrollmentStatus Status { get; set; }

public decimal? FinalGrade { get; set; }

// Navigation Properties
public Student Student { get; set; } = null!;

public Course Course { get; set; } = null!;
    }