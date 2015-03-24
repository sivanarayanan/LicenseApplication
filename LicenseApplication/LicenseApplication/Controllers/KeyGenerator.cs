using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseApplication.Controllers
{
    public class KeyGenerator
    {
        public string[] Generate(int numKey)
        {
            string[] keys = new string[numKey];

            for (int i = 0; i < numKey; i++)
            {
                keys[i] = CreateKey();
            }
            return keys;
        }

        private string CreateKey()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }
    }
}
