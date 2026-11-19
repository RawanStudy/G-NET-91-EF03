using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_91_EF03.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    // Foreign Key
    public int? ParentCategoryId { get; set; }

    // Navigation Property
    public Category? ParentCategory { get; set; }

    // Navigation Property
    public ICollection<Category> SubCategories { get; set; }
        = new List<Category>();

    // Navigation Property
    public ICollection<Course> Courses { get; set; }
        = new List<Course>();
}
