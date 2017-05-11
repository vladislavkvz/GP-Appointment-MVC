namespace GPAppointment.ViewModels.UserVMs
{
    using DataAccess.Entities;
    using Models;
    using Tools;
    using System;
    using System.Linq.Expressions;

    public class UsersFilterVM : BaseFilterVM<User>
    {
        [FilterProperty(DisplayName = "Username")]
        public string Username { get; set; }

        [FilterProperty(DisplayName = "FirstName")]
        public string FirstName { get; set; }

        [FilterProperty(DisplayName = "LastName")]
        public string LastName { get; set; }

        [FilterProperty(DisplayName = "Email")]
        public string Email { get; set; }

        public override Expression<Func<User, Boolean>> BuildFilter()
        {
            return (u => (String.IsNullOrEmpty(Username) || u.Username.Contains(Username)) &&
                            (String.IsNullOrEmpty(FirstName) || u.FirstName.Contains(FirstName)) &&
                            (String.IsNullOrEmpty(LastName) || u.LastName.Contains(LastName)) &&
                            (String.IsNullOrEmpty(Email) || u.Email.Contains(Email)) &&
                             u.Id != AuthenticationManager.LoggedUser.Id);
        }
    }
}