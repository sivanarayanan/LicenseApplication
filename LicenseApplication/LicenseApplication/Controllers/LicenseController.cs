using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseApplication.Models;

namespace LicenseApplication.Controllers
{
    public class LicenseController : Controller
    {
        LicenseEntities_InactiveLicense db = new LicenseEntities_InactiveLicense();
        Inactive_Licenses licenseKeys = new Inactive_Licenses();
        string[] keys;

        //
        // GET: /License/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /License/GenerateLicense
        public ActionResult GenerateLicense()
        {
            IEnumerable<Inactive_Licenses> licenses = db.Inactive_Licenses.ToList();
            return View(licenses);
        }


        //
        // POST : /License/GenerateLicense
        [HttpPost]
        public ActionResult GenerateLicense(string value)
        {
            KeyGenerator genKey = new KeyGenerator();
            int numKey = Convert.ToInt32(value);
            keys = genKey.Generate(numKey);
            licenseKeys = SaveLicense_Key();
            IEnumerable<Inactive_Licenses> licenses = db.Inactive_Licenses.ToList();
            return View(licenses);
        }

        private Inactive_Licenses SaveLicense_Key()
        {

            foreach (var i in keys)
            {
                licenseKeys.License_keys = i;
                db.Inactive_Licenses.Add(licenseKeys);
                db.SaveChanges();
            }
            return licenseKeys;

        }
    }
}
