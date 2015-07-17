using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cribs.Web.Models;

namespace Cribs.Web.ViewModels
{
    public class ThumbnailsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Title:")]
        public string Title { get; set; }

        [Display(Name = "Available Date:")]
        [UIHint("DateTime")]
        public DateTime? DateAvailable { get; set; }

        [UIHint("CribImageDisplay")]
        public byte[] CoverPhoto { get; set; }

        [Display(Name = "Month rent:")]
        public double MonthlyRent { get; set; }

        public static List<ThumbnailsViewModel> CreateList(IQueryable<RentCrib> cribs)
        {
            var thumbs = (from crib in cribs
                let image = crib.images.FirstOrDefault(x => x.Cover).Image
                select new ThumbnailsViewModel
                {
                    Id = crib.CribRentGuid,
                    Title = crib.Title,
                    MonthlyRent = crib.MonthlyPrice,
                    DateAvailable = crib.Available,
                    CoverPhoto = image
                }).ToList();
            return thumbs;
        }
    }
}