using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Cribs.Web.Annotations;

namespace Cribs.Web.ViewModels
{
    public class CreateCribViewModel
    {
        [Required]
        [Display(Name="Title")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; }

        [Display(Name = "Details")]
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }

        [Display(Name = "Monthly Rent")]
        [Required]
        public double MonthlyRent { get; set; }

        [Display(Name = "Available Date")]
        [NotPast(ErrorMessage = "Date and time should be set to the future. ")]
        [UIHint("DateTime")]
        [Required]
        public DateTime? AvailableDate { get; set; }

        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 50 characters")]
        [Required]
        public String Location { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Number of rooms")]
        [Required]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Cover photo is required")]
        [CribImage]
        [DataType(DataType.ImageUrl)]
        public HttpPostedFileBase MainImage { get; set; }
        [CribImage]
        public HttpPostedFileBase Image1 { get; set; }
        [CribImage]
        public HttpPostedFileBase Image2 { get; set; }
    }
}
