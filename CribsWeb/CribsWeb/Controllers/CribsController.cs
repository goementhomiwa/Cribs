using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Cribs.Web.Helpers;
using Cribs.Web.Models;
using Cribs.Web.Models.Chat;
using Cribs.Web.Repositories;
using Cribs.Web.ViewModels;

namespace Cribs.Web.Controllers
{
    [Authorize]
    public class CribsController : Controller
    {
        private readonly IdentityDb _db;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly IChatGroupRepository _chatGroupRepository;
        private readonly IMessageRepository _messageRepository;
        public CribsController(IChatGroupRepository chatGroupRepository, IMessageRepository messageRepository)
        {
            _chatGroupRepository = chatGroupRepository;
            _messageRepository = messageRepository;
            _db = new IdentityDb();
        }
      
        // GET: Cribs
        [AllowAnonymous]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Any, VaryByParam = "none")]
        public ActionResult Index()
        {
            var cribs = _db.RentCribs.OrderByDescending(x => x.Active);

            List<ThumbnailsViewModel> thumbs = (from crib in cribs
                let image = crib.images.FirstOrDefault(x => x.Cover).Image
                select new ThumbnailsViewModel
                {
                    Id = crib.CribRentGuid, Title = crib.Title, MonthlyRent = crib.MonthlyPrice, DateAvailable = crib.Available, CoverPhoto = image
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


            var user = _db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            rentCrib.images = images;
            rentCrib.User = user;
            rentCrib.CribRentGuid = Guid.NewGuid();
            _db.RentCribs.Add(rentCrib);
            
            _db.SaveChanges();

            _chatGroupRepository.Save(new CribChatGroup{Id=rentCrib.CribRentGuid, Name = "CRIB_CHAT_"+rentCrib.Id});

            return RedirectToAction("View", new { id = rentCrib.CribRentGuid });
        }       
 
        [HttpGet]
        public async Task<ActionResult> View(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            RentCrib rentCrib = await _db.RentCribs.FirstOrDefaultAsync(x=>x.CribRentGuid == id);
            if (rentCrib == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            return View(rentCrib);
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }
            var model = _db.RentCribs.FirstOrDefault(x => x.User.Email == User.Identity.Name && x.CribRentGuid == id);
            
            if (model == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            //get images 
            var coverImage = Convert.ToBase64String(model.images.Where(x => x.Cover).First().Image);

            List<string> supportingImages = (from image in model.images where image.Cover != true select Convert.ToBase64String(image.Image)).ToList();
            EditCribViewModel viewModel = new EditCribViewModel
            {
                Title = model.Title,
                Description = model.Description,
                MonthlyRent = model.MonthlyPrice,
                AvailableDate = model.Available,
                Location = model.Location,
                NumberOfRooms = model.NumberOfRooms,              
            };

            //data to send to the view
            ViewBag.CribId = id;
            ViewBag.CoverImage = coverImage;
            ViewBag.SupportingImages = supportingImages;
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, EditCribViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.CribId = id;
                return View(model);
            }

            //find the crib to update
            RentCrib rentCrib = null;
            if (_db.RentCribs.First(x => (x.CribRentGuid == id && x.User.Email == User.Identity.Name)) != null)
            {
                rentCrib = _db.RentCribs.First(x => (x.CribRentGuid== id && x.User.Email == User.Identity.Name));
            }

            if (rentCrib == null)
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
            rentCrib.images.First(x => x.Cover).Image = CribFileHelper.GetByteArray(model.MainImage);

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
            _db.Entry(rentCrib).State = EntityState.Modified;
            int success = await _db.SaveChangesAsync();
            if(success >= 0)
            {
                //handle as error
                return new HttpNotFoundResult("Whoops! Something weird happened.");
            }

            //successfully updated a post
            return RedirectToAction("View", id);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            var crib = _db.RentCribs.Find(id);
            if (crib == null)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            crib.Active = true;
            _db.Entry(crib).State = EntityState.Modified;
            int result = await _db.SaveChangesAsync();
            if (result == 0)
            {
                throw new ArgumentNullException("Error" + " occured while processing your request. Please contact support.");
            }

            return RedirectToAction("Index");
        }
    }
}