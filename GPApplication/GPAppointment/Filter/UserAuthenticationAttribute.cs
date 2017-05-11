namespace GPAppointment.Filter
{
    using System.Web.Mvc;
    using Models;

    public class UserAuthentiationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((AuthenticationManager.LoggedUser == null))
            {
                filterContext.Result = new RedirectResult(@"\");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
