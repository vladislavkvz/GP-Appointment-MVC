namespace GPAppointment.Controllers
{
    using DataAccess.Entities;
    using Filter;
    using DataAccess.Repositories;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels.UserVMs;
    using ViewModels;

    [AdminAuthentiation]
    public class UserController : BaseController<User, UserIndexVM, UserEditVM,UsersFilterVM>
    {
        protected override UserEditVM CreateBaseEVM()
        {
            return new UserEditVM();
        }

        protected override UserIndexVM CreateBaseIVM()
        {
            return new UserIndexVM();
        }

        protected override BaseRepo<User> CreateRepository()
        {
            return new UserRepo();
        }

        protected override List<User> PopilateaList(int currentPage, int pageSize)
        {
            UserRepo repo = new UserRepo();
            return repo.GetAll(u=>u.Id !=AuthenticationManager.LoggedUser.Id,currentPage,pageSize).ToList();
        }

        protected override int Count()
        {
            UserRepo repo = new UserRepo();
            return repo.Count(u => u.Id != AuthenticationManager.LoggedUser.Id);
        }
        

        protected override void PopulateViewModel(UserEditVM model, User entity)
        {
            model.Id = entity.Id;
            model.Username = entity.Username;
            model.Password = entity.Password;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Email = entity.Email;
            model.Position = entity.Position;
            model.IsAdmin = entity.IsAdmin;
        }

        protected override void PopulateEntity(User entity, UserEditVM model)
        {
            entity.Id = model.Id;
            entity.Username = model.Username;
            entity.Password = model.Password;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Position = model.Position;
            entity.IsAdmin = model.IsAdmin;
        }

        public override ActionResult Index()
        {
            UserRepo repo = new UserRepo();
            UserIndexVM model = CreateBaseIVM();
            model.Pager = new PagerVM();
            model.Filter = new UsersFilterVM();
            TryUpdateModel(model);

            model.Filter.Prefix = "Filter.";
            int resultCount = repo.Count(model.Filter.BuildFilter());
            string prefix = "Pager.";
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Pager = new PagerVM(resultCount, model.Pager.CurrentPage, model.Pager.PageSize, prefix, action, controller);
            model.Filter.ParentPager = model.Pager;
            model.Items = repo.GetAll(model.Filter.BuildFilter(), model.Pager.CurrentPage, model.Pager.PageSize.Value).ToList();

            return View(model);

        }


        [HttpGet]
        public ActionResult Register()
        {
            User entity = new User();
            UserEditVM model = new UserEditVM();
            model.Id = entity.Id;
            model.Username = entity.Username;
            model.Password = entity.Password;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Email = entity.Email;
            model.Position = entity.Position;
            model.IsAdmin = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(UserEditVM model)
        {
            if (ModelState.IsValid)
            {
                UserRepo usersRepository = new UserRepo();
                User entity = new User();
                entity.Id = model.Id;
                entity.Username = model.Username;
                entity.Password = model.Password;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.Position = model.Position;
                entity.IsAdmin = model.IsAdmin;
                usersRepository.Save(entity);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        protected override void DeleteFilter(int id)
        {
            AppointmentRepo aRepo = new AppointmentRepo();
            List<Appointment> appointmentsDoc = aRepo.GetAll(ap => ap.Doctor.Id == id).ToList();
            List<Appointment> appointmentsPat = aRepo.GetAll(ap => ap.Patient.Id == id).ToList();
            for (int i = 0; i < appointmentsDoc.Count; i++)
            {
                aRepo.Delete(appointmentsDoc[i]);
            }
            for (int i = 0; i < appointmentsPat.Count; i++)
            {
                aRepo.Delete(appointmentsPat[i]);
            }
        }
    }
}