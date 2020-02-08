using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Models.GoogleAuthenticator
{
    public class GoogleAuthenticatorViewModel
    {
        public string SecretKey { get; set; }
        public string BarcodeUrl { get; set; }
        public string TOTPCode { get; set; }
    }
}