using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace LicenseApplication.Tests.Controllers
{
    class MockHttpContext : HttpContextBase
    {
        private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null);

        public override IPrincipal User
        {
            get { return _user; }
            set { base.User = value; }
        }
    }
}
