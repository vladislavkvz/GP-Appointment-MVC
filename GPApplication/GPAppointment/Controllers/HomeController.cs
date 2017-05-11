namespace GPAppointment.Controllers
{
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using Models;
    using System;
    using System.Web.Mvc;
    using static DataAccess.Tools.Enums;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
//            User u1 = new User()
//{
//    FirstName = "Pacienta",
//    IsAdmin = false,
//    Email = "patient@abv.bg",
//    Username = "patient",
//    Position = Position.Patient,
//    LastName = "Pacientov",
//    Password = "patient",

//};

//            User u2 = new User()
//            {
//                FirstName = "Doki",
//                IsAdmin = false,
//                Email = "doctor@abv.bg",
//                Username = "doctor",
//                Position = Position.Doctor,
//                LastName = "Doktorski",
//                Password = "doctor",

//            };


//            UserRepo urepo = new UserRepo();

//            AppointmentRepo repo = new AppointmentRepo();
//            Appointment ap = new Appointment();
//            repo.Count();
//            ap.Status = Status.Approved;
//            ap.Patient = u1;
//            ap.Doctor = u2;
//            ap.ArrangeTime = DateTime.Now;
//            repo.Save(ap);
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (AuthenticationManager.LoggedUser != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (AuthenticationManager.LoggedUser != null)
                return RedirectToAction("Index", "Home");

            AuthenticationManager.Authenticate(username, password);

            if (AuthenticationManager.LoggedUser == null)
            {
                ModelState.AddModelError("authenticationFailed", "Wrong username or password!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            AuthenticationManager.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}