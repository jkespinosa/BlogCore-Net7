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
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticlesController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
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
            ArticlesVM artvm = new ArticlesVM()
            {
                Article = new BlogCore.Models.Article(),
                CategoryList = _unitOfWork.Category.GetListAllCategories()
            };

            return View(artvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticlesVM model)
        {
            if (ModelState.IsValid)
            {
                string pathPrincipal = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (model.Article.Id == 0)
                {
                    string nameFile = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(pathPrincipal, @"images\articles");
                    var extention = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, nameFile + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    model.Article.UrlImage = @"\images\articles\" + nameFile + extention;
                    model.Article.DateCreate = DateTime.Now.ToString();

                    _unitOfWork.Articles.Add(model.Article);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
            }

            model.CategoryList = _unitOfWork.Category.GetListAllCategories();
            return View(model);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticlesVM artvm = new ArticlesVM()
            {
                Article = new BlogCore.Models.Article(),
                CategoryList = _unitOfWork.Category.GetListAllCategories()
            };

            if (id != null)
            {
                artvm.Article = _unitOfWork.Articles.Get(id.GetValueOrDefault());
            }

            return View(artvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticlesVM model)
        {
            if (ModelState.IsValid)
            {
                string pathPrincipal = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var articleFromDB = _unitOfWork.Articles.Get(model.Article.Id);



                if (files.Count() > 0)
                {
                    string nameFile = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(pathPrincipal, @"images\articles");
                    var extention = Path.GetExtension(files[0].FileName);
                    var NewExtension = Path.GetExtension(files[0].FileName);

                    var pathImagen = Path.Combine(pathPrincipal, articleFromDB.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(pathImagen))
                    {
                        System.IO.File.Delete(pathImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, nameFile + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    model.Article.UrlImage = @"\images\articles\" + nameFile + extention;
                    model.Article.DateCreate = DateTime.Now.ToString();

                    _unitOfWork.Articles.Update(model.Article);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
                else
                {
                    model.Article.UrlImage = articleFromDB.UrlImage;


                }

                _unitOfWork.Articles.Update(model.Article);
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }

            model.CategoryList = _unitOfWork.Category.GetListAllCategories();
            return View(model);

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var articleFromDB = _unitOfWork.Articles.Get(id);
            string pathPrincipal = _hostingEnvironment.WebRootPath;
            var pathImagen = Path.Combine(pathPrincipal, articleFromDB.UrlImage.TrimStart('\\'));

            if (System.IO.File.Exists(pathImagen))
            {
                System.IO.File.Delete(pathImagen);
            }

            var objFromDb = _unitOfWork.Articles.Get(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando articulo" });
            }

        

            _unitOfWork.Articles.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "articulo borrada correctamente" });

        }


        #region API Call

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new { data = _unitOfWork.Articles.GetAll(includeProperties: "Categories") });
        }


    }
    #endregion
}

