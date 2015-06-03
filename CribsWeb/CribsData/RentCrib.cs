using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Cribs.Entities
{
    public class RentCrib
    {
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
        public virtual DbSet<CribImages> images { get; set; }
    }

    public class CribImages
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public bool Cover { get; set; }
        public virtual RentCrib RentCrib { get; set; }
    }
}