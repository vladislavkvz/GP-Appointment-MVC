namespace GPAppointment.Controllers
{
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using ViewModels;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Filter;

    [UserAuthentiation]
    public abstract class BaseController<T, IVM, EVM,FilterVM> : Controller
       where T : BaseEntity, new()
       where IVM : BaseIndexVM<T, FilterVM>
       where EVM : BaseEditVM
        where FilterVM : BaseFilterVM<T>, new()
    {
        public BaseController()
        {
            repo = CreateRepository();
        }

        private BaseRepo<T> repo;

        protected abstract BaseRepo<T> CreateRepository();
        protected abstract IVM CreateBaseIVM();
        protected abstract EVM CreateBaseEVM();

        protected virtual void PopulateViewModel(EVM model, T entity)
        {
        }

        protected virtual void PopulateEntity(T entity, EVM model)
        {
        }

        protected virtual List<T> PopilateaList(int currentPage, int pageSize)
        {
            return repo.GetAll(null, currentPage, pageSize).ToList();
        }
        protected virtual int Count()
        {
            return repo.Count();
        }

        protected virtual ActionResult Redirect()
        {
            return RedirectToAction("Index");
        }

        public virtual ActionResult Index()
        {
            IVM model = CreateBaseIVM();
            model.Pager = new PagerVM();
            TryUpdateModel(model);
            string prefix = "Pager.";
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            int resultCount = Count();
            model.Pager = new PagerVM(resultCount, model.Pager.CurrentPage, model.Pager.PageSize, prefix, action, controller);
            model.Items = PopilateaList(model.Pager.CurrentPage, model.Pager.PageSize.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            EVM model = CreateBaseEVM();
            T entity = (id == null || id <= 0) ? new T() : repo.GetById(id.Value);
            PopulateViewModel(model, entity);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(FormCollection collection)
        {
            EVM model = CreateBaseEVM();
            TryUpdateModel(model);
            if (ModelState.IsValid)
            {
                T entity = new T();
                PopulateEntity(entity, model);
                repo.Save(entity);
                return Redirect();
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            T entity = repo.GetById(id);
            DeleteFilter(entity.Id);
            repo.Delete(entity);
            return Redirect();
        }

        protected virtual void DeleteFilter(int id)
        {
        }
    }
}