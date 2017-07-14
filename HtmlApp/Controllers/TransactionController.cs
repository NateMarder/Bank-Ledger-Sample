using System;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using HtmlApp.Filters;
using Libraries.Dal;
using Libraries.Enums;
using Libraries.Models;
using Libraries.Properties;

namespace HtmlApp.Controllers
{
    public class TransactionController : Controller
    {
        private XmlDal _xmlDal;
        public XmlDal XmlDal => _xmlDal ?? ( _xmlDal = new XmlDal() );

        [HttpGet]
        public ActionResult TransactionHistory()
        {
            var valid = Session["UserId"] != null;
            try
            {
                if ( valid )
                {
                    var transactions = XmlDal.GetTransactionHistory( Session["UserId"].ToString() );
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
                    XmlDal.SubmitTransaction( model );
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
        [AuthorizeConsoleAppUser]
        public ActionResult ConsoleTransaction( TransactionRequestModel model )
        {
            try
            {
                if ( model.Type == TransactionType.GetHistory )
                {

                    var transactionHistory = "No transactions found";
                    var transactions = XmlDal.GetTransactionHistory( model.UserId );
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
                    Content = XmlDal.SubmitTransaction( transactionModel ) 
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