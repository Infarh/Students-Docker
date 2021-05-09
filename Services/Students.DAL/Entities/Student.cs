using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Students.DAL.Entities.Base;

namespace Students.DAL.Entities
{
    [Index(nameof(Name), nameof(LastName), nameof(Patronymic), nameof(Birthday), IsUnique = true)]
    public class Student : NamedEntity
    {
        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public DateTime Birthday { get; set; }

        public virtual Group Group { get; set; }
    }
}
