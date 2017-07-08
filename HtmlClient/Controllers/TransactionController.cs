using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlClient.Dal;
using HtmlClient.Models;

namespace HtmlClient.Controllers
{
    public class TransactionController : Controller
    {

        
        [HttpGet]
        public ActionResult TransactionHistory()
        {

            var valid = Session["UserId"] != null;
            var dataHandler = new DalHandler();

            try
            {
                if ( valid )
                {
                    var transactions = dataHandler.getTransactions( Session["UserId"].ToString() );
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = transactions
                    };
                }
            }
            catch
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
            return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
        }

        [HttpPost]
        public ActionResult Transaction(TransactionViewModel model)
        {

            var valid = model != null;
            var dataHandler = new DalHandler();

            try
            {
                if ( valid )
                {
                    dataHandler.SubmitTransaction( model );  
                    return new HttpStatusCodeResult( HttpStatusCode.OK );
                }
            }
            catch
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
            return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
        }
    }
}
