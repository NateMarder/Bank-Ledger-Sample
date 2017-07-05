using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentValidation.Results;
using HtmlClient.Dal;
using HtmlClient.Models;
using HtmlClient.Properties;
using HtmlClient.Validators;

namespace HtmlClient.Controllers
{
    public class RegisterController : Controller
    {

        [AllowAnonymous]
        public ActionResult Register( RegisterViewModel model )
        {
            try
            {
                var validator = new RegistrationValidator();
                var result = validator.Validate( model );

                if ( result.IsValid )
                {
                    var dataHandler = new DalHandler();
                    dataHandler.RegisterNewUser( model );   
                    return new HttpStatusCodeResult( HttpStatusCode.OK );
                }

                TempData["RegistrationErrors"] = result.Errors as List<ValidationFailure>;
                return new HttpStatusCodeResult( HttpStatusCode.Conflict );
            }
            catch ( Exception ex)
            {
                TempData["RegistrationErrors"] = Resources.GenericErrorMessage +"\r\n"+ ex.Message;
                return new HttpStatusCodeResult( HttpStatusCode.Conflict );
            }
        }
    }
}