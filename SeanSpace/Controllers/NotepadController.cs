using SeanSpace.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeanSpace.Controllers
{
    [Authorize]
    public class NotepadController : Controller
    {
        private long userSid;
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            userSid = long.Parse(this.User.Identity.Name);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new Manager().GetNotepadList(userSid);
            return View(model);
        }

        [HttpPost]
        public void Delete(long sid)
        {
            new Manager().DeleteNote(sid, userSid);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void AddOrUpdate(long sid, string title, string content)
        {
            if (sid == 0)
            {
                new Manager().AddNote(userSid, title, content);
            }
            else
            {
                new Manager().UpdateNote(sid, userSid, title, content);
            }
        }
    }
}
