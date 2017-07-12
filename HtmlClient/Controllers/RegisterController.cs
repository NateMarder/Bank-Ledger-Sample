using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HtmlClient.Dal;
using HtmlClient.Models;
using HtmlClient.Properties;
using HtmlClient.Validators;

namespace HtmlClient.Controllers
{
    public class RegisterController : Controller
    {
        private DalHandler _dal;
        public DalHandler Dal => _dal ?? ( _dal = new DalHandler( Session.SessionID ) );


        [AllowAnonymous]
        public ActionResult Register( UserViewModel model )
        {
            try
            {
                var validator = new RegistrationValidator();
                var result = validator.Validate( model );

                if ( result.IsValid )
                {
                    Dal.RegisterNewUser( model );
                    Response.StatusCode = (int) HttpStatusCode.OK;

                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new[] {"New user " + model.Email + " was successfully registered."}
                    };
                }

                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = result.Errors.ToList().Select( er => er.ErrorMessage ).ToList().Distinct()
                };
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = Resources.GenericErrorMessage + "\n" + ex.Message
                };
            }
        }
    }
}