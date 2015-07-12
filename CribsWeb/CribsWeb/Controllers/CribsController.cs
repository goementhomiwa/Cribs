using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Cribs.Web.Helpers;
using Cribs.Web.Models;
using Cribs.Web.ViewModels;

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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Any, VaryByParam = "none")]
        public ActionResult Index()
        {
            var cribs = db.RentCribs.OrderByDescending(x => x.Active);
            List<ThumbnailsViewModel> thumbs = (from crib in cribs
                let image = crib.images.FirstOrDefault(x => x.Cover).Image
                select new ThumbnailsViewModel
                {
                    Id = crib.Id, Title = crib.Title, MonthlyRent = crib.MonthlyPrice, DateAvailable = crib.Available, CoverPhoto = image
                }).ToList();
            ViewBag.SearchParams = new SearchModel();
            return View(thumbs);
        }

        [HttpPost]
        public ActionResult Search(SearchModel queryParams)
        {
            var list = ThumbnailsViewModel.CreateList(CribSearch.Search(queryParams));
            return PartialView("PartialViews/_SearchResultsPartialView", list);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateCribViewModel model)
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


            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            rentCrib.images = images;
            rentCrib.User = user;
            db.RentCribs.Add(rentCrib);
            
            db.SaveChanges();

            return RedirectToAction("View", new { id = rentCrib.Id });
        }       
 
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            RentCrib rentCrib = await db.RentCribs.FindAsync(id);
            if (rentCrib == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            return View(rentCrib);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            var model = db.RentCribs.Find(id);
            
            if (model == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            if (model.User.UserName != HttpContext.User.Identity.Name)
            {
                throw new UnauthorizedAccessException("Error : You are not authorized to perform this action.");
            }
            EditCribViewModel viewModel = new EditCribViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                MonthlyRent = model.MonthlyPrice,
                AvailableDate = model.Available,
                Location = model.Location,
                NumberOfRooms = model.NumberOfRooms,
            };
            //get images 
            ViewBag.CoverImage = Convert.ToBase64String(model.images.Where(x => x.Cover).First().Image);

            ViewBag.SupportingImages = (from image in model.images where image.Cover != true select Convert.ToBase64String(image.Image)).ToList();
          
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditCribViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            //find the crib to update
            RentCrib rentCrib = db.RentCribs.First(x => (x.Id == model.Id && x.User.Email == User.Identity.Name));

            if (rentCrib == null)
            {
                //Handle as error
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            rentCrib.Title = model.Title;
            rentCrib.Description = model.Description;
            rentCrib.Location = model.Location;
            rentCrib.NumberOfRooms = model.NumberOfRooms;
            rentCrib.Available = model.AvailableDate;
            rentCrib.MonthlyPrice = model.MonthlyRent;

            #region set images edit
            if(model.MainImage != null)
            { 
                rentCrib.images.First(x => x.Cover).Image =  CribFileHelper.GetByteArray(model.MainImage);
            }
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
            int success = db.SaveChanges();

            //successfully updated a post
            return RedirectToAction("View", new {id = model.Id});
        }

        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            var crib = db.RentCribs.Find(id);
            if (crib == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            crib.Active = true;
            db.Entry(crib).State = EntityState.Modified;
            int result = await db.SaveChangesAsync();
            if (result == 0)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            return RedirectToAction("Index");
        }
    }
}