// =====================================================================
// StudentPortalConsole — TODO GUIDE ONLY (Style Guide Rule 20 / Rule 35)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 10 — Inheritance
//
// This file holds VERBAL/TODO guidance only — NOT working code. Build
// this yourself, from scratch, following the TODOs below in order.
// Every pattern here was demonstrated live in today's lecture — the
// Student Guide has the exact worked examples if you get stuck.
//
// This REFACTORS Session 09's Student and Instructor classes: their
// duplicated full-name field/property/constructor logic is pulled up
// into one shared Person base class. Student and Instructor become
// derived classes, chaining their constructors to Person's with
// `base(...)`. Everything else about Student/Instructor from Session
// 09 (GPA validation, overloaded methods, operator overloads, the
// AssignedCourseName Association) is UNCHANGED — only the fullName
// piece moves.
//
// For the full, correct, runnable version (do NOT peek until you've
// tried it yourself, or you're using it to check your own work), see:
// ../StudentPortalConsole_Complete/Program.cs
// =====================================================================

using System.Globalization;

namespace StudentPortalConsole
{
    
    public class Person
    {
        protected string fullName;
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        public Person(string fullName)
        {
            Console.WriteLine("Person Constructor");
            FullName = fullName;
        }
        public void PrintBasicInfo()
        {
            Console.WriteLine($"Person : {FullName}");
        }
        // this is protected because it is only used by the derived classes, not by the outside world
        // and not private because it is used by the derived classes, not by the outside world
        // it is not public because it is not used by the outside world
        protected void FormatTag()
        {
            Console.WriteLine($"Tag : {fullName.Substring(0, myID.id_mod_3)}");
            
        }
    }

    public class Student : Person // Student Is A Person
    {
        private int yearOfStudy;
        private double gpa;

        public int YearOfStudy
        {
            get { return yearOfStudy; }
            set { yearOfStudy = value; }
        }

        public double Gpa
        {
            get { return gpa; }
            set { gpa = value; }
        }
        public Student(string fullName, int yearOfStudy, double gpa) 
            : base(fullName)
        {
            Console.WriteLine("Student Constructor");
            YearOfStudy = yearOfStudy;
            Gpa = gpa;
        }

        //private string GetShortName()
        //{
        //    return fullName.Substring(0, 3).ToUpper();
        //}
        public void PrintSummary() 
        {
            base.PrintBasicInfo(); // Student -> person -> baseClass
            Console.WriteLine($"Year of Study : {YearOfStudy}");
            Console.WriteLine($"GPA : {Gpa}");
            base.FormatTag();
        }
    }
    public class Instructor : Person // Instructor Is A Person
    {
        private int yearsOfExperience;
        public int YearsOfExperience 
        {
            get { return yearsOfExperience; }
            set {  yearsOfExperience = value; }
        }


        public Instructor(string fullName , int yearsOfExperience) : base(fullName)
        {
            YearsOfExperience = yearsOfExperience;
        }
        public void PrintSummary()
        {
            base.PrintBasicInfo();
            Console.WriteLine($"Years of Experience : {YearsOfExperience}");
            base.FormatTag(); 
        }
    }

    

    internal class Program
    {
        // ===== Person base class =====

        // TODO 1: Define a class named Person — this will be the base
        //         class both Student and Instructor derive from. Give
        //         it one PROTECTED (not private) text field for the
        //         full name — protected specifically so a derived
        //         class's own code can reach it directly, unlike a
        //         private field, which no derived class could touch
        //         at all. Add a public property for the full name
        //         that simply reads and writes that protected field,
        //         no extra validation needed — this is for code
        //         OUTSIDE the Person family, unrelated to today's
        //         protected-access lesson. Add a constructor taking a
        //         full name and assigning it to the protected field
        //         (using `this.` to resolve the naming collision).
        //         Finally, add a public method with no parameters
        //         that prints a single line naming the object's own
        //         full name — this method will be inherited, as-is,
        //         by every class that derives from Person.

        // ===== Student class, now derived from Person =====

        // TODO 2: Define a class named Student that DERIVES from
        //         Person (using the colon syntax). Give it two
        //         private fields matching Session 09's version: a
        //         whole-number field for year of study and a
        //         decimal-number field for GPA — do NOT redeclare a
        //         full-name field here at all; it's inherited from
        //         Person now. Also add one private static
        //         whole-number field to track how many Student
        //         objects have been created in total.

        // TODO 3: Give Student a public property for year of study.
        //         Its setter should only accept whole numbers between
        //         1 and 4 (inclusive), silently rejecting anything
        //         outside that range.

        // TODO 4: Give Student a public property for GPA. Its setter
        //         should only accept decimal values between 0.0 and
        //         4.0 (inclusive), silently rejecting anything outside
        //         that range.

        // TODO 5: Give Student its main constructor, taking a full
        //         name, a year of study, and a GPA. Immediately after
        //         its parameter list, chain to Person's own
        //         constructor using the colon-base syntax, passing the
        //         full name through unchanged — this is what actually
        //         builds the "Person part" of the object, and it must
        //         run before anything else. Inside the constructor's
        //         own body, assign year of study and GPA THROUGH their
        //         properties (TODO 3 and TODO 4), never directly to
        //         their private fields. Increment the static
        //         total-count field from TODO 2 inside this
        //         constructor.

        // TODO 6: Give Student a SECOND constructor, taking only a
        //         full name and a year of study — no GPA parameter.
        //         Its body should be empty; instead, chain to the
        //         TODO 5 constructor using the colon-this syntax
        //         (chaining within the SAME class, not to Person —
        //         that already happened inside TODO 5), passing the
        //         full name and year of study through and supplying
        //         0.0 as the GPA value.

        // TODO 7: Give Student a PUBLIC method that updates GPA given
        //         just a new decimal value, with no logged reason. Its
        //         body should call the OTHER GPA-update method
        //         described in TODO 8, supplying a generic placeholder
        //         reason, so the real update logic exists in exactly
        //         one place.

        // TODO 8: Give Student a SECOND public method with the exact
        //         same name as TODO 7's method, but taking two things:
        //         a new decimal GPA value AND a text reason. This
        //         version actually assigns the new GPA (through the
        //         property) and prints one line naming the student,
        //         their new GPA, and the reason given. This is method
        //         OVERLOADING — the compiler picks whichever version
        //         matches how it's called.

        // TODO 9: Give Student two special static methods with the
        //         reserved name pattern for the greater-than and
        //         less-than symbols, each taking two Student objects
        //         and returning true or false based on comparing their
        //         GPA values.

        // TODO 10: Give Student two more special static methods, for
        //          the equal-to and not-equal-to symbols — these MUST
        //          be written together, C# will not compile one
        //          without the other. The equal-to version should
        //          first check whether both inputs are literally the
        //          same object in memory, then check whether either
        //          input has no object at all, and only then compare
        //          both the full name AND the GPA of the two Student
        //          objects for equality. The not-equal-to version
        //          should simply return the exact opposite of the
        //          equal-to version's result.

        // TODO 11: Give Student a PUBLIC method named PrintSummary
        //          that takes one true/false parameter (defaulting to
        //          true) controlling whether honor status would be
        //          included in a fuller report (today's version can
        //          keep this simple — no honor-status classification
        //          needed yet, that's carried over from Session 08/09
        //          if you want to restore it). Inside, FIRST call the
        //          inherited Person method from TODO 1 using the
        //          colon-base syntax to print the object's own name,
        //          THEN print a second line with the object's own year
        //          of study and GPA. Notice this method calls the
        //          PARENT's method explicitly rather than repeating
        //          what it already does.

        // TODO 12: Give Student a PUBLIC STATIC method that returns
        //          the current value of the TODO 2 total-count field.

        // ===== Instructor class, now derived from Person =====

        // TODO 13: Define a class named Instructor that DERIVES from
        //          Person, same as Student. Give it one private
        //          whole-number field for years of experience — do
        //          NOT redeclare a full-name field; it's inherited.
        //          Also add a public property (using the shortcut
        //          get/set form, no explicit backing field needed)
        //          that stores the NAME of whichever course this
        //          instructor is currently loosely associated with —
        //          a plain piece of text, not a reference to an actual
        //          Course object, matching Session 09's Association
        //          pattern exactly, unchanged.

        // TODO 14: Give Instructor a public property for years of
        //          experience. Its setter should only accept whole
        //          numbers zero or greater, rejecting negative values.

        // TODO 15: Give Instructor a constructor taking a full name and
        //          years of experience. Chain to Person's constructor
        //          using the colon-base syntax, passing the full name
        //          through. Inside the constructor's own body, assign
        //          years of experience THROUGH its property (TODO 14).

        // TODO 16: Give Instructor a PUBLIC method named PrintSummary,
        //          taking no parameters. Inside, FIRST call the
        //          inherited Person method from TODO 1 using the
        //          colon-base syntax, THEN print a second line with
        //          the object's own years of experience.

        static void Main(string[] args)
        {

            //Person person = new Person("Hamdy");
            //Student student = new Student("Hamdy" , 4 , 3.5);
            //student.PrintSummary();

            //List<Person> everyone = new List<Person>();
            //everyone.Add(student);
            //everyone.Add(new Instructor("Khaled", 10));

            //foreach (var p in everyone)
            //{
            //    p.PrintBasicInfo();
            //}

            //Person person = everyone[0];
            //Console.WriteLine(person.FullName);
            //Console.WriteLine(((Student)person).Gpa);   
            


            //Student student1 = new Person("Hamdy");

            List<Admin> admins = new List<Admin>();
            Admin admin1 = new Admin("Ahmed", 2);
            admins.Add(admin1);
        foreach (var admin in admins)
        {
            admin.PrintSummary();
        }
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
                            string targetInstructor = Console.ReadLine();

                            Instructor foundInstructor = null;
                            foreach (var instructor in instructors)
                            {
                                if (instructor.FullName == targetInstructor)
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
                //student.fullName , person.fullName; // error

                // TODO 17: Create a single list capable of holding Person
                //          objects — NOT a list of Student and a separate
                //          list of Instructor. Add at least one Student
                //          object (built with either of TODO 5/6's
                //          constructors) and at least one Instructor
                //          object to this ONE shared list.

                // TODO 18: Loop over every object in the shared list from
                //          TODO 17 and call each one's inherited
                //          Person-level print method from TODO 1 — notice
                //          this works identically for every object in the
                //          list, regardless of whether it's really a
                //          Student or an Instructor underneath, because
                //          that method is guaranteed to exist on anything
                //          stored as a Person.

                // TODO 19: Print the total number of Students ever
                //          created, using TODO 12's static method, called
                //          on the Student class name itself.

                Console.ReadLine();  // Wait for user to press Enter before closing
    }
    }

    #region 📋 Full TODO Checklist (collapse this region for a quick overview)
    // 1.  Define Person: protected fullName field, public FullName property, constructor, PrintBasicInfo method
    // 2.  Define Student : Person — private fields (year, GPA) + a static total-count field, no fullName here
    // 3.  Add the YearOfStudy property (validates 1-4)
    // 4.  Add the Gpa property (validates 0.0-4.0)
    // 5.  Add Student's main constructor — chains to `base(fullName)`, assigns year/GPA through properties, increments the static count
    // 6.  Add Student's second constructor — chains to `this(...)` (the main constructor above), defaulting GPA to 0.0
    // 7.  Add the 1-parameter UpdateGpa overload (delegates to the 2-parameter version)
    // 8.  Add the 2-parameter UpdateGpa overload (does the real work)
    // 9.  Add the `>` and `<` operator overloads, comparing by GPA
    // 10. Add the `==` and `!=` operator overloads together (name + GPA comparison, with guards)
    // 11. Add Student's PrintSummary — calls `base.PrintBasicInfo()` first, then its own year/GPA line
    // 12. Add the public static method that returns the Student total-count field
    // 13. Define Instructor : Person — private years-of-experience field + an AssignedCourseName auto-property, no fullName here
    // 14. Add the YearsOfExperience property (rejects negative values)
    // 15. Add Instructor's constructor — chains to `base(fullName)`, assigns years of experience through its property
    // 16. Add Instructor's PrintSummary — calls `base.PrintBasicInfo()` first, then its own years-of-experience line
    // 17. Build ONE List<Person> holding both a Student and an Instructor
    // 18. Loop the shared list, calling the inherited Person-level print method on every object
    // 19. Print the total Student count, called on the Student class name
    #endregion
}
