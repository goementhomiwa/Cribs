using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cribs.Web.Controllers
{
    public class CribsController : Controller
    {
        // GET: Cribs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        
    }
}