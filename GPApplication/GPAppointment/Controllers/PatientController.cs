namespace GPAppointment.Controllers
{
    using ViewModels.AppointmentVMs;
    using System.Web.Mvc;
    using DataAccess.Repositories;
    using Models;
    using System.Linq;
    using DataAccess.Entities;
    using static DataAccess.Tools.Enums;
    using Filter;
    using ViewModels;
    using System.Collections.Generic;
    
    [PatientAuthentication]
    public class PatientController : BaseController<Appointment,AppointmentIndexVM,AppointmentEditVM,AppointmentFilterVM>
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
            return repo.GetAll(ap => ap.Patient.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Approved, currentPage, pageSize).ToList();
        }
        protected override int Count()
        {
            AppointmentRepo repo = new AppointmentRepo();
            return repo.Count(ap => ap.Patient.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Approved);
        }

        protected override void PopulateViewModel(AppointmentEditVM model, Appointment entity)
        {
            model.Id = entity.Id;
            model.ArrangeTime = entity.ArrangeTime;
            model.EndArrangeTime = model.ArrangeTime.AddHours(1);
            model.Seen = entity.Seen;
            model.Status = entity.Status;
            model.Patient = entity.Patient;

            UserRepo uRepo = new UserRepo();

            List<User> doctorList = uRepo.GetAll(u=>u.Position==Position.Doctor).ToList();
            model.Doctors = new List<User>();
            foreach (var useritem in doctorList)
            {
                model.Doctors.Add(useritem);
            }

            model.Doctor = uRepo.GetAll(u=>u.Position==Position.Doctor).FirstOrDefault();
            model.SelectedDoctorName = model.Doctor.FirstName + " " + model.Doctor.LastName;
            model.SelectedDoctorId = model.Doctor.Id;
        }

        [HttpPost]
        public override ActionResult Edit(FormCollection collection)
        {
            AppointmentRepo repo = new AppointmentRepo();
            UserRepo urepo = new UserRepo();
            AppointmentEditVM model = new AppointmentEditVM();
            TryUpdateModel(model);
            User doctor = urepo.GetById(model.SelectedDoctorId);
            foreach (var item in doctor.AppointmentsDoctor)
            {
                if ((model.ArrangeTime <= item.ArrangeTime.AddHours(1)) && (model.ArrangeTime > item.ArrangeTime) || (model.ArrangeTime==item.ArrangeTime))
                {
                    return RedirectToAction("BusyArrangeTime", "Patient");
                }
            }
            if (ModelState.IsValid)
            {
                Appointment entity = new Appointment();
                PopulateEntity(entity, model);
                repo.DbContext.Entry(entity.Patient).State = System.Data.Entity.EntityState.Unchanged;
                repo.DbContext.Entry(entity.Doctor).State = System.Data.Entity.EntityState.Unchanged;
                repo.Save(entity);
                return Redirect();
            }
            return View(model);
        }


        protected override void PopulateEntity(Appointment entity, AppointmentEditVM model)
        {
            UserRepo repo = new UserRepo();
            repo.DisableProxy();
            entity.Id = model.Id;
            entity.ArrangeTime = model.ArrangeTime;
            entity.Status = Status.Unseen;
            entity.Patient = repo.GetById(AuthenticationManager.LoggedUser.Id);
            entity.Doctor = repo.GetById(model.SelectedDoctorId);
        }

        public ActionResult ViewConfirmed()
        {
            AppointmentIndexVM model = new AppointmentIndexVM();
            AppointmentRepo repo = new AppointmentRepo();
            model.Items = repo.GetAll(ap => ap.Patient.Id == AuthenticationManager.LoggedUser.Id && ap.Status == DataAccess.Tools.Enums.Status.Approved && ap.Seen==false).ToList();
            foreach (var item in model.Items)
            {
                if (!item.Seen)
                {
                    item.Seen = true;
                    repo.Save(item);
                }
            }

            return View(model);
        }

        public ActionResult BusyArrangeTime()
        {
            return View();
        }
        public ActionResult Reschedule()
        {
            AppointmentIndexVM model = new AppointmentIndexVM();
            AppointmentRepo repo = new AppointmentRepo();
            model.Pager = new PagerVM();
            TryUpdateModel(model);
            string prefix = "Pager.";
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            int resultCount = repo.Count(ap => ap.Patient.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Decline);
            model.Pager = new PagerVM(resultCount, model.Pager.CurrentPage, model.Pager.PageSize, prefix, action, controller);
            model.Items = repo.GetAll(ap => ap.Patient.Id == AuthenticationManager.LoggedUser.Id && ap.Status == Status.Decline, model.Pager.CurrentPage, model.Pager.PageSize.Value).ToList();
            return View(model);
        }
    }
}