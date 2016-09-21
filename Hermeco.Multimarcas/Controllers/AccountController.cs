using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Spatial;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;

namespace Hermeco.Multimarcas.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {

            String userName = Request["UserName"];
            String password = Request["Password"];
            DotNetNuke.Security.Membership.MembershipProvider MP;
            UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
            Boolean bLogeado = false;
            String ip = Request.ServerVariables["REMOTE_ADDR"];
            UserInfo userInfo = UserController.ValidateUser(0, userName, password, "", "", ip, ref loginStatus);
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
            return View();
        }
    }
}