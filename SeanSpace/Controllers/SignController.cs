using SeanSpace.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeanSpace.Controllers
{
    public class SignController : Controller
    {
        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("/Notepad/Index");
            }

            return View();
        }

        [HttpPost]
        public void SignIn(string userName, string password)
        {
            bool result = new Manager().SignIn(userName, password);
            if (result)
            {
                Response.Redirect("/Notepad/Index");
            }
            else
            {
                Response.Redirect("/Sign/SignIn");
            }
        }

    }
}
