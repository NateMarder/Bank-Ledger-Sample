using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentValidation.Results;
using Libraries.Dal;
using Libraries.Validators;
using Libraries.Models;
using Libraries.Properties;

namespace HtmlApp.Controllers
{
    public class RegisterController : Controller
    {
        private XmlDal _xmlDal;
        public XmlDal XmlDal => _xmlDal ?? ( _xmlDal = new XmlDal() );

        [AllowAnonymous]
        public ActionResult Register( UserViewModel model )
        {
            try
            {
                var validator = new RegistrationValidator();
                var result = validator.Validate( model );

                if ( result.IsValid )
                {
                    XmlDal.RegisterNewUser( model );
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
                    Data = GetPrettifiedValidationError( result )
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

        private string GetPrettifiedValidationError( ValidationResult result )
        {
            if ( result.IsValid )
            {
                return null;
            }

            return result.Errors.Aggregate( "", ( current, error ) 
                => current + ( error.ErrorMessage + "\n" ) );
        }
    }
}