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
        public DateTime AvailableDate { get; set; }
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 50 characters")]
        public String Location { get; set; }
        [Display(Name = "Number of rooms")]
        public int NumberOfRooms { get; set; }
        public HttpPostedFileBase Images { get; set; }
    }
}
