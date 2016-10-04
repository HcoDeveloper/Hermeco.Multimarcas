using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermeco.Multimarcas.Controllers
{
    public class Security
    {
        public static Boolean ValidarAcceso(){
            var Session = HttpContext.Current.Session;
            if (Session["userInfo"] != null)
            {
                return true;
            }
            return false;
        }

    }
}