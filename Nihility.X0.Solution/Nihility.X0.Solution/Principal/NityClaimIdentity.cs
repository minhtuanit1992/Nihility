using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Nihility.X0.Solution.Principal
{
    public class NityClaimIdentity : IIdentity
    {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public NityClaimIdentity(string name, IEnumerable<Claim> claims)
        {
            if (Claims.Count() > 0)
            {
                this.IsAuthenticated = true;
                this.AuthenticationType = "Forms";
                this.Name = name;
                this.Claims = claims;
            }
        }

        public Claim FindFirst(string key)
        {
            if (Claims.Count() > 0)
            {
                return Claims.FirstOrDefault(x => x.Type == key);
            }

            return null;
        }
    }
}