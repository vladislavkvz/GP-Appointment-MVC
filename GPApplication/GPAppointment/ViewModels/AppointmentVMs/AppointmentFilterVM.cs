namespace GPAppointment.ViewModels.AppointmentVMs
{
    using DataAccess.Entities;
    using Tools;
    using System;
    using System.Linq.Expressions;
  
    public class AppointmentFilterVM : BaseFilterVM<Appointment>
    {
        [FilterProperty(DisplayName = "Name")]
        public string Name{ get; set; } 
        
        public override Expression<Func<Appointment,Boolean>> BuildFilter()
        {
            return (u => (String.IsNullOrEmpty(Name) || u.Patient.FirstName.Contains(Name)) ||
                            (String.IsNullOrEmpty(Name) || u.Patient.LastName.Contains(Name)) ||
                            (String.IsNullOrEmpty(Name) || u.Doctor.FirstName.Contains(Name)) ||
                            (String.IsNullOrEmpty(Name) || u.Doctor.LastName.Contains(Name)));
        }
    }
}