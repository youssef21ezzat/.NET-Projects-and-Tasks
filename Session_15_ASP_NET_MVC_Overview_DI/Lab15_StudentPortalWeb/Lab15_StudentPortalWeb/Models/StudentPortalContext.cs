using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab15_StudentPortalWeb


{
    public class StudentPortalContext : DbContext
    {
        public DbSet<Student> Students { get; set; } // Represents the Students table
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        public StudentPortalContext(DbContextOptions<StudentPortalContext> options) : base(options)
        {
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
