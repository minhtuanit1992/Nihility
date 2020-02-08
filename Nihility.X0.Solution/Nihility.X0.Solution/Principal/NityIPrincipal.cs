using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.X0.Solution.Principal
{
    interface NityIPrincipal : IPrincipal
    {
        string ID { get; set; }
        string UserName { get; set; }
    }
}
