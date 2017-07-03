using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using HtmlClient.Models;

namespace HtmlClient.Controllers
{
    public class LoginController : Controller
    {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult UserSignIn(LoginViewModel model)
        {
            return View("../Home/Index");
        }

        public ActionResult Validate(UserViewModel user)
        {
            var userIsOkay = false;
            try
            {
                // 'userIsOkay' look in the local data store to make sure
                // the user now exists there!
                userIsOkay = false;
                if ( userIsOkay )
                {
                    Session["UserId"] = user.Email;
                    Session["SessionGuid"] = Guid.NewGuid();
                }
                else
                {
                    TempData["LoginMessage"] = HtmlClient.Properties.Resources.GenericErrorMessage;
                }
            }
            catch(Exception ex)
            {
                TempData["LoginMessage"] = HtmlClient.Properties.Resources.GenericErrorMessage + Environment.NewLine + ex.Message;
                userIsOkay = false;
                return RedirectToAction( "Validate" );
            }
            
            return RedirectToAction( "UserSignIn" );
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SignOut(int id)
        {
            return new JsonResult();
        }
    }
}
