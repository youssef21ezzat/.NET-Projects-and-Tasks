
// StudentPortalWeb — SESSION PROJECT (Style Guide Rule 20/34/35/39/40)
// ITI Summer Training | Web Development Using .NET | Morning Group
// Session 16 — Routing: Custom Routes, Constraints, Attribute Routing
//
// THIS IS YESTERDAY'S PROJECT, NOT A NEW ONE (Rule 40). It is the exact
// same StudentPortalWeb solution you finished Session 15 with — same
// context, same connection string, same injected HomeController, same
// [START]/[END] middleware you typed yourselves. Everything Session 15
// taught is already here as real, working code. Nothing gets re-typed.
//
// THIS PROJECT IS DAY-READY (Rule 39). Press F5 right now, before a
// single TODO is done, and the Session 15 home page loads with the same
// students on it. Today we do not change that page at all. We change
// what URLs the application is willing to answer.
//
// TODAY'S TODOs, AND THE ORDER THE LECTURE VISITS THEM:
//
//   Program.cs (this file)              TODO 2     Block 2 — your first
//                                                  custom routes
//   Program.cs (this file)              TODO 3     Block 3 — constraints
//                                                  on those routes
//   Constraints/HonourBandConstraint.cs TODO 5     Block 4 — a constraint
//                                                  you write yourself
//   Program.cs (this file)              TODO 1, 4  Block 4 — register it,
//                                                  then route with it
//   Controllers/StudentsController.cs   TODO 6     Block 5 — an action
//                                                  that carries its own
//                                                  address
//
// ⚠️ WHY TODO 1 IS NUMBERED 1 BUT TAUGHT FOURTH (Rule 40):
//   TODO 1 registers your custom constraint into the routing options.
//   Registering ANYTHING into the service container can only happen
//   BEFORE builder.Build() runs. TODO 2, 3 and 4 add routes, which can
//   only happen AFTER it. The dividing line between them is a single
//   line of code that cannot move, so TODO 1 is physically stuck above
//   TODOs that are taught before it. Rule 40's stated exception: a TODO
//   forced out of teaching order by the framework takes the number its
//   PHYSICAL position demands, and says so. It pairs with TODO 4 and
//   TODO 5 — all three are Block 4, and none of them works alone.
//   Same shape as Session 15's TODO 3 / TODO 8 split, and Session 14's
//   LogTo call.
//
// ⚠️ STILL NOT THE MIGRATION OWNER. The Session 14 console project owns
//   this database's migration history. Do not run Add-Migration or
//   Update-Database from here. Today changes ZERO schema — routing lives
//   entirely in code and touches no table.
//
// For the full, correct, runnable version (do NOT peek until you've
// tried it yourself, or you're checking your own work), see:
// ../StudentPortalWeb_Complete/Program.cs
// =====================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentPortalWeb.Constraints;
using StudentPortalWeb.Models;
using System;

namespace StudentPortalWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // =========================================================
            // PHASE ONE — WHAT CAN THIS APP DO?
            // Everything above builder.Build() registers capabilities
            // into the DI container. Nothing here handles a request yet.
            // =========================================================
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // TODO 1: ⚠️ Numbered 1 because it sits here physically, but
            //         TAUGHT FOURTH, in Block 4 — leave it blank until
            //         then. See the note at the top of this file for why
            //         it cannot move down next to the route it serves.
            //         Teach the routing system the nickname of the
            //         constraint class you will write in TODO 5. Call the
            //         routing configuration method on builder.Services,
            //         give it a lambda that receives an options object,
            //         and on that object's map of constraint nicknames,
            //         add one entry: the short lowercase word you want to
            //         type inside route patterns, paired with the type of
            //         your constraint class. Use the typeof operator for
            //         the second half — you are handing over the class
            //         itself, not an instance of it. The framework will
            //         create instances when it needs them, which is the
            //         same idea you met in Session 15.
            //         ⚠️ The nickname you choose here is the exact word
            //         TODO 4 must type after the colon. If the two ever
            //         disagree, the app throws at startup naming the
            //         constraint it could not find.

            builder.Services.AddDbContext<StudentPortalContext>(options =>
            {
                options.UseSqlServer("Data Source=localhost;Initial Catalog=ITI_StudentPortal;Integrated Security=True;Encrypt=True;TrustServerCertificate=True")
                .LogTo(Console.WriteLine , LogLevel.Information)
                .EnableSensitiveDataLogging();
            });


            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("honourBand", typeof(HonourBandConstraint));
                options.ConstraintMap.Add("intakecode", typeof(IntakeCodeConstraint));
            });

            var app = builder.Build();
            // ↑↑↑ THE DIVIDING LINE. Above: what exists. Below: what runs.

            // =========================================================
            // PHASE TWO — HOW IS A REQUEST HANDLED?
            // Every app.Use... call below adds one checkpoint to the
            // hallway a request walks down, in the order written.
            // =========================================================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Session 15, unchanged. Today this becomes a measuring
            // instrument: it prints the path of every request BEFORE
            // routing decides anything, so you can see the difference
            // between "the route rejected it" and "the action ran and
            // found nothing".
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[START] Request path : {context.Request.Path}");
                await next.Invoke();
                Console.WriteLine($"[END] Request path : {context.Request.Path}");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();

            // TODO 2: Add two custom routes here, ABOVE the default route
            //         that is already below you. Both are added with the
            //         same controller-route mapping method the default
            //         route uses, and both take three named arguments: a
            //         name, a pattern, and a defaults object.
            //         The first route: name it after the students list,
            //         give it the single literal segment students with no
            //         parameters at all, and in its defaults object state
            //         which controller and which action should answer it
            //         — the students controller and its listing action,
            //         written as plain strings without the word
            //         "Controller" on the end.
            //         The second route: name it after the student detail
            //         page, give it the literal segment students followed
            //         by one parameter segment holding the student's
            //         identifier, and default it to the same controller
            //         and the detail action.
            //         Notice what is NOT in either pattern: any mention
            //         of the controller or the action. In these routes
            //         the URL no longer describes your class names — the
            //         defaults object does, privately. That separation is
            //         the entire point of Block 2.

            //app.MapControllerRoute(
            //    name: "studentsList",
            //    pattern: "students",
            //    defaults: new { controller = "Students", action = "Index" }
            //    );

            // Is it acceptable for two different URLs to reach the same action? Yes, it can be useful to provide a simpler alias for users (like /roster), provided we aren't creating confusing SEO duplicate content.
            app.MapControllerRoute(
                name: "roster",
                pattern: "roster",
                defaults: new { controller = "Students", action = "Index" }
                );

            // Is your MAX_YEAR itself accepted, or rejected? My MAX_YEAR is 3. Since the range constraint is inclusive, passing 3 is accepted.
            app.MapControllerRoute(
                name: "studentsTop",
                pattern: "students/top/{count:int:range(1,3)}",
                defaults: new { controller = "Students", action = "Top" }
                );

            app.MapControllerRoute(
                name: "studentsIntake",
                pattern: "students/intake/{code:intakecode}",
                defaults: new { controller = "Students", action = "Intake" }
                );

            app.MapControllerRoute(
                name: "studentsDetails",
                pattern: "students/{id:int}",
                defaults: new { controller = "Students", action = "Details" }
                );

            //app.MapControllerRoute(
            //    name: "studentsByYear",
            //    pattern: "students/year/{year:int:range(1,4)}",
            //    defaults: new { controller = "Students", action = "ByYear" }
            //    );

            //app.MapControllerRoute(
            //    name: "studentsHonours",
            //    pattern: "students/honours/{band:honourBand}",
            //    defaults: new { controller = "Students", action = "Honours" }
            //    );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.MapControllers();

            // TODO 3: Two edits, both about refusing bad input at the
            //         door rather than crashing behind it.
            //         First, go back to TODO 2's detail route and attach
            //         the built-in whole-number constraint to its
            //         parameter, using a colon between the parameter name
            //         and the constraint name inside the same braces.
            //         Second, add a THIRD route above the default one:
            //         name it after the by-year listing, and give it the
            //         literal segment students, then the literal segment
            //         year, then one parameter segment for the academic
            //         year. Constrain that parameter twice in a row — to
            //         whole numbers, and to the inclusive numeric range
            //         one through four — by chaining both constraint
            //         names after the parameter with colons. Default it
            //         to the students controller and the by-year action.
            //         Predict before you run it: what should the browser
            //         show for a year of 7, and should the action run at
            //         all?

            // TODO 4: (Block 4 — do TODO 5 and TODO 1 first.) Add a
            //         fourth route above the default one. Pattern: the
            //         literal segment students, then the literal segment
            //         honours, then one parameter segment for the class
            //         band. Constrain that parameter with the nickname
            //         you registered in TODO 1 — the same colon syntax as
            //         the built-in constraints, because to the routing
            //         system there is no difference between the ones
            //         Microsoft wrote and the one you wrote. Default it
            //         to the students controller and the honours action.

            app.Run();
        }
    }
}

#region 📋 Full TODO Checklist
// ---------------------------------------------------------------------
// Program.cs — Phase One (before builder.Build())
//   TODO 1: Register your constraint's nickname in the constraint map
//           [Block 4 — sits first only because it must run before Build]
//
// Program.cs — Phase Two (after builder.Build())
//   TODO 2: Custom routes for the students list and the student detail   [Block 2]
//   TODO 3: Constrain the detail id to integers; add the by-year route   [Block 3]
//   TODO 4: Add the honours route using your own constraint's nickname   [Block 4]
//
// Constraints/HonourBandConstraint.cs
//   TODO 5: Implement Match so only the three real band names pass       [Block 4]
//
// Controllers/StudentsController.cs
//   TODO 6: Give the search action its own address, and read the query   [Block 5]
// ---------------------------------------------------------------------
#endregion
