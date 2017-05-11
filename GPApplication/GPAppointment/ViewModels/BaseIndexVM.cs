namespace GPAppointment.ViewModels
{
    using DataAccess.Entities;
    using System.Collections.Generic;

    public class BaseIndexVM<T, F>
        where T : BaseEntity
        where F : BaseFilterVM<T>
    {
        public IList<T> Items { get; set; }

        public PagerVM Pager { get; set; }

        public F Filter { get; set; }
    }
}