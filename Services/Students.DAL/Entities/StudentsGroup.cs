using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Students.DAL.Entities.Base;

namespace Students.DAL.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class StudentsGroup : NamedEntity
    {
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
