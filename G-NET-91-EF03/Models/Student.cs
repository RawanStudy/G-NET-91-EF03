using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Models;

public class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    // Navigation Property
    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}