using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rota.Security.ViewModels
{
    public class UserInfoViewModel
    {
        public string UserName { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
}