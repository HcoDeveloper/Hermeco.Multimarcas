using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hermeco.Multimarcas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Security.ValidarAcceso())
            {
                return View();
            }
            return RedirectToAction("login", "Account");
        }

        public ActionResult Catalogo()
        {
            if (Security.ValidarAcceso())
            {
                return View();
            }
            return RedirectToAction("login", "Account");
        }

        public ActionResult Ofertas()
        {
            if (Security.ValidarAcceso())
            {
                return View();
            }
            return RedirectToAction("login", "Account");
        }

        public ActionResult Pedido()
        {
            if (Security.ValidarAcceso())
            {
                return View();
            }
            return RedirectToAction("login", "Account");
        }
    }
}