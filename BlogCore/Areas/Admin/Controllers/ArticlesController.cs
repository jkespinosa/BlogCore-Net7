using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogCore.Areas.Admin.Controllers
{
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

                    using (var fileStreams = new FileStream(Path.Combine(uploads, nameFile+extention), FileMode.Create))
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


        #region API Call

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new { data = _unitOfWork.Articles.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Articles.Get(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando categoria" });
            }

            _unitOfWork.Articles.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Categoria borrada orrectamente" });

        }
    }
    #endregion
}

