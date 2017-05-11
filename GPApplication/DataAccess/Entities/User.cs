namespace DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Tools.Enums;

    public class User : BaseEntity
    {
        public User()
        {
            this.AppointmentsPatient = new HashSet<Appointment>();
            this.AppointmentsDoctor = new HashSet<Appointment>();
        }

        public string Username { get; set; }

        public string Password { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public Position Position { get; set; }

        public bool IsAdmin { get; set; }

        [InverseProperty("Patient")]
        public virtual ICollection<Appointment> AppointmentsPatient { get; set; }
        
        [InverseProperty("Doctor")]
        public virtual ICollection<Appointment> AppointmentsDoctor { get; set; }
    }
}
