// =====================================================================
// StudentPortalConsole — SESSION PROJECT (Style Guide Rule 20/35/39/40)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 13 — LINQ Part 2 + EF Core (Code First)
//
// THIS PROJECT IS DAY-READY (Rule 39). The model classes and all seed
// data are already present as REAL, WORKING, RUNNABLE code, and the EF
// Core packages are already referenced. Open it, press run, and it
// builds and prints the Session 12 warm-up chain immediately — before a
// single TODO is done.
//
// Only TODAY'S NEW content is left as TODOs. Each TODO sits exactly
// where its code will be written (Rule 40), and the numbers run
// strictly top-to-bottom through this file in the same order the
// lecture teaches them:
//
//   TODO 1      Block 3 — your own LINQ extension method (top-level)
//   TODO 2-3    Blocks 4-5 — the DbContext
//   TODO 4-6    Block 1 (aggregates), inside Main
//   TODO 7-11   Block 2 (GroupBy, Join), inside Main
//   TODO 12-16  Block 3 (deferred execution), inside Main
//   TODO 17-19  Blocks 4-5 (EF Core queries), inside Main
//
// ⚠️ WHY TODO 1 IS AT THE TOP RATHER THAN WITH BLOCK 3'S OTHER TODOs:
//   C# requires extension methods in a top-level static class (CS1109),
//   so it cannot sit inside Program next to the code that uses it. It is
//   numbered 1 because it is physically first (Rule 40); teach it when
//   you reach Block 3, and TODO 16 is where you actually call it.
//
// ⚠️ BEFORE THE EF TODOs WILL WORK:
//   1. Run Application/Session13_PreInit.sql in SSMS.
//   2. Check TODO 3's connection string matches YOUR server.
//   3. Package Manager Console:  Add-Migration InitialCreate
//                                Update-Database
//
// For the full, correct, runnable version (do NOT peek until you've
// tried it yourself, or you're checking your own work), see:
// ../StudentPortalConsole_Complete/Program.cs
// =====================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography;
namespace StudentPortalConsole
{
    public static class StudentExtensions
    {
        public static IEnumerable<Student> MyTopStudents(
            this IEnumerable<Student> students,
            double threshold)
        {
            return students.Where(s => s.Gpa > threshold);
        }
    }
    // ===== Block 3 — your own LINQ operator =====

    // TODO 1: Define a class to hold today's custom LINQ operator. It
    //         must be marked both public and static, must NOT be
    //         generic, and must stay HERE at namespace level — nesting
    //         it inside Program produces a specific compiler error the
    //         lecture demonstrates. Inside it, define a public static
    //         method named HonorRoll whose single parameter is preceded
    //         by the keyword `this` — that keyword is the entire thing
    //         turning an ordinary static method into an extension
    //         method. The parameter's type should be the general
    //         "something you can walk through" collection interface with
    //         Student as its element type, and the method returns that
    //         same type. Its body returns the source collection filtered
    //         to students whose GPA is at or above 3.5, using the same
    //         filtering operator you already know.

    // ===== Blocks 4-5 — the DbContext =====

    // TODO 2: Define a class named StudentPortalContext that DERIVES
    //         from EF Core's base context class. Give it three public
    //         properties, each of the generic "one table" type, one per
    //         entity above — for Students, Courses, and Instructors.
    //         Remember the property NAME becomes the table name, so use
    //         plurals.

    // TODO 3: Inside TODO 2's class, override the configuration method
    //         EF calls when it needs to know how to connect. Inside it,
    //         call the SQL Server provider method on the options builder
    //         and pass a connection string with four parts: the server
    //         (a single dot means "this machine" — CHANGE THIS if your
    //         SQL Server is a named instance), the database name
    //         ITI_StudentPortalDB_EF, an instruction to log in as the
    //         current Windows user rather than a username and password,
    //         and an instruction to trust the server's certificate
    //         (needed because modern SQL Server encrypts by default and
    //         a local development certificate isn't one your machine
    //         trusts). Hardcoding this here is a real anti-pattern —
    //         Session 15 moves it to a configuration file — but today it
    //         is written out in full so every part is visible.

    internal class Program
    {

        static async Task Main(string[] args)
        {
            // ================================ SESSION 14 =======================================
            using (var context = new StudentPortalContext())
            {
                //    var hamdy = await context.Instructors
                //        .FirstAsync(i => i.FullName == "Hamdy");

                //    context.Courses.Add(new Course
                //    {
                //        CourseName = "ASP.NET Core",
                //        Credits = 3,
                //        InstructorId = hamdy.Id
                //    });

                //    context.Courses.Add(new Course
                //    {
                //        CourseName = "Entity Framework",
                //        Credits = 3,
                //        InstructorId = hamdy.Id
                //    });

                //    context.Courses.Add(new Course
                //    {
                //        CourseName = "LINQ Advanced",
                //        Credits = 2,
                //        InstructorId = hamdy.Id
                //    });

                //    await context.SaveChangesAsync();
                //}
                // Extra courses created = 3

                //var instructors = await context.Instructors.ToListAsync();

                //foreach (var i in instructors)
                //{
                //    Console.WriteLine(
                //        $"{i.FullName} : {i.Courses.Count}");
                //}
                // printed 0
                //var instructors = await context.Instructors
                //        .Include(i => i.Courses)
                //        .ToListAsync();

                //foreach (var i in instructors)
                //{
                //    Console.WriteLine(i.FullName);

                //    foreach (var c in i.Courses)
                //    {
                //        Console.WriteLine(c.CourseName);
                //    }
                //}

                // explict laoding
                //var hamdy =await context.Instructors
                //        .FirstAsync(i => i.FullName == "Hamdy");

                //Console.WriteLine(hamdy.Courses.Count);

                //await context.Entry(hamdy).Collection(i => i.Courses).LoadAsync();

                //Console.WriteLine(hamdy.Courses.Count);
                var students =await context.Students.AsNoTracking().ToListAsync();

                students[0].Gpa = 4;

                await context.SaveChangesAsync();


            }


        }



        //Did any migration fail today? If yes: what failed,
        //exactly which commands you used to roll back,
        //and what you changed before re-applying.
        //If no: write "no rollback needed" — but also state the two commands you would have used.

        // i used Update-Database -20260803180724_StC as that is the last migration that was successful
        // and then i changed the connection string to the correct one and then i used Update-Database to apply the migration again


    }
}





//    //// Session 12's chain, unchanged — proof the project runs
//    //// as-is, and the baseline today's EF half is compared against.
//    //Console.WriteLine("===== WARM-UP: Session 12's chain =====");
//    //var warmUp = students
//    //    .Where(s => s.Gpa > 3.0)
//    //    .OrderByDescending(s => s.Gpa)
//    //    .Select(s => s.FullName)
//    //    .ToList();
//    //foreach (string n in warmUp) Console.WriteLine($"  {n}");
//    //Console.WriteLine();
//List<Student> students = new List<Student>
//{
//    new Student { FullName = "Yara Adel",    YearOfStudy = 2, Gpa = 3.5 },
//    new Student { FullName = "Omar Hesham",  YearOfStudy = 3, Gpa = 2.8 },
//    new Student { FullName = "Nada Samir",   YearOfStudy = 1, Gpa = 3.9 },
//    new Student { FullName = "Kareem Fouad", YearOfStudy = 4, Gpa = 3.2 },

//};

//List<Instructor> instructors = new List<Instructor>
//{
//    new Instructor { FullName = "Hamdy",       YearsOfExperience = 10,
//                     AssignedCourseName = "Web Development Using .NET" },
//    new Instructor { FullName = "Mona Khalil", YearsOfExperience = 6,
//                     AssignedCourseName = "Database Fundamentals" },
//    new Instructor { FullName = "youssef ezzat", YearsOfExperience = ex_val,
//                     AssignedCourseName = "Machine Learning" }
//};

//List<Course> courses = new List<Course>
//{
//    new Course { CourseName = "Web Development Using .NET", Credits = 4 },
//    new Course { CourseName = "Database Fundamentals",      Credits = 3 }
//};

//    Console.WriteLine($"counts : {students.Count()}");
//    var count = students.Where(s => s.Gpa > threshold).ToList();
//    Console.WriteLine($"Students above threshold : {count.Count()}");
//    Console.WriteLine($"Average GPA : {students.Average(s => s.Gpa):F2}");
//    Console.WriteLine($"Highest GPA : {students.Max(s => s.Gpa):F2}");
//    Console.WriteLine($"Lowest GPA : {students.Min(s => s.Gpa):F2}");
//    Console.WriteLine($"Students with GPA < 2 : {students.Count(s => s.Gpa < 2)}");
//    Console.WriteLine($"All students have GPA >= 2 : {students.All(s => s.Gpa >= 2)}");

//    List<Student> emptyList = new List<Student>();
//    Console.WriteLine($"Empty list count : {emptyList.Count()}");
//    Console.WriteLine($"Empty list has any students : {emptyList.Any()}");
//    if (emptyList.Any())
//    {
//        Console.WriteLine(emptyList.Average(s => s.Gpa));
//    }
//    else
//    {
//        Console.WriteLine("No students.");
//    }

//    var groupedByYear = students.GroupBy(s => s.YearOfStudy);
//    foreach (var group in groupedByYear)
//    {
//        Console.WriteLine($"Students in year {group.Key} with count {group.Count()}:");
//        foreach (var student in group)
//        {
//            Console.WriteLine($" - {student.FullName}");
//        }
//        // not sorted bec the GroupBy does not guarantee order of groups
//    }

//    var myygroup = students.GroupBy(s => s.Gpa >= threshold);
//    foreach (var group in myygroup)
//    {
//        string groupName = group.Key ? $"students above threshold: {threshold:F2}" : $"students below threshold: {threshold:F2}";
//        Console.WriteLine($"Students in {groupName} with count {group.Count()}:");
//        foreach (var student in group)
//        {
//            Console.WriteLine($" - {student.FullName}");
//        }
//    }

//    var groupedByYear2 = students.GroupBy(s => s.YearOfStudy).OrderByDescending(g => g.Key);
//    foreach (var group in groupedByYear2)
//    {
//        Console.WriteLine($"Students in year {group.Key} with count {group.Count()}:");
//        foreach (var student in group)
//        {
//            Console.WriteLine($" - {student.FullName}");
//        }
//    }



//    var joinmethod= instructors.Join(
//        courses,
//        i => i.AssignedCourseName,
//        c => c.CourseName,
//        (i, c) => $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)"
//        );


//    Console.WriteLine($"instructors went in {instructors.Count()}, Instructors went out {joinmethod.Count()}");


//    var defferredexecution = students.Where(s => s.Gpa > 3.0);
//    students.Add(new Student { FullName = "Layla Mostafa", YearOfStudy = 2, Gpa = 3.7 });
//    Console.WriteLine(defferredexecution.Count());
//    //wont execute until we enumerate it

//    //========================================
//    //Multiple enumeration anti pattern 
//    var q = students.Where(s => s.Gpa > threshold);

//    Console.WriteLine($"Count = {q.Count()}");

//    foreach (var student in q)
//    {
//        Console.WriteLine(student.FullName);
//    }

//    Console.WriteLine($"Average = {q.Average(s => s.Gpa):F2}");
//    //========================================
//    //Reproduce the multiple - enumeration

//    var f = students
//            .Where(s => s.Gpa > threshold)
//            .ToList();

//    Console.WriteLine($"Count = {f.Count}");

//    foreach (var student in f)
//    {
//        Console.WriteLine(student.FullName);
//    }

//    Console.WriteLine($"Average = {f.Average(s => s.Gpa):F2}");

//    var topStudents = students
//                    .MyTopStudents(threshold)
//                    .OrderBy(s => s.FullName)
//                    .Select(s => s.FullName)
//                    .ToList();

//    foreach (var name in topStudents)
//    {
//        Console.WriteLine(name);
//    }

//    //var queryjoin = from i in instructors
//    //                join c in courses
//    //                on i.AssignedCourseName equals c.CourseName
//    //                select $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)";

//    //foreach (var line in queryjoin)
//    //{
//    //    Console.WriteLine(line);
//    //}

//    // shorthand syntax  ? : 

//    //Console.WriteLine($"Grouped By GPA band");
//    //var byBand = students.GroupBy(s => s.Gpa >= 3.5 ? "Honors" : "Standard")
//    //    .OrderBy(g => g.Key);

//    //foreach (var group in byBand)
//    //{
//    //    Console.WriteLine($"{group.Key} : {group.Count()} students");
//    //    foreach (var st in group)
//    //    {
//    //        Console.WriteLine($"     {st.FullName} : {st.Gpa:F2}");
//    //    }
//    //}





//    //var teaching = instructors.Join(
//    //    courses,
//    //    i => i.AssignedCourseName,
//    //    c => c.CourseName,
//    //    (i, c) => $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)"
//    //    );
//    //foreach (var line in teaching)
//    //{
//    //    Console.WriteLine(line);
//    //}

//    //var teachingQuery = from i in instructors
//    //                    join c in courses on i.AssignedCourseName equals c.CourseName
//    //                    select $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)";

//    // GroupJoin , DefaultIfEmpty



//    // ===== Block 3 — deferred execution, in Main =====



//    //var highStudents = students.Where(s => s.Gpa > 3.0); // Deffered execution
//    //var highStudents = students.Where(s => s.Gpa > 3.0).ToList(); // Immediate execution
//    //students.Add(new Student { FullName = "Layla Mostafa", YearOfStudy = 2, Gpa = 3.7 });
//    //foreach (var student in highStudents)
//    //{
//    //    Console.WriteLine(student.FullName);
//    //}

//    //var highStudents = students.Where(s => s.Gpa > 3.0).ToList();
//    //Console.WriteLine($"There are {highStudents.Count} high GPA students");
//    //foreach (var st in highStudents)
//    //{
//    //    Console.WriteLine(st.FullName);
//    //}

//    //var avg = highStudents.Average(s => s.Gpa);
//    //Console.WriteLine($"The average is {avg:F2}");

//    //var name = "I   T   I";
//    //Console.WriteLine(name.RemoveWhiteSpaces());



//    // ===== Blocks 4-5 — EF Core, in Main =====



//    using (var context = new StudentPortalContext())
//    {
//        //context.Database.EnsureCreated();
//        var allStudents = context.Students.ToList();
//        foreach (var st in allStudents)
//        {
//            Console.WriteLine($"  {st.FullName} - Year {st.YearOfStudy} - GPA {st.Gpa}");
//        }
//    context.Students.Where(s => s.Gpa > 3.0).ToList();
//    context.Students.ToList().Where(s => s.Gpa > 3.0);

//    //if (!context.Students.Any())
//    //{
//    //    context.AddRange(students);
//    //    context.AddRange(instructors);
//    //    context.AddRange(courses);
//    //    context.SaveChanges();
//    //}

//    var topNames = context.Students
//        .Where(s => s.Gpa > 3.0)
//        .OrderByDescending(s => s.Gpa)
//        .Select(s => s.FullName)
//        .ToList();
//    foreach (var name in topNames)
//    {
//        Console.WriteLine(name);
//    }