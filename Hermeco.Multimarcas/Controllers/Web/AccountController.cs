using System;
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
                try
                {
                    Cliente cliente = cs.getInfoClient(userInfo.Profile.GetProperty("VendorId").PropertyValue);
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Ha ocurrido un error interno!";
                    ViewBag.Result = true;
                    return View();
                }

                if (userInfo != null && loginStatus == UserLoginStatus.LOGIN_SUCCESS)
                {
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
                    ViewData["Message"] = "Usuario o contraseña no validos";
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