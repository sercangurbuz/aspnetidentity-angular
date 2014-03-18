using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Rota.Security.Models;

namespace Rota.Security.Extensions
{
    public class RotaClaimsIdentityFactory : ClaimsIdentityFactory<RotaUser, long>
    {
        public RotaClaimsIdentityFactory()
        {

        }


        public async override Task<ClaimsIdentity> CreateAsync(UserManager<RotaUser, long> manager,
                                                               RotaUser user,
                                                               string authenticationType)
        {
            //Yeni claim based identity yaratiliyor
            var id = new ClaimsIdentity(authenticationType, UserNameClaimType, null);
            //Claim'ler ekleniyor
            //Kullanici id si
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
            //Kullanici adi
            id.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
            //Son giriş tarihi
            id.AddClaim(new Claim("LoginTime", DateTime.Now.ToString()));
            //Tum claimleri cekip idendtity'e ekler
            id.AddClaims(await manager.GetClaimsAsync(user.Id));
            //Sonuc
            return id;
        }
    }
}