namespace GPAppointment.ViewModels.AppointmentVMs
{
    using System;
    using ViewModels;
    using System.ComponentModel.DataAnnotations;
    using DataAccess.Entities;
    using static DataAccess.Tools.Enums;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;

    public class AppointmentEditVM:BaseEditVM
    {
        public Status Status { get; set; }

        [Required(ErrorMessage = "Choose start time for an arrangement")]

        public DateTime ArrangeTime { get; set; }

        public DateTime EndArrangeTime { get; set; }

        public User Doctor { get; set; }

        public int SelectedDoctorId { get; set; }
        public string SelectedDoctorName { get; set; }
        public List<User> Doctors;
        public IEnumerable<SelectListItem> DoctorsList
        {
            get
            {
                var allDoctors = Doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.FirstName + " - " + d.LastName
                });
                return allDoctors;

            }
        }

        public User Patient { get; set; }

        public bool Seen { get; set; }
    }
}