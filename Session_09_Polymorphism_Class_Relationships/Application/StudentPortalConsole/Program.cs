// =====================================================================
// StudentPortalConsole — TODO GUIDE ONLY (Style Guide Rule 20 / Rule 35)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 09 — Polymorphism + Class Relationships
//
// This file holds VERBAL/TODO guidance only — NOT working code. Build
// this yourself, from scratch, following the TODOs below in order.
// Every pattern here was demonstrated live in today's lecture — the
// Student Guide has the exact worked examples if you get stuck.
//
// This EXTENDS Session 08's encapsulated Student class with today's
// two topics: Polymorphism (method overloading, constructor
// overloading with chaining, operator overloading) and Class
// Relationships (Course now genuinely holds its enrolled Students —
// an Aggregation relationship, built live).
//
// For the full, correct, runnable version (do NOT peek until you've
// tried it yourself, or you're using it to check your own work), see:
// ../StudentPortalConsole_Complete/Program.cs
// =====================================================================

namespace StudentPortalConsole
{
    class Student
    {
        // Fields
        private static int _totalStudentsCreated = 0; // static field (static member)
        private string _fullName; // instance field
        private double _gpa;
        private int _yearOfStudy;

        //public Student()
        //{
        //    Console.WriteLine($"Constructor Called for student");
        //}

        // Properties
        public string FullName
        { // Full-Property
            get { return _fullName; }
            set { _fullName = value; }
        }

        public double GPA
        {
            get { return _gpa; }
            set
            {
                if (value < 0.0 || value > 4.0)
                {
                    Console.WriteLine("Invalid GPA");
                    return;
                }
                _gpa = value;
            }
        }

        public int YearOfStudy
        {
            get { return _yearOfStudy; }
            set
            {
                if (value < 1 || value > 4)
                {
                    Console.WriteLine("Invalid year of study");
                    return;
                }
                _yearOfStudy = value;
            }
        }

        public Student(string fullName, int yearOfStudy) :
            this(fullName, 0.0, yearOfStudy)
        {
            Console.WriteLine($"Constructor Called for unknown GPA for student {fullName}");
        }

        public Student(string FullName, double GPA, int YearOfStudy)
        {
            // Initialization
            Console.WriteLine($"Constructor Called for student {FullName}");
            this.FullName = FullName;
            this.GPA = GPA;
            this.YearOfStudy = YearOfStudy;
            _totalStudentsCreated++;
        }



        public void PrintSummary()
        {
            Console.WriteLine($"{FullName} - GPA {GPA} - Year {YearOfStudy}");
        }

        public static int GetTotalStudentsCreated()
        {
            return _totalStudentsCreated;
        }

        public void UpdateGpa(double newGpa) // Signature : MethodName + Parameters(Order , Types , Numbers)
        {
            UpdateGpa(newGpa, "No Reason Given");
        }

        public void UpdateGpa(double newGpa, string reason = "No Reason Given")
        {
            GPA = newGpa;
            Console.WriteLine($"{FullName}'s GPA updated to {newGpa:F2}. Reason: {reason}");
        }
        public void UpdateGpa(string resaon)
        {
            UpdateGpa(GPA, resaon);
        }

        public int Add(int number1, int number2)
        {
            return number1 + number2;
        }
        public double Add(double number1, double number2)
        {
            return number1 + number2;
        }
        //public double Add(int number1 , int number2)
        //{
        //    return number1 + number2;
        //}

        public static bool operator >(Student a, Student b)
        {
            return a.GPA > b.GPA;
        }
        public static bool operator <(Student a, Student b)
        {
            return a.GPA < b.GPA;
        }

        public static bool operator ==(Student a, Student b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.FullName == b.FullName && a.GPA == b.GPA && a.YearOfStudy == b.YearOfStudy;
        }
        public static bool operator !=(Student a, Student b)
        {
            return !(a == b);
        }

        //public override bool Equals(object? obj)
        //{
        //    return obj is Student other && this == other;
        //}
    }

    class Course
    {
        private static int _totalCoursesCreated = 0;
        private string _courseName;
        private int _credits;
        private List<Student> _enrolledStudents = new();

        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; }
        }
        public int Credits
        {
            get { return _credits; }
            set
            {
                if (value >= 1 && value <= 6)
                    _credits = value;
            }
        }

        public Course(string courseName, int credits)
        {
            CourseName = courseName;
            Credits = credits;
            _totalCoursesCreated++;
        }

        //PART C
        //C.1
        public Course(string courseName) : this(courseName, 3)
        {

        }

        public void EnrollStudent(Student student)
        {
            _enrolledStudents.Add(student);
        }

        //C.2
        public void EnrollStudent(Student student, string textNote)
        {
            EnrollStudent(student);
            Console.WriteLine($"Student Name: {student.FullName}, Note: {textNote}");
        }

        public void PrintRoster()
        {
            Console.WriteLine($"{CourseName} - {Credits} credits , Enrolled Students : {_enrolledStudents.Count}");
            //for (int i = 0; i < _enrolledStudents.Count; i++)
            //{
            //    _enrolledStudents[i].PrintSummary();
            //}

            foreach (var student in _enrolledStudents)
            {
                student.PrintSummary();
            }
        }

        public static int GetTotalCourses()
        {
            return _totalCoursesCreated;
        }

        //PART D
        //D.1
        public static bool operator >(Course a, Course b)
        {
            return a.Credits > b.Credits;
        }
        public static bool operator <(Course a, Course b)
        {
            return a.Credits < b.Credits;
        }

        //D.2
        public static bool operator ==(Course a, Course b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.CourseName == b.CourseName && a.Credits == b.Credits;
        }

        public static bool operator !=(Course a, Course b)
        {
            return !(a == b);
        }

    }

    //PART E
    class Instructor
    {
        //E.1
        private string _fullName;
        private int _yearsOfExperience;
        private string _assignedCourseName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public int YearsOfExperience
        {
            get { return _yearsOfExperience; }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Invalid years of experience");
                    return;
                }
                _yearsOfExperience = value;
            }
        }

        //E.2
        public Instructor(string fullname, int yearsOfExperience)
        {
            FullName = fullname;
            YearsOfExperience = yearsOfExperience;
        }

        //E.3
        public string AssignedCourseName
        {
            get { return _assignedCourseName; }
            set { _assignedCourseName = value; }
        }

        //E.4
        public void printSummry()
        {
            Console.WriteLine($"Instructor Name : {FullName}, Years of Experience: {YearsOfExperience}");
            if (!string.IsNullOrEmpty(AssignedCourseName))
            {
                Console.WriteLine($" and Course Name is : {AssignedCourseName}");
            }
            else
            {
                Console.WriteLine(" ");
            }
        }

    }


    internal class Program
    {
        // ===== Student class =====

        // TODO 1: Define a class named Student. Give it three private
        //         fields matching Session 08's version: a text field
        //         for the full name, a whole-number field for year of
        //         study, and a decimal-number field for GPA. Also add
        //         one private static whole-number field to track how
        //         many Student objects have been created in total.



        // TODO 2: Give Student a public property for the full name
        //         that simply reads and writes the private name field,
        //         no extra validation needed.

        // TODO 3: Give Student a public property for year of study.
        //         Its setter should only accept whole numbers between
        //         1 and 4 (inclusive), silently rejecting anything
        //         outside that range.

        // TODO 4: Give Student a public property for GPA. Its setter
        //         should only accept decimal values between 0.0 and
        //         4.0 (inclusive), silently rejecting anything outside
        //         that range.

        // TODO 5: Give Student its main constructor, taking a full
        //         name, a year of study, and a GPA. Assign the full
        //         name directly to its private field (using `this.`
        //         to resolve the naming collision), but assign year of
        //         study and GPA THROUGH their properties (TODO 3 and
        //         TODO 4), never directly to their private fields, so
        //         even a constructor call with bad data gets the same
        //         validation as any other assignment. Increment the
        //         static total-count field from TODO 1 inside this
        //         constructor.

        // TODO 6: Give Student a PUBLIC method that updates GPA given
        //         just a new decimal GPA value, with no logged reason.
        //         Its body should not repeat any assignment logic
        //         itself — instead, have it call the OTHER GPA-update
        //         method described in TODO 7, supplying a generic
        //         placeholder reason like "No reason given," so the
        //         real update logic exists in exactly one place.

        // TODO 7: Give Student a SECOND public method with the exact
        //         same name as TODO 6's method, but this version takes
        //         two things: a new decimal GPA value AND a text
        //         reason for the change. This is the version that
        //         actually assigns the new GPA (through the property,
        //         from TODO 4) and prints one line naming the student,
        //         their new GPA, and the reason given. Because this
        //         method shares a name with TODO 6's method but has a
        //         different parameter list, this is method OVERLOADING
        //         — the compiler picks whichever one matches how it's
        //         called.

        // TODO 8: Give Student a SECOND constructor, taking only a
        //         full name and a year of study — no GPA parameter at
        //         all. Its body should be empty; instead, immediately
        //         after its parameter list, chain to the TODO 5
        //         constructor using the colon-this syntax, passing the
        //         full name and year of study through unchanged and
        //         supplying 0.0 as the GPA value. This lets a Student
        //         be created the moment they enroll, before they have
        //         any GPA at all, while still guaranteeing every field
        //         goes through the exact same validation as any other
        //         Student — because it's the exact same constructor
        //         body actually doing the work underneath.

        // TODO 9: Give Student two special static methods with the
        //         reserved name pattern for the greater-than and
        //         less-than symbols — these let two Student objects be
        //         compared directly with `>` and `<` in code, the same
        //         way two plain numbers can be. Each should take two
        //         Student objects as its two inputs and return true or
        //         false based on comparing their GPA values (greater
        //         for one, less for the other).

        // TODO 10: Give Student two more special static methods, for
        //          the equal-to and not-equal-to symbols. These two
        //          MUST be written together — C# will not compile one
        //          without the other. The equal-to version should
        //          first check whether both inputs are literally the
        //          same object in memory (accept immediately if so),
        //          then check whether either input has no object at
        //          all (reject immediately if so, to avoid a crash),
        //          and only then compare both the full name AND the
        //          GPA of the two Student objects for equality. The
        //          not-equal-to version should simply return the exact
        //          opposite of whatever the equal-to version returns —
        //          no need to repeat its comparison logic separately.

        // TODO 11: Give Student two PRIVATE methods, unchanged from
        //          Session 08 — one that reads the object's own year
        //          of study and returns a text label for it
        //          ("Freshman" through "Senior"), and one that reads
        //          the object's own GPA and returns an honor-status
        //          label ("Dean's List" / "Honor Roll" / "Standard
        //          Standing").

        // TODO 12: Give Student a PUBLIC method named PrintSummary
        //          that takes one true/false parameter (defaulting to
        //          true) controlling whether honor status is included,
        //          exactly like Session 08. It prints one line with
        //          the object's own name, year label, GPA, and — only
        //          if the true/false parameter says to — honor status.

        // TODO 13: Give Student a PUBLIC STATIC method, unchanged from
        //          Session 08, that returns the current value of the
        //          TODO 1 total-count field.

        // ===== Course class =====

        // TODO 14: Define a class named Course, matching what you
        //          built in yesterday's lab: a private text field for
        //          the course name, a private whole-number field for
        //          credits, and one private static whole-number field
        //          tracking how many Course objects have been created
        //          in total. Also give it a private field that holds a
        //          LIST of Student objects — this will be the roster
        //          of everyone enrolled, and it should start out as an
        //          empty list the moment a Course object is created,
        //          not something set up later.

        // TODO 15: Give Course a public property for the course name —
        //          simple get/set, reading and writing the private
        //          field from TODO 14, no validation needed.

        // TODO 16: Give Course a public property for credits. Its
        //          setter should only accept whole numbers between 1
        //          and 6 (inclusive), silently rejecting anything
        //          outside that range.

        // TODO 17: Give Course a constructor taking a course name and
        //          a credit count, assigning both THROUGH their
        //          properties (TODO 15 and TODO 16), and incrementing
        //          the static total-count field from TODO 14.

        // TODO 18: Give Course a PUBLIC method that takes one Student
        //          object as its input and adds it to the roster list
        //          from TODO 14. This is the ONLY way outside code can
        //          add to the roster — the list itself stays private.
        //          Notice this method does NOT create a new Student
        //          itself; it only stores a reference to a Student
        //          object that already exists, created independently,
        //          somewhere else — that's what makes this an
        //          Aggregation relationship, not a Composition one.

        // TODO 19: Give Course a PUBLIC method that prints a small
        //          header naming the course and how many students are
        //          currently enrolled (the roster list's own count),
        //          then loops over every Student currently in the
        //          roster and calls each one's own PrintSummary method
        //          (TODO 12), passing false so honor status is left
        //          out of this compact roster listing.

        // TODO 20: Give Course a PUBLIC STATIC method that returns the
        //          current value of the TODO 14 total-count field.


        static void Main(string[] args)
        {

            Student demoStudent1 = new Student("Test", 3.5, 4);
            demoStudent1.UpdateGpa(3.7);
            demoStudent1.Add(2, 3); // Better matching rules

            Student student2 = new Student("Test2", 4);
            student2.GPA = 4;

            Console.WriteLine(demoStudent1 > student2);

            Course Database = new Course("Database Fundamentals", 3);
            Database.EnrollStudent(demoStudent1);
            Database.EnrollStudent(student2);

            Database.PrintRoster();

            //PART D
            Course a = new Course("Course A", 3);
            Course b = new Course("Course B", 4);

            Console.WriteLine(a > b);
            Console.WriteLine(a < b);

            Course EqualA = new Course("CourseEqual A", 3);
            Course EqualB = new Course("CourseEqual B", 3);

            Console.WriteLine(EqualA == EqualB);
            Console.WriteLine(EqualA != EqualB);


            //PART E
            List<Instructor> instructors = new();


            while (true)
            {
                Console.WriteLine("1. Register an Instructor");
                Console.WriteLine("2. Assign Instructor to a Course");
                Console.WriteLine("3. Print all Instructors");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter Instructor Full Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter Years of Experience: ");
                            if (int.TryParse(Console.ReadLine(), out int years))
                            {
                                instructors.Add(new Instructor(name, years));
                            }
                            else
                            {
                                Console.WriteLine("Invalid number for years of experience.");
                            }
                            break;

                        case 2:
                            Console.Write("Enter the Instructor's Name");
                            string Instructorname = Console.ReadLine();

                            Instructor foundInstructor = null;
                            foreach (var instructor in instructors)
                            {
                                if (instructor.FullName == Instructorname)
                                {
                                    foundInstructor = instructor;
                                    break;
                                }
                            }

                            if (foundInstructor != null)
                            {
                                Console.Write("Enter the Course Name to assign them to: ");
                                string courseName = Console.ReadLine();
                                foundInstructor.AssignedCourseName = courseName;
                                Console.WriteLine($" Instructor {foundInstructor.FullName}  to {courseName}.");
                            }
                            else
                            {
                                Console.WriteLine(" Instructor not found.");
                            }
                            break;

                        case 3:
                            Console.WriteLine("\n  Instructors :");

                            foreach (var inst in instructors)
                            {
                                inst.printSummry();
                            }
                            break;

                        case 4:
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please pick 1-4.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(" Invalid input. Please enter a number.");
                }
            }

            // TODO 21: Create at least two Student objects: one using
            //          the full three-value constructor from TODO 5,
            //          and at least one using the newer two-value
            //          constructor from TODO 8, proving both
            //          overloaded constructors work. Print whether the
            //          first Student's GPA is greater than the
            //          second's using the `>` operator from TODO 9 —
            //          not a method call, the actual symbol.

            // TODO 22: Call one of the Student objects' GPA-update
            //          method (TODO 6 or TODO 7) at least once each —
            //          once with just a new GPA value, once with a new
            //          GPA value AND a reason — proving both
            //          overloads work from a real call site, not just
            //          existing in the class.

            // TODO 23: Create at least two Course objects. Enroll at
            //          least one Student object in MORE than one
            //          Course (the exact same Student object,
            //          reused — not a second Student created with the
            //          same data) using TODO 18's method, then print
            //          each Course's roster using TODO 19's method.
            //          Seeing the same student's name appear on more
            //          than one roster is your own live proof of
            //          Aggregation — that object was never owned
            //          exclusively by either Course.

            // TODO 24: Print the total number of Students ever created
            //          and the total number of Courses ever created,
            //          using TODO 13 and TODO 20's static methods,
            //          called on the CLASS names themselves
            //          (Student and Course), never on any one object.
        }
    }

    #region 📋 Full TODO Checklist (collapse this region for a quick overview)
    // 1.  Define the Student class: private fields + a static total-count field
    // 2.  Add the FullName property (simple get/set, no validation)
    // 3.  Add the YearOfStudy property (validates 1-4)
    // 4.  Add the Gpa property (validates 0.0-4.0)
    // 5.  Add the main 3-parameter constructor (routes through properties, increments the static count)
    // 6.  Add the 1-parameter UpdateGpa overload (delegates to the 2-parameter version)
    // 7.  Add the 2-parameter UpdateGpa overload (does the real work: assigns + prints)
    // 8.  Add the 2-parameter constructor, chained via `: this(...)` to the 3-parameter one, defaulting GPA to 0.0
    // 9.  Add the `>` and `<` operator overloads, comparing by GPA
    // 10. Add the `==` and `!=` operator overloads together (name + GPA comparison, with null/reference guards)
    // 11. Add the private year-classification and honor-status-classification methods
    // 12. Add the public PrintSummary method (uses TODO 11's methods internally)
    // 13. Add the public static method that returns the Student total-count field
    // 14. Define the Course class: private fields + a static total-count field + an empty Student list field
    // 15. Add the CourseName property (simple get/set, no validation)
    // 16. Add the Credits property (validates 1-6)
    // 17. Add the constructor (assigns through properties, increments the static count)
    // 18. Add the EnrollStudent method (stores a reference to an already-existing Student — Aggregation)
    // 19. Add the PrintRoster method (loops the roster, calls each Student's PrintSummary(false))
    // 20. Add the public static method that returns the Course total-count field
    // 21. Create Students with BOTH constructors; compare two with the `>` operator
    // 22. Call both UpdateGpa overloads on a real Student object
    // 23. Enroll the same Student in more than one Course; print both rosters
    // 24. Print total Students and total Courses ever created, called on the class names
    #endregion
}
