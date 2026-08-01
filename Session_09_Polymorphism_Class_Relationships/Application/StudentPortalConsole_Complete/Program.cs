// =====================================================================
// StudentPortalConsole_Complete — FULL WORKING FALLBACK (Rule 20)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 09 — Polymorphism + Class Relationships
//
// Complete, correct, runnable version of everything taught live today.
// Matches Instructor_Guide_EN.md and Student_Guide.md exactly (Rule 15).
// Extends Session 08's encapsulated Student with method/constructor/
// operator overloading, and gives Course a real Aggregation
// relationship to Student via a live roster.
// Run with: dotnet run (or Visual Studio's Start Without Debugging)
// =====================================================================

namespace StudentPortalConsole
{
    internal class Program
    {
        class Student
        {
            private static int totalStudentsCreated = 0;

            private string fullName;
            private int yearOfStudy;
            private double gpa;

            public string FullName
            {
                get { return fullName; }
                set { fullName = value; }
            }

            public int YearOfStudy
            {
                get { return yearOfStudy; }
                set
                {
                    if (value >= 1 && value <= 4)
                    {
                        yearOfStudy = value;
                    }
                }
            }

            public double Gpa
            {
                get { return gpa; }
                set
                {
                    if (value >= 0.0 && value <= 4.0)
                    {
                        gpa = value;
                    }
                }
            }

            // Overloaded constructor #1 — no GPA known yet at enrollment time.
            // Chains to the main constructor below, supplying 0.0 for gpa.
            public Student(string fullName, int yearOfStudy)
                : this(fullName, yearOfStudy, 0.0)
            {
            }

            // Main constructor — the one real place a Student is actually built.
            public Student(string fullName, int yearOfStudy, double gpa)
            {
                this.fullName = fullName;
                YearOfStudy = yearOfStudy;   // through the property, so validation applies
                Gpa = gpa;                    // through the property, so validation applies
                totalStudentsCreated++;
            }

            // Overloaded method #1 — routine update, no reason logged.
            public void UpdateGpa(double newGpa)
            {
                UpdateGpa(newGpa, "No reason given");
            }

            // Overloaded method #2 — update with a logged reason.
            public void UpdateGpa(double newGpa, string reason)
            {
                Gpa = newGpa;
                Console.WriteLine($"{fullName}'s GPA updated to {newGpa:F2}. Reason: {reason}");
            }

            private string ClassifyYear()
            {
                switch (yearOfStudy)
                {
                    case 1: return "Freshman";
                    case 2: return "Sophomore";
                    case 3: return "Junior";
                    case 4: return "Senior";
                    default: return "Unknown Year";
                }
            }

            private string ClassifyHonorStatus()
            {
                if (gpa >= 3.5) return "Dean's List";
                if (gpa >= 3.0) return "Honor Roll";
                return "Standard Standing";
            }

            public void PrintSummary(bool includeHonors = true)
            {
                string line = $"{fullName} — {ClassifyYear()}, GPA {gpa:F2}";
                if (includeHonors)
                {
                    line += $", {ClassifyHonorStatus()}";
                }
                Console.WriteLine(line);
            }

            public static int GetTotalStudents()
            {
                return totalStudentsCreated;
            }

            // Operator overloads — compare two Students the same way you'd compare two numbers.
            public static bool operator >(Student a, Student b)
            {
                return a.Gpa > b.Gpa;
            }

            public static bool operator <(Student a, Student b)
            {
                return a.Gpa < b.Gpa;
            }

            public static bool operator ==(Student a, Student b)
            {
                if (ReferenceEquals(a, b)) return true;
                if (a is null || b is null) return false;
                return a.fullName == b.fullName && a.Gpa == b.Gpa;
            }

            public static bool operator !=(Student a, Student b)
            {
                return !(a == b);
            }

            // Overriding Equals/GetHashCode to stay consistent with the == / != overloads
            // above (recommended practice — see Session 09, Block 3 Q&A).
            public override bool Equals(object? obj)
            {
                if (obj is Student other) return this == other;
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(fullName, gpa);
            }
        }

        class Course
        {
            private static int totalCoursesCreated = 0;

            private string courseName;
            private int credits;
            private List<Student> enrolledStudents = new List<Student>();

            public string CourseName
            {
                get { return courseName; }
                set { courseName = value; }
            }

            public int Credits
            {
                get { return credits; }
                set
                {
                    if (value >= 1 && value <= 6)
                    {
                        credits = value;
                    }
                }
            }

            public Course(string courseName, int credits)
            {
                CourseName = courseName;
                Credits = credits;
                totalCoursesCreated++;
            }

            // Aggregation: stores a reference to a Student that already exists.
            // Never creates one itself — that's what makes this Aggregation, not Composition.
            public void EnrollStudent(Student s)
            {
                enrolledStudents.Add(s);
            }

            public void PrintRoster()
            {
                Console.WriteLine($"=== {courseName} Roster ({enrolledStudents.Count} enrolled) ===");
                foreach (Student s in enrolledStudents)
                {
                    s.PrintSummary(false);
                }
            }

            public static int GetTotalCourses()
            {
                return totalCoursesCreated;
            }
        }

        static int ReadValidYear()
        {
            int year = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine("Enter year of study (1-4):");
                string? input = Console.ReadLine();
                isValid = int.TryParse(input, out year) && year >= 1 && year <= 4;

                if (!isValid)
                {
                    Console.WriteLine("That's not a valid year. Try again.");
                }
            }
            return year;
        }

        static double ReadValidGpa()
        {
            double gpa = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine("Enter GPA (0.0-4.0):");
                string? input = Console.ReadLine();
                isValid = double.TryParse(input, out gpa) && gpa >= 0.0 && gpa <= 4.0;

                if (!isValid)
                {
                    Console.WriteLine("That's not a valid GPA. Try again.");
                }
            }
            return gpa;
        }

        static Course? FindCourseByName(List<Course> courses, string name)
        {
            foreach (Course c in courses)
            {
                if (c.CourseName == name) return c;
            }
            return null;
        }

        static Student? FindStudentByName(List<Student> students, string name)
        {
            foreach (Student s in students)
            {
                if (s.FullName == name) return s;
            }
            return null;
        }

        static void Main(string[] args)
        {
            List<Student> students = new List<Student>();
            List<Course> courses = new List<Course>();
            bool keepRunning = true;

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== StudentPortal Menu ===");
                Console.WriteLine("1. Register a new student (with GPA)");
                Console.WriteLine("2. Register a new student (no GPA yet)");
                Console.WriteLine("3. Update a student's GPA");
                Console.WriteLine("4. Create a course");
                Console.WriteLine("5. Enroll a student in a course");
                Console.WriteLine("6. Print a course roster");
                Console.WriteLine("7. Compare two students by GPA (>)");
                Console.WriteLine("8. Show total students/courses ever created");
                Console.WriteLine("0. Quit");
                Console.WriteLine("Choose an option:");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter the student's full name:");
                        string name1 = Console.ReadLine() ?? "";
                        int year1 = ReadValidYear();
                        double gpa1 = ReadValidGpa();
                        students.Add(new Student(name1, year1, gpa1));
                        Console.WriteLine("Student registered with a starting GPA.");
                        break;

                    case "2":
                        Console.WriteLine("Enter the student's full name:");
                        string name2 = Console.ReadLine() ?? "";
                        int year2 = ReadValidYear();
                        students.Add(new Student(name2, year2));   // overloaded constructor
                        Console.WriteLine("Student registered with no GPA yet (defaults to 0.00).");
                        break;

                    case "3":
                        Console.WriteLine("Enter the student's full name to update:");
                        string updateName = Console.ReadLine() ?? "";
                        Student? toUpdate = FindStudentByName(students, updateName);
                        if (toUpdate == null)
                        {
                            Console.WriteLine("No student found with that name.");
                            break;
                        }
                        double newGpa = ReadValidGpa();
                        Console.WriteLine("Log a reason? (y/n)");
                        string? logReason = Console.ReadLine();
                        if (logReason == "y")
                        {
                            Console.WriteLine("Enter the reason:");
                            string reason = Console.ReadLine() ?? "";
                            toUpdate.UpdateGpa(newGpa, reason);   // 2-parameter overload
                        }
                        else
                        {
                            toUpdate.UpdateGpa(newGpa);           // 1-parameter overload
                        }
                        break;

                    case "4":
                        Console.WriteLine("Enter the course name:");
                        string courseName = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter credits (1-6):");
                        string? creditsInput = Console.ReadLine();
                        int.TryParse(creditsInput, out int credits);
                        courses.Add(new Course(courseName, credits));
                        Console.WriteLine("Course created.");
                        break;

                    case "5":
                        Console.WriteLine("Enter the student's full name:");
                        string enrollStudentName = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter the course name:");
                        string enrollCourseName = Console.ReadLine() ?? "";
                        Student? studentToEnroll = FindStudentByName(students, enrollStudentName);
                        Course? courseToEnrollIn = FindCourseByName(courses, enrollCourseName);
                        if (studentToEnroll == null || courseToEnrollIn == null)
                        {
                            Console.WriteLine("Student or course not found.");
                            break;
                        }
                        courseToEnrollIn.EnrollStudent(studentToEnroll);
                        Console.WriteLine("Student enrolled.");
                        break;

                    case "6":
                        Console.WriteLine("Enter the course name:");
                        string rosterCourseName = Console.ReadLine() ?? "";
                        Course? rosterCourse = FindCourseByName(courses, rosterCourseName);
                        if (rosterCourse == null)
                        {
                            Console.WriteLine("No course found with that name.");
                            break;
                        }
                        rosterCourse.PrintRoster();
                        break;

                    case "7":
                        Console.WriteLine("Enter the first student's full name:");
                        string firstName = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter the second student's full name:");
                        string secondName = Console.ReadLine() ?? "";
                        Student? first = FindStudentByName(students, firstName);
                        Student? second = FindStudentByName(students, secondName);
                        if (first == null || second == null)
                        {
                            Console.WriteLine("One or both students not found.");
                            break;
                        }
                        Console.WriteLine($"{first.FullName} > {second.FullName} by GPA: {first > second}");
                        break;

                    case "8":
                        Console.WriteLine($"Total students ever created: {Student.GetTotalStudents()}");
                        Console.WriteLine($"Total courses ever created: {Course.GetTotalCourses()}");
                        break;

                    case "0":
                        keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }

            } while (keepRunning);

            Console.WriteLine("Goodbye!");
        }
    }
}
