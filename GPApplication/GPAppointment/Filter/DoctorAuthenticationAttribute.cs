namespace GPAppointment.Filter
{
    using Models;
    using System.Web.Mvc;
    using static DataAccess.Tools.Enums;

    public class DoctorAuthenticationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((AuthenticationManager.LoggedUser == null) || (AuthenticationManager.LoggedUser.Position!=Position.Doctor))
            {
                filterContext.Result = new RedirectResult(@"\");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}