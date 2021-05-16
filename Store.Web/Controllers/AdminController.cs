using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Lib.Abstract;
using Store.Lib.Entities;

namespace Store.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IPartRepository repository;

        public AdminController(IPartRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Parts);
        }

        public ViewResult Edit(int partId )
        {
             
                Part part = repository.Parts
                    .FirstOrDefault(p => p.Part_Id == partId);
                   return View(part);
        }

        public ViewResult Create()
        {
            return View("Edit", new Part());
        }

        [HttpPost]
        public ActionResult Edit(Part part, HttpPostedFileBase image = null)
        {
            if(ModelState.IsValid)
            {
                if(image != null)
                {
                    part.ImageType = image.ContentType;
                    part.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(part.ImageData, 0, image.ContentLength);
                }

                repository.SavePart(part);
                TempData["message"] = string.Format("Changes in \"{0}\" were saved", part.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(part);
            }
        }

        [HttpPost]
        public ActionResult Delete(int part_id)
        {
            Part deletePart = repository.DeletePart(part_id);
            if(deletePart != null)
            {
                TempData["message"] = string.Format("Item \"{0}\" was deleted", deletePart.Name);
            }
            return RedirectToAction("Index");
        }
    }
}