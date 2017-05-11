namespace GPAppointment.Filter
{
    using Models;
    using System.Web.Mvc;
    using static DataAccess.Tools.Enums;

    public class PatientAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((AuthenticationManager.LoggedUser == null) || (AuthenticationManager.LoggedUser.Position != Position.Patient))
            {
                filterContext.Result = new RedirectResult(@"\");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}