using System;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using HtmlClient.Enums;
using HtmlClient.Filters;
using HtmlClient.Models;
using HtmlClient.Properties;

namespace HtmlClient.Controllers
{
    public class TransactionController : Controller
    {
        private Dal.Dal _dal;
        public Dal.Dal Dal => _dal ?? ( _dal = new Dal.Dal() );

        [HttpGet]
        public ActionResult TransactionHistory()
        {
            var valid = Session["UserId"] != null;
            try
            {
                if ( valid )
                {
                    var transactions = Dal.GetTransactionHistory( Session["UserId"].ToString() );
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

        /************************************************************
        *************************************************************
        ** 
        ** Console Endpoints and Helper Methods
        **
        *************************************************************
        *************************************************************/
        [AuthorizeConsoleClient]
        public ActionResult ConsoleTransaction( TransactionRequestModel model )
        {
            try
            {
                if ( model.Type == TransactionType.GetHistory )
                {

                    var transactionHistory = "No transactions found";
                    var transactions = Dal.GetTransactionHistory( model.UserId );
                    transactionHistory = GetTransactionSummaryString( transactions );
                    return new ContentResult {Content = transactionHistory};
                }

                var transactionModel = new TransactionViewModel
                {
                    UserEmail = model.UserId,
                    Date = DateTime.UtcNow.ToString( CultureInfo.InvariantCulture ),
                    IsWithdraw = model.Type == TransactionType.Withdraw,
                    IsDeposit = model.Type == TransactionType.Deposit,
                    Amount = model.Amount ?? 0,
                };

                return new ContentResult
                {
                    Content = Dal.SubmitTransaction( transactionModel ) 
                        ? "Transaction was successfully tendered." 
                        : "Transaction request denied"
                };
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
        }

        private string GetTransactionSummaryString(TransactionViewModel[] transactions)
        {
            var summaryString = "";
            var balance = 0.0;

            foreach ( var transaction in transactions )
            {
                balance += transaction.IsDeposit 
                    ? transaction.Amount 
                    : transaction.Amount * -1;
                
                summaryString += "  "+transaction + "\n";
            }

            return string.Format( Resources.TransactionHistoryString, summaryString, balance );
        }
    }
}