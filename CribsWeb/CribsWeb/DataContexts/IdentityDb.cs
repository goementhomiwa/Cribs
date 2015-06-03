using CribsWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Cribs.Web.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("AuthContext")
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }
}