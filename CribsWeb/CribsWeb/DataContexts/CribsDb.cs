using System.Data.Entity;
using Cribs.Entities;

namespace Cribs.Web.DataContexts
{
    public class CribsDb : DbContext
    {
        public CribsDb()
            : base("AuthContext")
        {
        }
        public DbSet<RentCrib> RentCribs { get; set; }
        public DbSet<CribImages> CribsImageSet { get; set; }
    }
}