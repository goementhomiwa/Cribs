using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cribs.Web.ViewModels
{
    public class SearchModel
    {
        [Display(Name="Min Price")]
        public double? MinPrice { get; set; }
        [Display(Name = "Max Price")]
        public double? MaxPrice { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Rooms")]
        public int? NumberOfRooms { get; set; }
        [Display(Name = "Search Phrase")]
        public string KeyWord { get; set; }
        public string Username { get; set; }
    }
}
