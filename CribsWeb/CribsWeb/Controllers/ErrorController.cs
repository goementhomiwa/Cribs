using System.Web.Mvc;

namespace Cribs.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Title = "Error";
            return View();
        }
    }
}