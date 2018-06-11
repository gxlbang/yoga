using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.SystemModule.Controllers
{
    public class ImagesController : Controller
    {
        //
        // GET: /SystemModule/Images/

        public ActionResult Index(string Title,string Image)
        {
            ViewBag.Image = Image;
            ViewBag.Title = Title;
            return View();
        }

    }
}
