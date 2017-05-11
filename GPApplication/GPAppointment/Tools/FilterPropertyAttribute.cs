namespace GPAppointment.Tools
{
    using System;

    public class FilterPropertyAttribute : Attribute
    {
        public string DisplayName { get; set; }
    }
}