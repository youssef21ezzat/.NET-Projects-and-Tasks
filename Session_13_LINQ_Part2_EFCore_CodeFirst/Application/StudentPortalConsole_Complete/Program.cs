// =====================================================================
// StudentPortalConsole_Complete — FULL WORKING FALLBACK (Rule 20)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 13 — LINQ Part 2 + EF Core (Code First)
//
// Complete, correct, runnable version of everything taught live today.
// Matches Instructor_Guide_EN.md and Student_Guide.md exactly (Rule 15).
//
// TWO HALVES:
//   Blocks 1-3 — LINQ Part 2 over in-memory lists: aggregates, GroupBy,
//                Join, deferred execution, multiple enumeration, and a
//                custom extension method.
//   Blocks 4-5 — EF Core: DbContext, the first real query, and Code
//                First migrations.
//
// ONE SET OF CLASSES, DELIBERATELY:
//   Student / Course / Instructor below are used BOTH as the in-memory
//   collections in Blocks 1-3 AND as the EF entities in Blocks 4-5.
//   That is not a shortcut — it is the point. The same class you spend
//   the morning querying with LINQ in memory becomes, in the afternoon,
//   a table in SQL Server, queried with the identical LINQ. Nothing
//   about the class changes.
//
//   They are deliberately simpler than Session 11's Person/IPrintable
//   hierarchy: no base class, no interfaces, no validating setters. A
//   table row has no concept of an abstract base class. Block 4's
//   STOP.Ask has trainees compare these against Session 12's project,
//   which is exactly the comparison worth making.
//
// ⚠️ BEFORE RUNNING THE EF HALF:
//   1. Run Application/Session13_PreInit.sql in SSMS.
//   2. Check the connection string in StudentPortalContext.OnConfiguring
//      matches YOUR server (see the note there).
//   3. In Package Manager Console:  Add-Migration InitialCreate
//                                   Update-Database
//   Until step 3 is done the EF half fails with "Cannot open database" —
//   which is expected, and is demonstrated deliberately in Block 4.
// =====================================================================

using Microsoft.EntityFrameworkCore;

namespace StudentPortalConsole
{
    // =================================================================
    // THE MODEL — used by BOTH halves of today.
    // `Id` becomes an auto-incrementing PRIMARY KEY purely by EF's
    // naming convention. We never write PRIMARY KEY anywhere.
    // =================================================================
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public int YearOfStudy { get; set; }
        public double Gpa { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = "";
        public int Credits { get; set; }
    }

    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public int YearsOfExperience { get; set; }
        public string? AssignedCourseName { get; set; }
    }

    // =================================================================
    // Block 3 — custom LINQ operator.
    // MUST be a top-level, non-generic, public static class (CS1109).
    // =================================================================
    public static class StudentQueryExtensions
    {
        // `this` on the first parameter is what makes this an extension
        // method. Deferred, because its body just returns a Where(...).
        public static IEnumerable<Student> HonorRoll(this IEnumerable<Student> source)
        {
            return source.Where(s => s.Gpa >= 3.5);
        }
    }

    // =================================================================
    // Blocks 4-5 — the DbContext: ONE SESSION with the database.
    // Each DbSet<T> is one table; the PROPERTY NAME becomes the table
    // name, so `Students` (plural) produces a table called Students.
    // =================================================================
    public class StudentPortalContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ⚠️ CHECK THIS MATCHES YOUR SERVER BEFORE THE SESSION.
            //   Server=.                     the SQL Server on this machine.
            //                                If Sessions 3-4 used a named
            //                                instance, change it to match
            //                                (e.g. Server=.\SQLEXPRESS).
            //                                A wrong server name here is the
            //                                most likely cause of a failed
            //                                demo, and the error message
            //                                never mentions server names.
            //   Database=...                 which database. It does not
            //                                exist until Update-Database.
            //   Trusted_Connection=True      log in as the current Windows
            //                                user; no username/password.
            //   TrustServerCertificate=True  modern SQL Server encrypts by
            //                                default and a local dev cert
            //                                isn't trusted. Dev only —
            //                                never against production.
            //
            // Hardcoding a connection string here is a genuine
            // anti-pattern, done deliberately today so all four parts are
            // visible in one place while being learned. Session 15 moves
            // it to appsettings.json, where it belongs.
            optionsBuilder.UseSqlServer(
                "Server=.;Database=ITI_StudentPortalDB_EF;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // =========================================================
            // Seed data for the LINQ half — identical values to Session
            // 12, so every answer is directly comparable (Rule 15).
            // =========================================================
            List<Student> students = new List<Student>
            {
                new Student { FullName = "Yara Adel",    YearOfStudy = 2, Gpa = 3.5 },
                new Student { FullName = "Omar Hesham",  YearOfStudy = 3, Gpa = 2.8 },
                new Student { FullName = "Nada Samir",   YearOfStudy = 1, Gpa = 3.9 },
                new Student { FullName = "Kareem Fouad", YearOfStudy = 4, Gpa = 3.2 }
            };

            List<Instructor> instructors = new List<Instructor>
            {
                new Instructor { FullName = "Hamdy",       YearsOfExperience = 10,
                                 AssignedCourseName = "Web Development Using .NET" },
                new Instructor { FullName = "Mona Khalil", YearsOfExperience = 6,
                                 AssignedCourseName = "Database Fundamentals" }
            };

            List<Course> courses = new List<Course>
            {
                new Course { CourseName = "Web Development Using .NET", Credits = 4 },
                new Course { CourseName = "Database Fundamentals",      Credits = 3 }
            };

            // =========================================================
            // WARM-UP — Session 12's chain, unchanged, for comparison
            // =========================================================
            Console.WriteLine("===== WARM-UP: Session 12's chain =====");
            var warmUp = students
                .Where(s => s.Gpa > 3.0)
                .OrderByDescending(s => s.Gpa)
                .Select(s => s.FullName)
                .ToList();
            foreach (string n in warmUp) Console.WriteLine($"  {n}");
            // → Nada Samir, Yara Adel, Kareem Fouad

            // =========================================================
            // BLOCK 1 — Aggregates: collection in, ONE VALUE out
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== BLOCK 1: Aggregates =====");
            Console.WriteLine($"Total students: {students.Count()}");                 // 4
            Console.WriteLine($"Above 3.0: {students.Count(s => s.Gpa > 3.0)}");      // 3
            Console.WriteLine($"Average GPA: {students.Average(s => s.Gpa):F2}");     // 3.35
            Console.WriteLine($"Highest GPA: {students.Max(s => s.Gpa)}");            // 3.9
            Console.WriteLine($"Lowest GPA: {students.Min(s => s.Gpa)}");             // 2.8
            Console.WriteLine($"Anyone failing: {students.Any(s => s.Gpa < 2.0)}");   // False
            Console.WriteLine($"All passing: {students.All(s => s.Gpa >= 2.0)}");     // True

            // ⚠️ Average/Max/Min THROW on an empty collection
            // ("Sequence contains no elements"). Count/Any/All are safe.
            List<Student> noStudents = new List<Student>();
            Console.WriteLine($"Empty count: {noStudents.Count()}");   // 0, safe
            Console.WriteLine($"Empty any: {noStudents.Any()}");       // False, safe
            if (noStudents.Any())
            {
                Console.WriteLine($"Empty average: {noStudents.Average(s => s.Gpa)}");
            }
            else
            {
                Console.WriteLine("Empty average: skipped — guarded with Any() first");
            }

            // =========================================================
            // BLOCK 2 — GroupBy and Join
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== BLOCK 2: GroupBy and Join =====");

            // GroupBy on a stored property. Buckets come out in
            // FIRST-ENCOUNTERED key order (2, 3, 1, 4) — never sorted.
            Console.WriteLine("-- Grouped by year --");
            foreach (var group in students.GroupBy(s => s.YearOfStudy))
            {
                Console.WriteLine($"Year {group.Key}: {group.Count()} student(s)");
                foreach (Student s in group)
                {
                    Console.WriteLine($"   {s.FullName}");
                }
            }

            // GroupBy on a COMPUTED key — "Honors"/"Standard" exist
            // nowhere on the class; the lambda invents them per item.
            Console.WriteLine("-- Grouped by GPA band --");
            foreach (var group in students.GroupBy(s => s.Gpa >= 3.5 ? "Honors" : "Standard"))
            {
                Console.WriteLine($"{group.Key}: {group.Count()} student(s)");
                foreach (Student s in group)
                {
                    Console.WriteLine($"   {s.FullName} ({s.Gpa:F2})");
                }
            }

            // If order matters, ask for it explicitly — GroupBy never sorts.
            Console.WriteLine("-- Grouped by year, sorted by key --");
            foreach (var group in students.GroupBy(s => s.YearOfStudy).OrderBy(g => g.Key))
            {
                Console.WriteLine($"Year {group.Key}: {group.Count()} student(s)");
            }

            // Join — INNER JOIN semantics. An instructor whose
            // AssignedCourseName matches no course produces NO row at
            // all: no error, no blank line, no warning.
            Console.WriteLine("-- Who teaches what (method syntax) --");
            var teaching = instructors.Join(
                courses,                        // the SECOND collection
                i => i.AssignedCourseName,      // key from the FIRST
                c => c.CourseName,              // key from the SECOND
                (i, c) => $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)");

            foreach (string line in teaching) Console.WriteLine($"   {line}");

            // Same join, query syntax — note `equals`, never `==`
            Console.WriteLine("-- Who teaches what (query syntax) --");
            var teachingQuery = from i in instructors
                                join c in courses on i.AssignedCourseName equals c.CourseName
                                select $"{i.FullName} teaches {c.CourseName} ({c.Credits} credits)";
            foreach (string line in teachingQuery) Console.WriteLine($"   {line}");

            // Proof of silent exclusion: 3 instructors in, still 2 rows out.
            Instructor sara = new Instructor
            {
                FullName = "Sara Nabil",
                YearsOfExperience = 4,
                AssignedCourseName = "Machine Learning"   // no such course
            };
            instructors.Add(sara);
            var teachingWithSara = instructors.Join(
                courses,
                i => i.AssignedCourseName,
                c => c.CourseName,
                (i, c) => $"{i.FullName} teaches {c.CourseName}");
            Console.WriteLine($"-- {instructors.Count} instructors in, " +
                              $"{teachingWithSara.Count()} rows out --");   // 3 in, 2 out
            instructors.Remove(sara);

            // =========================================================
            // BLOCK 3 — Deferred execution + your own operator
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== BLOCK 3: Deferred Execution =====");

            // The query is DESCRIBED here, not run. Layla is added
            // afterwards but BEFORE anything consumes it — so she counts.
            var deferredQuery = students.Where(s => s.Gpa > 3.0);
            students.Add(new Student { FullName = "Layla Mostafa", YearOfStudy = 2, Gpa = 3.7 });
            Console.WriteLine($"Deferred count (includes Layla): {deferredQuery.Count()}");   // 4
            students.RemoveAt(students.Count - 1);   // back to four students

            // ⚠️ MULTIPLE ENUMERATION — runs the filter THREE times.
            // Invisible over 4 in-memory items; three network round-trips
            // once this same shape points at SQL Server.
            var highAchievers = students.Where(s => s.Gpa > 3.0);
            Console.WriteLine($"Count (run 1): {highAchievers.Count()}");
            foreach (Student s in highAchievers)                           // run 2
            {
                Console.WriteLine($"   {s.FullName}");
            }
            Console.WriteLine($"Average (run 3): {highAchievers.Average(s => s.Gpa):F2}");

            // ✅ THE FIX — one ToList() forces a single execution.
            var highAchieversList = students.Where(s => s.Gpa > 3.0).ToList();
            Console.WriteLine($"Fixed count: {highAchieversList.Count}");   // property, no ()
            Console.WriteLine($"Fixed average: {highAchieversList.Average(s => s.Gpa):F2}");

            // Custom extension method, chaining like a built-in
            Console.WriteLine("-- Honor roll (custom extension method) --");
            var honorNames = students
                .HonorRoll()                       // ours — deferred
                .OrderBy(s => s.FullName)          // built-in
                .Select(s => s.FullName)
                .ToList();
            foreach (string n in honorNames) Console.WriteLine($"   {n}");
            // → Nada Samir, Yara Adel

            // =========================================================
            // BLOCKS 4-5 — EF Core against the real database
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== BLOCKS 4-5: EF Core =====");
            Console.WriteLine("(Requires Add-Migration InitialCreate + Update-Database first)");

            try
            {
                using (var context = new StudentPortalContext())
                {
                    // Seed once. Add() only records INTENT; SaveChanges()
                    // issues the actual INSERTs, in one transaction.
                    if (!context.Students.Any())
                    {
                        context.Students.Add(new Student { FullName = "Yara Adel",    YearOfStudy = 2, Gpa = 3.5 });
                        context.Students.Add(new Student { FullName = "Omar Hesham",  YearOfStudy = 3, Gpa = 2.8 });
                        context.Students.Add(new Student { FullName = "Nada Samir",   YearOfStudy = 1, Gpa = 3.9 });
                        context.Students.Add(new Student { FullName = "Kareem Fouad", YearOfStudy = 4, Gpa = 3.2 });
                        context.SaveChanges();
                        Console.WriteLine("Seeded 4 students into the database.");
                    }

                    // The first real query. IDENTICAL LINQ to the Warm-Up,
                    // different data source — EF turns it into SQL.
                    var dbTopNames = context.Students
                        .Where(s => s.Gpa > 3.0)              // ✅ filters on the SERVER
                        .OrderByDescending(s => s.Gpa)
                        .Select(s => s.FullName)
                        .ToList();

                    Console.WriteLine("-- From the DATABASE (same chain as the Warm-Up) --");
                    foreach (string n in dbTopNames) Console.WriteLine($"   {n}");
                    // → Nada Samir, Yara Adel, Kareem Fouad — identical output

                    // ⚠️ NEVER do this against a real table:
                    //    context.Students.ToList().Where(s => s.Gpa > 3.0)
                    // Same answer, but ToList() first drags the ENTIRE table
                    // across the network and filters in C# afterwards.

                    // Aggregates run on the SERVER too — this becomes
                    // SELECT AVG(...), not "fetch all rows then average".
                    Console.WriteLine($"   DB average GPA: {context.Students.Average(s => s.Gpa):F2}");
                    Console.WriteLine($"   DB student count: {context.Students.Count()}");
                }
            }
            catch (Exception ex)
            {
                // Expected before the migration is applied — Block 4
                // demonstrates this failure deliberately.
                Console.WriteLine();
                Console.WriteLine("EF section could not run:");
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine("  Most likely causes, in order:");
                Console.WriteLine("   1. Migration not applied — run Add-Migration then Update-Database");
                Console.WriteLine("   2. Wrong Server= in the connection string (check against SSMS)");
                Console.WriteLine("   3. Missing TrustServerCertificate=True");
            }

            Console.WriteLine();
            Console.WriteLine("Done.");
        }
    }
}
