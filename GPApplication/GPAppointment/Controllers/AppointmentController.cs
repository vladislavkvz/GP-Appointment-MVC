namespace GPAppointment.Controllers
{
    using DataAccess.Entities;
    using Filter;
    using DataAccess.Repositories;
    using Models;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;
    using ViewModels.AppointmentVMs;
    using System.Collections.Generic;
    
    [PatientAuthentication]
    public class AppointmentController : BaseController<Appointment,AppointmentIndexVM,AppointmentEditVM,AppointmentFilterVM>
    {
        protected override BaseRepo<Appointment> CreateRepository()
        {
            return new AppointmentRepo();
        }

        protected override AppointmentIndexVM CreateBaseIVM()
        {
            return new AppointmentIndexVM();
        }

        protected override AppointmentEditVM CreateBaseEVM()
        {
            return new AppointmentEditVM();
        }

        protected override List<Appointment> PopilateaList(int currentPage, int pageSize)
        {
               AppointmentRepo repo = new AppointmentRepo();
            return repo.GetAll(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id, currentPage, pageSize).ToList();
        }
        protected override int Count()
        {
               AppointmentRepo repo = new AppointmentRepo();
            return repo.Count(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id);
        }

        protected override ActionResult Redirect()
        {
            return RedirectToAction("GetAll");
        }

        protected override void PopulateViewModel(AppointmentEditVM model, Appointment entity)
        {
            model.Id = entity.Id;
            model.ArrangeTime = entity.ArrangeTime;
            model.EndArrangeTime = model.ArrangeTime.AddHours(1);
            model.Status = entity.Status;
            model.Seen = entity.Seen;
            model.Patient = entity.Patient;
            model.Doctor = entity.Doctor;
        }

        protected override void PopulateEntity(Appointment entity, AppointmentEditVM model)
        {
            UserRepo repo = new UserRepo();
            repo.DisableProxy();
            entity.Id = model.Id;
            entity.ArrangeTime = model.ArrangeTime;
            entity.Seen = false;
            entity.Status = DataAccess.Tools.Enums.Status.Unseen;
            entity.Patient = repo.GetById(model.Patient.Id);
            entity.Doctor = repo.GetById(model.Doctor.Id);
        }

        [AdminAuthentiation]
        public ActionResult GetAll()
        {
            AppointmentIndexVM model = new AppointmentIndexVM();
            AppointmentRepo repo = new AppointmentRepo();
            model.Pager = new PagerVM();
            model.Filter = new AppointmentFilterVM();
            TryUpdateModel(model);
            model.Filter.Prefix = "Filter.";
            string prefix = "Pager.";
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            int resultCount = repo.Count(model.Filter.BuildFilter());
            model.Pager = new PagerVM(resultCount, model.Pager.CurrentPage, model.Pager.PageSize, prefix, action, controller);
            model.Filter.ParentPager = model.Pager;
            model.Items = repo.GetAll(model.Filter.BuildFilter(), model.Pager.CurrentPage, model.Pager.PageSize.Value).ToList();
            return View(model);
        }
    }
}