using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace S3Train.Domain
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<TaiLieuVanBan> TaiLieuVanBans { get; set; }
        public virtual ICollection<Hop> Hops { get; set; }
        public virtual ICollection<HoSo> HoSos { get; set; }
        public virtual ICollection<Ke> Kes { get; set; }
        public virtual ICollection<MuonTra> MuonTras { get; set; }
        public virtual ICollection<LichSuHoatDong> LichSuHoatDongs { get; set; }
    }

    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public string Description { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
    }
}