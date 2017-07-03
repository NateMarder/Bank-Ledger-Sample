using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using HtmlClient.Dal;
using HtmlClient.Models;

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
                    return RedirectToAction( "UserSignIn", "Login", new {Email = model.Email, Password = model.Password} );
                }
                catch (Exception exception)
                {
                    Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    TempData["RegisterMessage"] = Properties.Resources.GenericErrorMessage + exception.Message;
                    return View("../Home/Index");
                }
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            var result = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = results.Errors.Select( e => e.ErrorMessage ).ToArray(),
            };

            return result;
        }
    }
}