namespace DataAccess.Entities
{
    using System;
    using static Tools.Enums;

    public class Appointment : BaseEntity
    {
        public Status Status { get; set; }

        public DateTime ArrangeTime { get; set; }
        
        public virtual User Doctor { get; set; }

        public virtual User Patient { get; set; }

        public bool Seen { get; set; }
    }
}
