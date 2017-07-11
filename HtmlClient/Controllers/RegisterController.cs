﻿using System;
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
        private DalHandler _dal;
        public DalHandler Dal => _dal ?? ( _dal = new DalHandler( Session["UserId"].ToString() ) );


        [AllowAnonymous]
        public ActionResult Register( RegisterViewModel model )
        {
            try
            {
                var validator = new RegistrationValidator();
                var result = validator.Validate( model );

                if ( result.IsValid )
                {
                    Dal.RegisterNewUser( model );
                    return new HttpStatusCodeResult( HttpStatusCode.OK );
                }

                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = result.Errors.ToList().Select( er => er.ErrorMessage ).ToList().Distinct()
                };
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = Resources.GenericErrorMessage
                };
            }
        }
    }
}