using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cribs.Web.ViewModels;
using System.Data.Entity;
using Cribs.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Cribs.Web.Helpers;

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
                Image = CribFileHelper.GetByteArray(model.MainImage),
                Cover = true
            });
            if(model.Image1 != null)
            {
                images.Add(new CribImages{
                Image = CribFileHelper.GetByteArray(model.Image1),
                Cover = false
                });
            }

            if(model.Image2 != null)
            {
                images.Add(new CribImages{
                Image = CribFileHelper.GetByteArray(model.Image2),
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
         

            return RedirectToAction("CribSuccess", rentCrib.Id);
        }       
 
        public ActionResult View(int cribId)
        {
            RentCrib rentCrib = db.RentCribs.Where(x => x.Id == cribId).FirstOrDefault();
            return View(rentCrib);
        }

        [Authorize]
        public ActionResult Edit(int Id)
        {
            var model = db.RentCribs.Where(x => x.User.Email == User.Identity.Name).Find(Id);
            
            if (model == null)
            {
                return new HttpNotFoundResult("Whoops! Something weird happened.");
            }

            List<string> supportingImages = new List<string>();

            //get images 
            var coverImage = Convert.ToBase64String(model.images.Where(x => x.Cover == true).FirstOrDefault().Image);

            foreach (var image in model.images)
            {
                if(image.Cover != true)
                {
                    supportingImages.Add(Convert.ToBase64String(image.Image));
                }
            }
            CreateCribViewModel viewModel = new CreateCribViewModel
            {
                Title = model.Title,
                Description = model.Description,
                MonthlyRent = model.MonthlyPrice,
                AvailableDate = model.Available,
                Location = model.Location,
                NumberOfRooms = model.NumberOfRooms,              
            };

            //data to send to the view
            ViewBag.CribId = Id;
            ViewBag.CoverImage = coverImage;
            ViewBag.SupportingImages = supportingImages;
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(int Id, EditCribViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.CribId = Id;
                return View(model);
            }

            //find the crib to update
            RentCrib rentCrib = db.RentCribs.Where(x => (x.Id == Id && x.User.Email == User.Identity.Name)).FirstOrDefault();

            if(rentCrib == null)
            {
                //Handle as error
                return new HttpNotFoundResult("Whoops! Something weird happened.");
            }

            rentCrib.Title = model.Title;
            rentCrib.Description = model.Description;
            rentCrib.Location = model.Location;
            rentCrib.NumberOfRooms = model.NumberOfRooms;
            rentCrib.Available = model.AvailableDate;

            #region set images edit
            rentCrib.images.Where(x => x.Cover == true).First().Image = CribFileHelper.GetByteArray(model.MainImage);

            var supportingImages = rentCrib.images.Where(x => x.Cover == false).ToList();
            if(supportingImages.Count == 0)
            {
                if(model.Image1 != null)
                {
                    rentCrib.images.Add(new CribImages{Image = CribFileHelper.GetByteArray(model.Image1)});
                }
                if (model.Image2 != null)
                {
                    rentCrib.images.Add(new CribImages{Image = CribFileHelper.GetByteArray(model.Image2)});

                }
            }
            else if (supportingImages.Count == 1)
            {
                if(model.Image1 != null)
                {
                    supportingImages.First().Image = CribFileHelper.GetByteArray(model.Image1);
                }
                if(model.Image2 != null)
                {
                    rentCrib.images.Add(new CribImages { Image = CribFileHelper.GetByteArray(model.Image2) });
                }
            }
            else if(supportingImages.Count == 2)
            {
                if(model.Image1 != null)
                {
                    supportingImages.First().Image = CribFileHelper.GetByteArray(model.Image1);
                }
                if(model.Image2 != null)
                {
                    supportingImages.Last().Image = CribFileHelper.GetByteArray(model.Image2);

                }
            }
            #endregion
            db.Entry(rentCrib).State = EntityState.Modified;
            int success = await db.SaveChangesAsync();
            if(success >= 0)
            {
                //handle as error
                return new HttpNotFoundResult("Whoops! Something weird happened.");
            }

            //successfully updated a post
            return RedirectToAction("View", Id);
        }
    }
}