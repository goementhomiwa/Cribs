using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
namespace Cribs.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<RentCrib> RentCribs { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("DefaultConnection")
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
        [Required]
        [StringLength(20, ErrorMessage = "Title cannot exceed 20 characters")]
        public string PreferredName { get; set; }
        public DbSet<RentCrib> RentCribs { get; set; }
        public DbSet<CribImages> RentCribImages { get; set; }
    }
    public class RentCrib
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
        [Required]
        public double MonthlyPrice { get; set; }
        [Required]
        public int NumberOfRooms { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Available { get; set; }
        public DateTime? DatePost { get; set; }
        public DateTime? DateExpire { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CribImages> images { get; set; }
    }

    public class CribImages
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public bool Cover { get; set; }
        [Required]
        public virtual RentCrib RentCrib { get; set; }
    }

}