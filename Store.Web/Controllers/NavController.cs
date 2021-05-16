using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Lib.Abstract;

namespace Store.Web.Controllers
{
    public class NavController : Controller
    {
        private IPartRepository repository;

        public NavController(IPartRepository rep)
        {
            repository = rep;
        }

         public PartialViewResult Menu(string types = null)
        {
            ViewBag.Selected_Type = types;

            IEnumerable<string> type = repository.Parts
              .Select(part => part.Type)
              .Distinct()
              .OrderBy(x => x);
            return PartialView(type);

            
        }
    }
}