using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cribs.Web.Models;

namespace Cribs.Web.ViewModels
{
    public class ThumbnailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Title:")]
        public string Title { get; set; }
        [Display(Name = "Available Date:")]
        [UIHint("DateTime")]
        public DateTime? DateAvailable { get; set; }
        [UIHint("CribImageDisplay")]
        public byte[] CoverPhoto { get; set; }
        [Display(Name="Month rent:")]
        public double MonthlyRent { get; set; }

        public static List<ThumbnailsViewModel> CreateList(IQueryable<RentCrib> list)
        {
            List<ThumbnailsViewModel> thumbs = new List<ThumbnailsViewModel>();
            foreach (var crib in list)
            {
                CribImages first = crib.images.FirstOrDefault(x => x.Cover);
                
                thumbs.Add(new ThumbnailsViewModel
                {
                    Id = crib.Id,
                    Title = crib.Title,
                    MonthlyRent = crib.MonthlyPrice,
                    DateAvailable = crib.Available,
                    CoverPhoto = first != null ? first.Image : null
                });               
            }

            return thumbs;
        }
    }
}
