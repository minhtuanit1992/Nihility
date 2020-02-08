using Nihility.X0.Solution.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Nihility.X0.Solution.Models.Principal
{
    public class PrincipalModel : NityIPrincipal
    {
        public string ID { get; set; }
        public string UserName { get; set; }

        public IIdentity Identity { get; private set; }

        public PrincipalModel(string name, IEnumerable<Claim> claims)
        {
            GenericIdentity genericIdentity = new GenericIdentity("");
            this.Identity = new NityClaimIdentity(name, claims);
        }

        public bool IsInRole(string role)
        {
            return false;
        }      
    }
}