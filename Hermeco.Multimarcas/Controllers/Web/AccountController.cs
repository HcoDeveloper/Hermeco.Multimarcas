﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Spatial;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.BusinessObjects;

namespace Hermeco.Multimarcas.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            if ( this.Request.RequestType == "POST" ){
                String userName = Request["UserName"];
                String password = Request["Password"];
                DotNetNuke.Security.Membership.MembershipProvider MP;
                UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
                Boolean bLogeado = false;
                String ip = Request.ServerVariables["REMOTE_ADDR"];
                UserInfo userInfo = UserController.ValidateUser(0, userName, password, "", "", ip, ref loginStatus);
                ClienteService cs = new ClienteService();
                if (userInfo != null && loginStatus == UserLoginStatus.LOGIN_SUCCESS)
                {
                    try
                    {
                        Cliente cliente = cs.getInfoClient(userInfo.Profile.GetProperty("VendorId").PropertyValue);
                        if (cliente.Codigo == null)
                        {
                            loginStatus = UserLoginStatus.LOGIN_FAILURE;
                            ViewData["Message"] = "No podemos validar tu cuenta en este momento";
                            ViewBag.Result = true;
                            return View();
                        }
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = "No podemos validar tu cuenta en este momento";
                        ViewBag.Result = true;
                        return View();
                    }

                    Session["userInfo"] = userInfo;
                    string VendorId = "";
                    string FullName = "";
                    if (userInfo.Profile != null && userInfo.Profile.GetProperty("VendorId") != null)
                    {
                        VendorId = userInfo.Profile.GetProperty("VendorId").PropertyValue;
                        FullName = userInfo.FullName;
                    }
                    Session["UserNit"] = VendorId;
                    Session["UserName"] = FullName;
                    return RedirectToAction("Index", "Home");
                }
                else if( loginStatus == UserLoginStatus.LOGIN_FAILURE) {
                    ViewData["Message"] = "Usuario o contraseña no válidos";
                    ViewBag.Result = true;
                    return View();
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["userInfo"] = null;
            Session["UserNit"] = null;
            Session["UserName"] = null;
            return View();
        }
    }
}