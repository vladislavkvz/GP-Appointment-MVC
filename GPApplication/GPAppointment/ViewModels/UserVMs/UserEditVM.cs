namespace GPAppointment.ViewModels.UserVMs
{
    using DataAccess.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ValidationAttributes;
    using static DataAccess.Tools.Enums;

    public class UserEditVM:BaseEditVM
    {
        [Required(ErrorMessage = "Enter Username")]
        [MinLength(4, ErrorMessage = "Min Length is 4 symbol")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [MinLength(6,ErrorMessage ="Min Length is 6 symbol")]
        public string Password { get; set; }

        [IsEqual("Password")]
        [MinLength(6, ErrorMessage = "Min Length is 6 symbol")]
        public string VerifyPassword { get; set; }

        [Required(ErrorMessage = "Please enter your First Name")]
        [MinLength(2, ErrorMessage = "Min Length is 2 symbol")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        [MinLength(2, ErrorMessage = "Min Length is 2 symbol")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        public Position Position { get; set; }

        public bool IsAdmin { get; set; }
        
        public virtual ICollection<Appointment> AppointmentsPatient { get; set; }
        
        public virtual ICollection<Appointment> AppointmentsDoctor { get; set; }
    }
}