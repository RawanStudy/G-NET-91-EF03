using G_NET_91_EF03.DDContext;
using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET_91_EF03;

internal class Program
{
    #region Create Operations
    #region Add Instructor
    static async Task AddInstructor(OnlineLearningPlatformDbContext db)
    {
        string email = "new.instructor@example.com";

        if (await db.Instructors.AnyAsync(i => i.Email == email))
        {
            Console.WriteLine("Instructor already exists.");
            return;
        }

        var instructor = new Instructor
        {
            FullName = "New Instructor",
            Email = email,
            Bio = "Backend Instructor",
            JoinedAt = DateTime.Now
        };

        try
        {
            db.Instructors.Add(instructor);

            await db.SaveChangesAsync();

            Console.WriteLine(
                $"Instructor Added. ID = {instructor.Id}");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Could not add instructor.");
        }
    }
    #endregion
    #region Add Course
    static async Task AddCourse(OnlineLearningPlatformDbContext db)
    {
        int instructorId = 1;
        int categoryId = 4;

        bool instructorExists = await db.Instructors
            .AnyAsync(i => i.Id == instructorId);

        bool categoryExists = await db.Categories
            .AnyAsync(c => c.Id == categoryId);

        if (!instructorExists)
        {
            Console.WriteLine("Instructor does not exist.");
            return;
        }

        if (!categoryExists)
        {
            Console.WriteLine("Category does not exist.");
            return;
        }

        if (await db.Courses.AnyAsync(c => c.Title == "API Testing"))
        {
            Console.WriteLine("Course already exists.");
            return;
        }

        var course = new Course
        {
            Title = "API Testing",
            Description = "Testing Web APIs",
            Price = 700,
            Level = CourseLevel.Intermediate,
            IsPublished = false,
            PublishedAt = null,
            InstructorId = instructorId,
            CategoryId = categoryId
        };

        try
        {
            db.Courses.Add(course);

            await db.SaveChangesAsync();

            Console.WriteLine(
                $"Course Added. ID = {course.Id}");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Could not add course.");
        }
    }
    #endregion
    #region Add Several Students
    static async Task AddSeveralStudents(OnlineLearningPlatformDbContext db)
    {
        var students = new List<Student>
    {
        new Student
        {
            FullName = "Student Six",
            Email = "student6@example.com",
            DateOfBirth = new DateTime(2003, 1, 1)
        },

        new Student
        {
            FullName = "Student Seven",
            Email = "student7@example.com",
            DateOfBirth = new DateTime(2003, 2, 2)
        },

        new Student
        {
            FullName = "Student Eight",
            Email = "student8@example.com",
            DateOfBirth = new DateTime(2003, 3, 3)
        }
    };

        // Avoid inserting the same students twice
        var existingEmails = await db.Students
            .Select(s => s.Email)
            .ToListAsync();

        students = students
            .Where(s => !existingEmails.Contains(s.Email))
            .ToList();

        if (students.Count == 0)
        {
            Console.WriteLine("These students already exist.");
            return;
        }

        try
        {
            // One AddRange
            await db.Students.AddRangeAsync(students);

            // One SaveChanges
            await db.SaveChangesAsync();

            foreach (var student in students)
            {
                Console.WriteLine(
                    $"Student Added. ID = {student.Id}");
            }
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Could not add students.");
        }
    }
    #endregion
    #region Enroll Student
    static async Task EnrollStudent(OnlineLearningPlatformDbContext db)
    {
        string studentEmail = "student6@example.com";
        int courseId = 1;

        var student = await db.Students
            .FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        var course = await db.Courses
            .FindAsync(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        if (!course.IsPublished)
        {
            Console.WriteLine(
                "Enrollment refused: Course is not published.");
            return;
        }

        bool alreadyEnrolled = await db.Enrollments
            .AnyAsync(e =>
                e.StudentId == student.Id &&
                e.CourseId == courseId);

        if (alreadyEnrolled)
        {
            Console.WriteLine(
                "Student is already enrolled in this course.");
            return;
        }

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = courseId,
            EnrolledAt = DateTime.Now,
            Progress = 0,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        };

        try
        {
            db.Enrollments.Add(enrollment);

            await db.SaveChangesAsync();

            Console.WriteLine("Student Enrolled Successfully.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Could not complete enrollment.");
        }
    }
    #endregion

    #endregion

    #region Read Operations
    #region Find Course
    static void FindCourse(
   OnlineLearningPlatformDbContext db,
    int id,
    string title)
    {
        // Find by ID
        var courseById = db.Courses.Find(id);

        if (courseById == null)
        {
            Console.WriteLine("Course not found by ID.");
        }
        else
        {
            Console.WriteLine(
                $"Found by ID: {courseById.Title}");
        }

        // Find by Title
        var courseByTitle = db.Courses
            .FirstOrDefault(c => c.Title == title);

        if (courseByTitle == null)
        {
            Console.WriteLine("Course not found by Title.");
        }
        else
        {
            Console.WriteLine(
                $"Found by Title: {courseByTitle.Title}");
        }
    }
    #endregion
    #region FilterCourses 
    static void FilterCourses(
   OnlineLearningPlatformDbContext db,
    decimal minPrice,
    decimal maxPrice)
    {
        var courses = db.Courses
            .Where(c =>
                c.Price >= minPrice &&
                c.Price <= maxPrice)
            .OrderByDescending(c => c.Price)
            .Select(c => new
            {
                c.Title,
                c.Level,
                c.Price
            })
            .ToList();

        foreach (var course in courses)
        {
            Console.WriteLine(
                $"Title: {course.Title} | " +
                $"Level: {course.Level} | " +
                $"Price: {course.Price}");
        }
    }
    #endregion
    #region Sub-Categories
    static void PrintSubCategories(
   OnlineLearningPlatformDbContext db,
    int parentId)
    {
        var subCategories = db.Categories
            .Where(c => c.ParentCategoryId == parentId)
            .ToList();

        Console.WriteLine("Direct Sub-Categories:");

        foreach (var category in subCategories)
        {
            Console.WriteLine(category.Name);
        }

        var topCategories = db.Categories
            .Where(c => c.ParentCategoryId == null)
            .ToList();

        Console.WriteLine("Top-Level Categories:");

        foreach (var category in topCategories)
        {
            Console.WriteLine(category.Name);
        }
    }
    #endregion
    #region Course Roster
    static void PrintCourseRoster(
   OnlineLearningPlatformDbContext db,
    int courseId)
    {
        // First get Student IDs from Enrollments
        var studentIds = db.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.StudentId)
            .ToList();

        // Then get Students using Contains
        var students = db.Students
            .Where(s => studentIds.Contains(s.Id))
            .ToList();

        Console.WriteLine(
            $"Number of Students: {students.Count}");

        foreach (var student in students)
        {
            Console.WriteLine(student.FullName);
        }
    }
    #endregion

    #endregion

    #region Update Operations

    #region Change Price
    static async Task ChangePrice(
   OnlineLearningPlatformDbContext db,
    int courseId,
    decimal newPrice)
    {
        if (newPrice < 0)
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        var course = await db.Courses.FindAsync(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        course.Price = newPrice;

        await db.SaveChangesAsync();

        Console.WriteLine("Price Updated Successfully.");
    }
    #endregion
    #region Update Progress
    static async Task UpdateProgress(
   OnlineLearningPlatformDbContext db,
    int studentId,
    int courseId,
    decimal newProgress)
    {
        if (newProgress < 0 || newProgress > 100)
        {
            Console.WriteLine(
                "Progress must be between 0 and 100.");
            return;
        }

        // Find using Composite Key
        var enrollment = await db.Enrollments
            .FindAsync(studentId, courseId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        enrollment.Progress = newProgress;

        if (newProgress == 100)
        {
            enrollment.Status = EnrollmentStatus.Completed;
        }

        await db.SaveChangesAsync();

        Console.WriteLine("Progress Updated Successfully.");
    }
    #endregion
    #region Publish Course
    static async Task PublishCourse(
   OnlineLearningPlatformDbContext db,
    int courseId)
    {
        var course = await db.Courses.FindAsync(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        course.IsPublished = true;

        course.PublishedAt ??= DateTime.Now;

        await db.SaveChangesAsync();

        Console.WriteLine("Course Published Successfully.");
    }
    #endregion

    #endregion

    #region Delete Operations
    #region Unenroll
    static async Task Unenroll(
   OnlineLearningPlatformDbContext db,
    int studentId,
    int courseId)
    {
        var enrollment = await db.Enrollments
            .FindAsync(studentId, courseId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        db.Enrollments.Remove(enrollment);

        await db.SaveChangesAsync();

        Console.WriteLine("Student Unenrolled Successfully.");
    }
    #endregion
    #region Delete Category – Try First
    static async Task TryDeleteCategoryWithChildren(
   OnlineLearningPlatformDbContext db,
    int categoryId)
    {
        var category = await db.Categories
            .FindAsync(categoryId);

        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        db.Categories.Remove(category);

        try
        {
            await db.SaveChangesAsync();

            Console.WriteLine("Category Deleted.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                "Cannot delete category: " +
                ex.GetBaseException().Message);

            // Restore the entity state after failed deletion
            db.Entry(category).State = EntityState.Unchanged;
        }
    }
    #endregion
    #region Safe Delete Category
    static async Task SafeDeleteCategory(
   OnlineLearningPlatformDbContext db,
    int categoryId)
    {
        var category = await db.Categories
            .FindAsync(categoryId);

        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        bool hasSubCategories = await db.Categories
            .AnyAsync(c => c.ParentCategoryId == categoryId);

        if (hasSubCategories)
        {
            Console.WriteLine(
                "Cannot delete: Category has sub-categories.");
            return;
        }

        bool hasCourses = await db.Courses
            .AnyAsync(c => c.CategoryId == categoryId);

        if (hasCourses)
        {
            Console.WriteLine(
                "Cannot delete: Courses belong to this category.");
            return;
        }

        db.Categories.Remove(category);

        await db.SaveChangesAsync();

        Console.WriteLine("Category Deleted Successfully.");
    }
    #endregion
    #region Delete Course With Enrollments
    static async Task DeleteCourseWithEnrollments(
   OnlineLearningPlatformDbContext db,
    int courseId)
    {
        var course = await db.Courses
            .FindAsync(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        int before = await db.Enrollments
            .CountAsync(e => e.CourseId == courseId);

        db.Courses.Remove(course);

        await db.SaveChangesAsync();

        int after = await db.Enrollments
            .CountAsync(e => e.CourseId == courseId);

        Console.WriteLine(
            $"Enrollments Before Delete: {before}");

        Console.WriteLine(
            $"Enrollments After Delete: {after}");

        // Enrollments are deleted because the Course relationship
        // uses DeleteBehavior.Cascade.
    }
    #endregion

    #endregion

    #region Bulk Operations
    #region Bulk Price Raise
    static async Task BulkRaisePrices(
  OnlineLearningPlatformDbContext db,
    CourseLevel level)
    {
        int affectedRows = await db.Courses
            .Where(c =>
                c.IsPublished &&
                c.Level == level)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    c => c.Price,
                    c => c.Price * 1.10m
                )
            );

        Console.WriteLine(
            $"Affected Rows: {affectedRows}");
    }
    #endregion
    #region Bulk Delete
    static async Task BulkDeleteDroppedEnrollments(
    OnlineLearningPlatformDbContext db)
    {
        int affectedRows = await db.Enrollments
            .Where(e => e.Status == EnrollmentStatus.Dropped)
            .ExecuteDeleteAsync();

        Console.WriteLine(
            $"Deleted Enrollments: {affectedRows}");
    }
    #endregion
    #endregion
    static async Task Main(string[] args)
    {
        using var db = new OnlineLearningPlatformDbContext();

        await db.Database.MigrateAsync();

        await SeedData(db);

        // CREATE
        await AddInstructor(db);
        await AddCourse(db);
        await AddSeveralStudents(db);
        await EnrollStudent(db);

        // READ
        FindCourse(db, 1, "C# Fundamentals");
        FilterCourses(db, 400, 1000);
        PrintSubCategories(db, 1);
        PrintCourseRoster(db, 1);

        // UPDATE
        await ChangePrice(db, 1, 550);
        await UpdateProgress(db, 1, 1, 100);
        await PublishCourse(db, 3);

        // DELETE
        await Unenroll(db, 4, 2);
        await TryDeleteCategoryWithChildren(db, 1);
        await SafeDeleteCategory(db, 3);
        await DeleteCourseWithEnrollments(db, 4);

        // BULK OPERATIONS
        await BulkRaisePrices(db, CourseLevel.Intermediate);
        await BulkDeleteDroppedEnrollments(db);

        Console.WriteLine("\nAll Operations Finished.");
    }

    // =====================================
    // PART 02: RUNTIME SEEDING
    // =====================================

    static async Task SeedData(OnlineLearningPlatformDbContext db)
    {
        // Seed Students only when the table is empty
        if (!await db.Students.AnyAsync())
        {
            var students = new List<Student>
                {
                    new Student
                    {
                        FullName = "Omar Ali",
                        Email = "omar.student@example.com",
                        DateOfBirth = new DateTime(2003, 5, 12)
                    },

                    new Student
                    {
                        FullName = "Mona Samir",
                        Email = "mona.student@example.com",
                        DateOfBirth = new DateTime(2004, 2, 20)
                    },

                    new Student
                    {
                        FullName = "Youssef Adel",
                        Email = "youssef.student@example.com",
                        DateOfBirth = new DateTime(2002, 9, 3)
                    },

                    new Student
                    {
                        FullName = "Salma Hassan",
                        Email = "salma.student@example.com",
                        DateOfBirth = new DateTime(2003, 11, 7)
                    },

                    new Student
                    {
                        FullName = "Nour Ahmed",
                        Email = "nour.student@example.com",
                        DateOfBirth = new DateTime(2004, 7, 18)
                    }
                };

            await db.Students.AddRangeAsync(students);

            await db.SaveChangesAsync();

            Console.WriteLine("5 Students Seeded.");
        }

        // Seed Enrollments only when the table is empty
        if (!await db.Enrollments.AnyAsync())
        {
            var students = await db.Students
                .OrderBy(s => s.Id)
                .Take(5)
                .ToListAsync();

            if (students.Count < 5)
            {
                Console.WriteLine(
                    "Cannot seed enrollments: 5 students are required.");
                return;
            }

            // Make sure the required courses exist
            var coursesExist = await db.Courses
                .AnyAsync(c => c.Id == 6);

            if (!coursesExist)
            {
                Console.WriteLine(
                    "Cannot seed enrollments: courses not found.");
                return;
            }

            var enrollments = new List<Enrollment>
                {
                    new Enrollment
                    {
                        StudentId = students[0].Id,
                        CourseId = 1,
                        EnrolledAt = DateTime.Today.AddDays(-20),
                        Progress = 40,
                        Status = EnrollmentStatus.Active
                    },

                    new Enrollment
                    {
                        StudentId = students[1].Id,
                        CourseId = 1,
                        EnrolledAt = DateTime.Today.AddDays(-40),
                        Progress = 100,
                        Status = EnrollmentStatus.Completed,
                        FinalGrade = 95
                    },

                    new Enrollment
                    {
                        StudentId = students[2].Id,
                        CourseId = 1,
                        EnrolledAt = DateTime.Today.AddDays(-10),
                        Progress = 15,
                        Status = EnrollmentStatus.Dropped
                    },

                    new Enrollment
                    {
                        StudentId = students[0].Id,
                        CourseId = 2,
                        EnrolledAt = DateTime.Today.AddDays(-12),
                        Progress = 60,
                        Status = EnrollmentStatus.Active
                    },

                    new Enrollment
                    {
                        StudentId = students[3].Id,
                        CourseId = 2,
                        EnrolledAt = DateTime.Today.AddDays(-50),
                        Progress = 100,
                        Status = EnrollmentStatus.Completed,
                        FinalGrade = 88
                    },

                    new Enrollment
                    {
                        StudentId = students[4].Id,
                        CourseId = 4,
                        EnrolledAt = DateTime.Today.AddDays(-8),
                        Progress = 20,
                        Status = EnrollmentStatus.Dropped
                    },

                    new Enrollment
                    {
                        StudentId = students[1].Id,
                        CourseId = 4,
                        EnrolledAt = DateTime.Today.AddDays(-5),
                        Progress = 30,
                        Status = EnrollmentStatus.Active
                    },

                    new Enrollment
                    {
                        StudentId = students[2].Id,
                        CourseId = 6,
                        EnrolledAt = DateTime.Today.AddDays(-3),
                        Progress = 10,
                        Status = EnrollmentStatus.Active
                    }
                };

            await db.Enrollments.AddRangeAsync(enrollments);

            await db.SaveChangesAsync();

            Console.WriteLine("8 Enrollments Seeded.");
        }
    }
}