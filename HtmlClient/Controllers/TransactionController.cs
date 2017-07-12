﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
        public DalHandler Dal => _dal ?? ( _dal = new DalHandler(Session.SessionID) );

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

        //[AuthorizeConsoleClient]
        [HttpGet]
        public ActionResult ConsoleTransaction( TransactionRequestModel model )
        {



            try
            {
                switch ( model.Type )
                {
                    case TransactionType.GetHistory:
                        string transactionHistory;
                        var transactions = Dal.GetTransactionHistory( model.UserId );
                        if ( transactions != null && transactions.Length > 0 )
                        {
                            transactionHistory =
                                transactions.Aggregate( "", ( current, item ) => current + ( item.ToString() + "\n" ) );
                        }
                        else
                        {
                            transactionHistory = "No transactions found";
                        }

                        return new ContentResult()
                        {
                            Content = transactionHistory
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
            catch (Exception ex)
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