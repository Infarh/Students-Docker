using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Students.DAL.Entities;

namespace Students.DAL
{
    public class DbInitializer
    {
        private readonly StudentsDB _db;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(StudentsDB db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            _Logger.LogInformation("Инициализация базы данных...");

            var db = _db.Database;

            if (db.EnsureCreated() && !db.GetPendingMigrations().Any())
                _Logger.LogInformation("БД создана {0} c", timer.Elapsed.TotalSeconds);
            else
            {
                _Logger.LogInformation("Инициализация БД не требуется {0} c", timer.Elapsed.TotalSeconds);
                return;
            }

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Миграция БД");
                db.Migrate();
                _Logger.LogInformation("Миграция БД выполнена за {0} c", timer.Elapsed.TotalSeconds);
            }
            else
                _Logger.LogInformation("Миграция БД не требуется");

            if (_db.Students.Any())
            {
                _Logger.LogInformation("Инициализация БД не требуется. {0} с", timer.Elapsed.TotalSeconds);
                return;
            }

            for (var (i, student_n) = (1, 1); i <= 10; i++)
            {
                var group = new StudentsGroup
                {
                    Name = $"Группа-{i}",
                };
                for (var j = 1; j <= 4; j++, student_n++)
                {
                    var runway = new Student
                    {
                        Name = $"Имя-{student_n}",
                        LastName = $"Фамилия-{student_n}",
                        Patronymic = $"Отчество-{student_n}",
                        Birthday = DateTime.Now.AddYears(-22 + j).AddMonths(i - j).AddDays(2 * j + i)
                    };
                    group.Students.Add(runway);
                }

                _db.Groups.Add(group);
            }

            _db.SaveChanges();

            _Logger.LogInformation("Инициализация БД завершена {0} c", timer.Elapsed.TotalSeconds);
        }
    }
}
