using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [StringLength(500, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Description { get; set; }

        [Display(Name = "Monthly Rent")]
        [Required]
        public double MonthlyRent { get; set; }

        [Display(Name = "Available Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NotPast(ErrorMessage = "Date and time should be set to the future. ")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime? AvailableDate { get; set; }

        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 50 characters")]
        [Required]
        public String Location { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Number of rooms")]
        [Required]
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
