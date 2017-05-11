namespace DataAccess.Service
{
    using System.Linq;
    using Entities;
    using Repositories;

    public class AuthenticationService
    {
        public User LoggedUser { get; private set; }

        public void AuthenticateUser(string username, string password)
        {
            UserRepo userRepo = new UserRepo();
            LoggedUser = userRepo.GetAll(x => x.Username == username && x.Password == password).FirstOrDefault();
        }
    }
}
