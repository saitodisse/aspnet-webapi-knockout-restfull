using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaMvcApp.Controllers
{
    public class AdministrationController : Controller
    {
        //
        // GET: /Ingredient/

        public ActionResult RecreateDataBase()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult RecreateDataBase(FormCollection collection)
        {
            return View();
        }

        
    }
}
