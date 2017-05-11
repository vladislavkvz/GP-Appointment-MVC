namespace DataAccess
{
    using System.Data.Entity;

    public class GPAppointmentDbContext<T> : DbContext where T : class
    {
        public GPAppointmentDbContext()
            : base("GPAppointmentDbContext")
        {

        }

        public DbSet<T> Items { get; set; }
    }
}
