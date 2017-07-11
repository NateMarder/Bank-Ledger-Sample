using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlClient.Dal;
using HtmlClient.Enums;
using HtmlClient.Filters;
using HtmlClient.Models;

namespace HtmlClient.Controllers
{
    public class TransactionController : Controller
    {
        private DalHandler _dal;
        public DalHandler Dal => _dal ?? ( _dal = new DalHandler( Session["UserId"].ToString() ) );

        [HttpGet]
        public ActionResult TransactionHistory()
        {
            var valid = Session["UserId"] != null;
            try
            {
                if ( valid )
                {
                    var transactions = Dal.getTransactions( Session["UserId"].ToString() );
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

        //[AuthorizeConsoleClient]
        [HttpGet]
        public ActionResult ConsoleTransaction( TransactionRequestModel model )
        {
            try
            {
                switch ( model.Type )
                {
                    case TransactionType.GetHistory:
                        var transactions = Dal.getTransactions( Session["UserId"].ToString() );
                        return new JsonResult
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = transactions
                        };

                    default:
                        var transactionModel = new TransactionViewModel
                        {
                            UserEmail = Session["UserId"]?.ToString(),
                            Date = DateTime.UtcNow.ToShortTimeString(),
                            IsWithdraw = model.Type == TransactionType.Withdraw,
                            IsDeposit = model.Type == TransactionType.Deposit,
                            Amount = model.Amount ?? 0,
                        };

                        var result = Dal.SubmitTransaction( transactionModel );

                        return new JsonResult
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = result ? "Transaction was successfully tendered." : "Transaction request denied"
                        };
                }
            }
            catch
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
        }

        [HttpPost]
        public ActionResult Transaction( TransactionViewModel model )
        {
            var valid = model != null;

            try
            {
                if ( valid )
                {
                    Dal.SubmitTransaction( model );
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