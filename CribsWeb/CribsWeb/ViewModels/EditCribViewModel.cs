using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Cribs.Web.Annotations;

namespace Cribs.Web.ViewModels
{
    public class EditCribViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name="Title")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Details")]
        [StringLength(500, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Description { get; set; }
        [Display(Name = "Monthly Rent")]
        public double MonthlyRent { get; set; }
        [Required]
        [Display(Name = "Available Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NotPast(ErrorMessage = "Date and time should be set to the future. ")]
        [DataType(DataType.DateTime)]
        public DateTime? AvailableDate { get; set; }
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 50 characters")]
        public String Location { get; set; }
        [Range(1, int.MaxValue)]
        [Display(Name = "Number of rooms")]
        public int NumberOfRooms { get; set; }
        [Display(Name="Cover Photo")]
        [CribImage]
        [DataType(DataType.ImageUrl)]
        public HttpPostedFileBase MainImage { get; set; }
        [Display(Name = "1)")]
        [CribImage]
        public HttpPostedFileBase Image1 { get; set; }
        [Display(Name = "2)")]
        [CribImage]
        public HttpPostedFileBase Image2 { get; set; }
    }
}
