namespace GPAppointment
{
    using DataAccess.Entities;
    using System.Data.Entity;

    public class AppDbContext: DbContext 
    {
        public AppDbContext()
            : base("GPAppointmentDbContext")
        {

        }

        public DbSet<Appointment> Appointment { get; set; }

        public DbSet<User> User { get; set; }
    }
}
