using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentPortalConsole
{
    public class StudentPortalContext : DbContext
    {
        public DbSet<Student> Students { get; set; } // Represents the Students table
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=ITI_StudentPortal;Trusted_Connection=True;TrustServerCertificate=True"
                )
                .LogTo(Console.WriteLine , LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Student>()
                .Property(s => s.FullName)

                .HasMaxLength(110);
            modelBuilder.Entity<Course>()
                .Property(s => s.CourseName)

                .HasMaxLength(120);
            modelBuilder.Entity<Course>()
                .HasOne(i => i.Instructor)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        

    }
}
