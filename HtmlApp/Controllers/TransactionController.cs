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

        private LocalDal _localDal;
        public LocalDal LocalDal => _localDal ?? ( _localDal = new LocalDal() );

        [HttpGet]
        public ActionResult TransactionHistory()
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var data = Properties.Settings.Default.UseXmlDataStore
                    ? XmlDal.GetTransactionHistory( userId )
                    : LocalDal.GetTransactionHistory( userId );

                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = Json(data)
                };
            }
            catch
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Transaction( TransactionViewModel model )
        {
            //todo: validate that transaction view model properly
            try
            {
                if ( Properties.Settings.Default.UseXmlDataStore )
                {
                    XmlDal.SubmitTransaction( model );
                }
                else
                {
                    LocalDal.SubmitTransaction( model );
                }

                return new HttpStatusCodeResult( HttpStatusCode.OK );
            }
            catch
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
        }

        /************************************************************
        *************************************************************
        ** 
        ** Console Endpoints and Helper Methods
        **
        *************************************************************
        *************************************************************/
        [HttpGet, AuthorizeConsoleAppUser]
        public ActionResult TransactionHistoryForConsole()
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

        [AuthorizeConsoleAppUser]
        public ActionResult TransactionForConsole( TransactionRequestModel model )
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