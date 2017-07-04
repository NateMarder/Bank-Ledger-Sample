using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HtmlClient.Dal;
using HtmlClient.Models;
using HtmlClient.Properties;

namespace HtmlClient.Controllers
{
    public class RegisterController : Controller
    {

        [AllowAnonymous]
        public ActionResult Register( RegisterViewModel model )
        {
            var validator = new Validators.RegistrationValidator();
            var results = validator.Validate( model );

            if ( results.IsValid )
            {
                try
                {
                    var dataHandler = new DalHandler();
                    dataHandler.RegisterNewUser( model );

                    // auto-login new users
                    return RedirectToAction( "UserSignIn", "Login",
                        new {Email = model.Email, Password = model.Password} );

                }
                catch (Exception exception)
                {
                    Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    TempData["RegisterMessage"] = Resources.GenericErrorMessage + exception.Message;
                    return View("../Home/Index");
                }
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            TempData["RegisterMessage"] = Resources.GenericErrorMessage;
            return View("../Home/Index");
        }
    }
}