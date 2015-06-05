using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cribs.Web.ViewModels;
using System.Data.Entity;
using CribsWeb.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace Cribs.Web.Controllers
{
    [Authorize]
    public class CribsController : Controller
    {
        private IdentityDb db;
        public CribsController()
        {
            db = new IdentityDb();
        }

      
        // GET: Cribs
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(CreateCribViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }


            List<CribImages>  images = new List<CribImages>();
             images.Add(new CribImages{
                Image = GetByteArray(model.MainImage),
                Cover = true
            });
            if(model.Image1 != null)
            {
                images.Add(new CribImages{
                Image = GetByteArray(model.Image1),
                Cover = false
                });
            }

            if(model.Image2 != null)
            {
                images.Add(new CribImages{
                Image = GetByteArray(model.Image2),
                Cover = false
                });
            }

            RentCrib rentCrib = new RentCrib
            {
                Title = model.Title,
                Description = model.Description,
                MonthlyPrice = model.MonthlyRent,
                NumberOfRooms = model.NumberOfRooms,
                Active = true,
                Available = model.AvailableDate,
                DatePost = DateTime.Now,
                DateExpire = DateTime.Now.AddMonths(1),
                Location = model.Location
            };


            ApplicationUser user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            rentCrib.images = images;
            rentCrib.User = user;
            db.RentCribs.Add(rentCrib);
            await db.SaveChangesAsync();
         

            return View();
        }

#region helpers
        public Byte[] GetByteArray(HttpPostedFileBase file)
        {
            Byte[] byteArray = new Byte[file.ContentLength];
            file.InputStream.Position = 0;
            file.InputStream.Read(byteArray, 0, file.ContentLength);

            return byteArray;
        }
#endregion
        
    }
}