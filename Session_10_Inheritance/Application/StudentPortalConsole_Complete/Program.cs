// =====================================================================
// StudentPortalConsole_Complete — FULL WORKING FALLBACK (Rule 20)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 10 — Inheritance
//
// Complete, correct, runnable version of everything taught live today.
// Matches Instructor_Guide_EN.md and Student_Guide.md exactly (Rule 15).
// Refactors Session 09's Student/Instructor to derive from a shared
// Person base class — see the Closing Reference in the Instructor
// Guide for the exact same shape.
// Run with: dotnet run (or Visual Studio's Start Without Debugging)
// =====================================================================

namespace StudentPortalConsole
{
    public static class myID
    {
        public static int ID = 34;
        public static int id_mod_3 = ID % 3 + 2;
    }
    internal class Program
    {
        class Person
        {
            protected string fullName;

            public string FullName
            {
                get { return fullName; }
                set { fullName = value; }
            }

            public Person(string fullName)
            {
                this.fullName = fullName;
            }

            public void PrintBasicInfo()
            {
                Console.WriteLine($"Person: {fullName}");
            }
        }

        class Student : Person
        {
            private static int totalStudentsCreated = 0;

            private int yearOfStudy;
            private double gpa;

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
            public Student(string fullName, int yearOfStudy)
                : this(fullName, yearOfStudy, 0.0)
            {
            }

            // Main constructor — chains to Person's constructor for the shared "Person part."
            public Student(string fullName, int yearOfStudy, double gpa)
                : base(fullName)
            {
                YearOfStudy = yearOfStudy;   // through the property, so validation applies
                Gpa = gpa;                    // through the property, so validation applies
                totalStudentsCreated++;
            }

            public void UpdateGpa(double newGpa)
            {
                UpdateGpa(newGpa, "No reason given");
            }

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
                base.PrintBasicInfo();
                string line = $"  {ClassifyYear()}, GPA {gpa:F2}";
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

        class Instructor : Person
        {
            private int yearsOfExperience;

            public int YearsOfExperience
            {
                get { return yearsOfExperience; }
                set
                {
                    if (value >= 0)
                    {
                        yearsOfExperience = value;
                    }
                }
            }

            // Association, unchanged from Session 09 — a plain name, not a Course reference.
            public string? AssignedCourseName { get; set; }

            public Instructor(string fullName, int yearsOfExperience)
                : base(fullName)
            {
                YearsOfExperience = yearsOfExperience;
            }

            public void PrintSummary()
            {
                base.PrintBasicInfo();
                string line = $"  {yearsOfExperience} years of experience";
                if (AssignedCourseName != null)
                {
                    line += $", currently associated with {AssignedCourseName}";
                }
                Console.WriteLine(line);
            }
        }

        class Admin : Person
        {
            private int _accessLevel;
            public int accessrange
            {
                get { return _accessLevel; }
                set
                {
                    if (value >= 1 && value <= (myID.id_mod_3))
                    {
                        _accessLevel = value;
                    }
                    else
                    {
                        Console.WriteLine($"Access level must be between 1 and {myID.id_mod_3}.");
                    }
                }
            }
            public Admin(string fullName) : base(fullName)
            {
            }
            public Admin(String fullName, int accesslev) : this(fullName)
            {
                accessrange = accesslev;
            }
            public void PrintSummary()
            {
                base.PrintBasicInfo();
                Console.WriteLine($"Access Level : {accessrange}");
            }
        }

        static Person? FindPersonByName(List<Person> people, string name)
        {
            foreach (Person p in people)
            {
                if (p.FullName == name) return p;
            }
            return null;
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

        static void Main(string[] args)
        {
            List<Person> everyone = new List<Person>();

            bool keepRunning = true;

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== StudentPortal Menu ===");
                Console.WriteLine("1. Register a new student (with GPA)");
                Console.WriteLine("2. Register a new student (no GPA yet)");
                Console.WriteLine("3. Register a new instructor");
                Console.WriteLine("4. Register a new admin");
                Console.WriteLine("5. Print everyone's basic info (Person-level)");
                Console.WriteLine("6. Print one person's full summary (by name)");
                Console.WriteLine("7. Show total students ever created");
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
                        everyone.Add(new Student(name1, year1, gpa1));
                        Console.WriteLine("Student registered with a starting GPA.");
                        break;

                    case "2":
                        Console.WriteLine("Enter the student's full name:");
                        string name2 = Console.ReadLine() ?? "";
                        int year2 = ReadValidYear();
                        everyone.Add(new Student(name2, year2));   // overloaded constructor
                        Console.WriteLine("Student registered with no GPA yet (defaults to 0.00).");
                        break;

                    case "3":
                        Console.WriteLine("Enter the instructor's full name:");
                        string name3 = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter years of experience:");
                        string? expInput = Console.ReadLine();
                        int.TryParse(expInput, out int years3);
                        everyone.Add(new Instructor(name3, years3));
                        Console.WriteLine("Instructor registered.");
                        break;

                    case "4":
                        Console.WriteLine("Enter the admin's full name:");
                        string name4 = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter access level:");
                        string? accessInput = Console.ReadLine();
                        int.TryParse(accessInput, out int accessLevel);
                        everyone.Add(new Admin(name4, accessLevel));
                        Console.WriteLine("Admin registered.");
                        break;


                    case "5":
                        Console.WriteLine("=== Everyone (Person-level view) ===");
                        foreach (Person p in everyone)
                        {
                            p.PrintBasicInfo();
                        }
                        break;

                    case "6":
                        Console.WriteLine("Enter the full name to look up:");
                        string lookupName = Console.ReadLine() ?? "";
                        Person? found = FindPersonByName(everyone, lookupName);
                        if (found == null)
                        {
                            Console.WriteLine("No one found with that name.");
                            break;
                        }
                        
                        Student? asStudent = found as Student;
                        Instructor? asInstructor = found as Instructor;
                        Admin? asAdmin = found as Admin;
                        if (asStudent != null)
                        {
                            asStudent.PrintSummary();
                        }
                        else if (asInstructor != null)
                        {
                            asInstructor.PrintSummary();
                        }
                        else if (asAdmin != null)
                        {
                            asAdmin.PrintSummary();
                        }   
                        break;

                    case "7":
                        Console.WriteLine($"Total students ever created: {Student.GetTotalStudents()}");
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
