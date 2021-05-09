using Microsoft.EntityFrameworkCore;
using Students.DAL.Entities;

namespace Students.DAL
{
    public class StudentsDB : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<StudentsGroup> Groups { get; set; }

        public StudentsDB(DbContextOptions<StudentsDB> options) : base(options) { }
    }
}
