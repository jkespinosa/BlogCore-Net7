using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {         

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider model)
        {
            if (ModelState.IsValid)
            {
                string pathPrincipal = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
               
                    string nameFile = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(pathPrincipal, @"images\sliders");
                    var extention = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, nameFile + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                model.UrlImage = @"\images\sliders\" + nameFile + extention;

                    _unitOfWork.Sliders.Add(model);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
           
            if (id != null)
            {
                var slider = _unitOfWork.Sliders.Get(id.GetValueOrDefault());
                return View(slider);

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider model)
        {
            if (ModelState.IsValid)
            {
                string pathPrincipal = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var sliderFromDB = _unitOfWork.Sliders.Get(model.Id);



                if (files.Count() > 0)
                {
                    string nameFile = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(pathPrincipal, @"images\sliders");
                    var extention = Path.GetExtension(files[0].FileName);
                    var NewExtension = Path.GetExtension(files[0].FileName);

                    var pathImagen = Path.Combine(pathPrincipal, sliderFromDB.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(pathImagen))
                    {
                        System.IO.File.Delete(pathImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, nameFile + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    model.UrlImage = @"\images\sliders\" + nameFile + extention;

                    _unitOfWork.Sliders.Update(model);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
                else
                {
                    model.UrlImage = sliderFromDB.UrlImage;


                }

                _unitOfWork.Sliders.Update(model);
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }

            return View();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var sliderFromDB = _unitOfWork.Sliders.Get(id);
      

        

            if (sliderFromDB == null)
            {
                return Json(new { success = false, message = "Error borrando slider" });
            }

        

            _unitOfWork.Sliders.Remove(sliderFromDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "slider borrado correctamente" });

        }


        #region API Call

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new { data = _unitOfWork.Sliders.GetAll() });
        }


    }
    #endregion
}

