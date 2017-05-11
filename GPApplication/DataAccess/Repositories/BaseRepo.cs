namespace DataAccess.Repositories
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class BaseRepo<T> where T : BaseEntity, new()
    {
        public readonly GPAppointmentDbContext<T> DbContext;

        public BaseRepo()
        {
            DbContext = new GPAppointmentDbContext<T>();
        }

        private void Insert(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
            this.DbContext.SaveChanges();
        }

        private void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
            this.DbContext.SaveChanges();
        }

        public T GetById(object id)
        {
            return this.DbContext.Items.Find(id);
        }

        public IQueryable<T> GetAll(Expression<Func<T, Boolean>> expr = null, int page = 0, int pageSize = 0)
        {
            IQueryable<T> result = DbContext.Items;

            if (expr != null)
                result = result.Where(expr);

            if (page > 0 && pageSize > 0)
                result = result.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);

            return result;
        }

        public int Count(Expression<Func<T,Boolean>> expr = null)
        {
            return expr == null ? this.DbContext.Items.Count() : this.DbContext.Items.Count(expr);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
            this.DbContext.SaveChanges();
        }

        public void Save(T entity)
        {
            if (entity.Id > 0)
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
        }

        public void DisableProxy()
        {
            this.DbContext.Configuration.ProxyCreationEnabled = false;
        }

        private void ChangeState(T entity, EntityState state)
        {
            var dbEntry = this.DbContext.Entry(entity);
            dbEntry.State = state;
        }
    }
}