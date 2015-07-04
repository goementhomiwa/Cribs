using System.Web.Mvc;
using System.Web.UI.WebControls;

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