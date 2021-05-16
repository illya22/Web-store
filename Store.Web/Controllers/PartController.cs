using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Lib.Abstract;
using Store.Lib.Entities;
using Store.Web.Models;

 
namespace Store.Web.Controllers
{
     
    public class PartController : Controller
    {
        private IPartRepository repository;
        public int pageSize = 4;

        public PartController(IPartRepository repo)
        {
            repository = repo;
        }
        
        public ViewResult List( string type, int page = 1)
        {
            PartListViewModel model = new PartListViewModel
            {
                Parts = repository.Parts
                .Where(p => type == null || p.Type == type)
                .OrderBy(part => part.Part_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PaginInfo = new PaginInfo
                {
                    Current_Page = page,
                    Items_On_Page = pageSize,
                    Total_Items = type == null ?
                    repository.Parts.Count() :
                    repository.Parts.Where(part => part.Type == type).Count()
                },
                Current_Type = type
            };
            return View(model);
        }

        public FileContentResult GetImage(int partid)
        {
            Part part = repository.Parts
                .FirstOrDefault(p => p.Part_Id == partid);

            if (part != null)
            {
                return File(part.ImageData, part.ImageType);
            }
            else
            {
                return null;
            }
        }
    }
}