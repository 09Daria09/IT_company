using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace IT_company.Model
{
    public class CompanyContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=6E7PN2T;Database=ITCompanyDB;Integrated Security=True;TrustServerCertificate=true")
                .UseLazyLoadingProxies();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Position>()
                .HasKey(p => p.PositionId);

            modelBuilder.Entity<Position>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId);
        }
        public void InitializeDb()
        {
            if (this.Positions.Any() || this.Employees.Any())
            {
                return;
            }

            var positions = new List<Position>
{
    new Position { Title = "Разработчик" },
    new Position { Title = "Тестировщик" },
    new Position { Title = "Менеджер проектов" },
    new Position { Title = "Дизайнер" },
    new Position { Title = "Аналитик" },
    new Position { Title = "HR-менеджер" },
    new Position { Title = "Системный администратор" }
};
            this.Positions.AddRange(positions);

            var employees = new List<Employee>
{
    new Employee { Name = "Алексей", Surname = "Смирнов", Position = positions[0] },
    new Employee { Name = "Марина", Surname = "Иванова", Position = positions[1] },
    new Employee { Name = "Дмитрий", Surname = "Петров", Position = positions[2] },
    new Employee { Name = "Елена", Surname = "Сидорова", Position = positions[3] },
    new Employee { Name = "Игорь", Surname = "Кузнецов", Position = positions[4] },
    new Employee { Name = "Ольга", Surname = "Васильева", Position = positions[5] },
    new Employee { Name = "Сергей", Surname = "Морозов", Position = positions[6] },
    new Employee { Name = "Наталья", Surname = "Соколова", Position = positions[0] },
    new Employee { Name = "Виктор", Surname = "Лебедев", Position = positions[1] },
    new Employee { Name = "Ирина", Surname = "Козлова", Position = positions[2] },
    new Employee { Name = "Павел", Surname = "Новиков", Position = positions[3] },
    new Employee { Name = "Анна", Surname = "Зайцева", Position = positions[4] },
    new Employee { Name = "Кирилл", Surname = "Белов", Position = positions[5] },
    new Employee { Name = "Олег", Surname = "Медведев", Position = positions[6] },
    new Employee { Name = "Татьяна", Surname = "Ермакова", Position = positions[0] },
    new Employee { Name = "Михаил", Surname = "Павлов", Position = positions[1] },
    new Employee { Name = "Ксения", Surname = "Голубева", Position = positions[2] },
    new Employee { Name = "Артем", Surname = "Виноградов", Position = positions[3] },
    new Employee { Name = "Людмила", Surname = "Богданова", Position = positions[4] },
    new Employee { Name = "Антон", Surname = "Федоров", Position = positions[5] },
    new Employee { Name = "Светлана", Surname = "Михайлова", Position = positions[6] }
};
            this.Employees.AddRange(employees);

            this.SaveChanges();

        }

    }
}
