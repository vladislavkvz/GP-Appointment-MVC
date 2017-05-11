namespace GPAppointment.ViewModels
{
    using DataAccess.Entities;
    using System;
    using System.Linq.Expressions;

    public abstract class BaseFilterVM<T>
        where T : BaseEntity
    {
        public string Prefix { get; set; }
        public PagerVM ParentPager { get; set; }
        public abstract Expression<Func<T, Boolean>> BuildFilter();
    }
}