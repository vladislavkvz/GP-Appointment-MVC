namespace GPAppointment.Controllers
{
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using Models;
    using ViewModels.AppointmentVMs;
    using System.Linq;
    using System.Web.Mvc;
    using static DataAccess.Tools.Enums;
    using ViewModels;
    using System.Collections.Generic;
    using Filter;

    [DoctorAuthentication]
    public class DoctorController : BaseController<Appointment,AppointmentIndexVM,AppointmentEditVM,AppointmentFilterVM>
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

            return repo.GetAll(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Unseen, currentPage, pageSize).ToList();
        }

        protected override int Count()
        {
               AppointmentRepo repo = new AppointmentRepo();
            return repo.Count(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Unseen);
        }

        public ActionResult Approve(int id)
        {
            Appointment appointment = new Appointment();
            AppointmentRepo repo = new AppointmentRepo();
            appointment = repo.GetById(id);
            appointment.Status = Status.Approved;
            repo.Save(appointment);
            return RedirectToAction("Index");
        }

        public ActionResult Decline(int id)
        {
            Appointment appointment = new Appointment();
            AppointmentRepo repo = new AppointmentRepo();
            appointment = repo.GetById(id);
            appointment.Status = Status.Decline;
            repo.Save(appointment);
            return RedirectToAction("Index");
        }

        public ActionResult ReviewApproved()
        {
            AppointmentIndexVM model = new AppointmentIndexVM();
            model.Pager = new PagerVM();
            TryUpdateModel(model);
            AppointmentRepo repo = new AppointmentRepo();
            string prefix = "Pager.";
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            int resultCount = repo.Count(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Approved);
            model.Pager = new PagerVM(resultCount, model.Pager.CurrentPage, model.Pager.PageSize, prefix, action, controller);
            model.Items = repo.GetAll(ap => ap.Doctor.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Approved, model.Pager.CurrentPage, model.Pager.PageSize.Value).ToList();
            return View(model);
        }
    }
}