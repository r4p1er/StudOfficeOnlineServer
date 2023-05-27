using Microsoft.EntityFrameworkCore;

namespace StudOfficeOnlineServer.Models
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Announcement> Announcemenments { get; set; }
        public DbSet<Retake> Retake { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentDocument> StudentDocuments { get; set; }
        public DbSet<ConsultationTicket> Consultations { get; set; }

        private readonly IConfiguration _configuration;
        public DBContext(DbContextOptions<DBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(x => x.Admin)
                                       .WithOne(x => x.User)
                                       .HasForeignKey<Admin>(x => x.UserId);

            modelBuilder.Entity<User>().HasOne(x => x.Student)
                                       .WithOne(x => x.User)
                                       .HasForeignKey<Student>(x => x.UserId);

            modelBuilder.Entity<User>().HasOne(x => x.Teacher)
                                       .WithOne(x => x.User)
                                       .HasForeignKey<Teacher>(x => x.UserId);

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = _configuration["AuthOptions:AdminEmail"]!, FirstName = "admin", MiddleName = "admin", LastName = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword(_configuration["AuthOptions:AdminPassword"] + _configuration["AuthOptions:PEPPER"]), Role = "Admin", AdminId = 1 });
            modelBuilder.Entity<Admin>().HasData(new Admin { Id = 1, UserId = 1 });

            modelBuilder.Entity<User>().HasData(new User { Id = 2, Email = "teacher@gmail.com", FirstName = "Аркадий", MiddleName = "Иванович", LastName = "Перегуда", PasswordHash = BCrypt.Net.BCrypt.HashPassword(_configuration["AuthOptions:AdminPassword"] + _configuration["AuthOptions:PEPPER"]), Role = "Teacher", TeacherId = 1 });
            modelBuilder.Entity<Teacher>().HasData(new Teacher { Id = 1, UserId = 2 });

            modelBuilder.Entity<User>().HasData(new User { Id = 3, Email = "student@gmail.com", FirstName = "Александр", MiddleName = "Михайлович", LastName = "Цыганок", PasswordHash = BCrypt.Net.BCrypt.HashPassword(_configuration["AuthOptions:AdminPassword"] + _configuration["AuthOptions:PEPPER"]), Role = "Student", StudentId = 1 });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 1, Citizenship = "РФ", EducationBase = "Бюджет", EducationForm = "Очная", EducationStart = DateTime.UtcNow, EducationEnd = DateTime.UtcNow.AddYears(2), FacultyId = 1, GroupId = 1, OrderNumber = "229/7-4", StudentCard = "СТО/1132-21", OrderDate = DateTime.UtcNow, UserId = 3, Course = 2 });

            modelBuilder.Entity<Group>().HasData(new Group { Id = 1, Name = "ИВТ-Б21" });
            modelBuilder.Entity<Faculty>().HasData(new Faculty { Id = 1, Name = "ОИКС" });
        }
    }
}
