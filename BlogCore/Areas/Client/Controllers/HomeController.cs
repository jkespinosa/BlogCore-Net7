using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            HomeVM homevm = new HomeVM()
            {
                Slider = _unitOfWork.Sliders.GetAll(),
                ArticlesList= _unitOfWork.Articles.GetAll()

            };

            ViewBag.IsHome = true;

            return View(homevm);
        }

        public IActionResult Details(int id)
        {
            var articleFromDb = _unitOfWork.Articles.Get(id);
            return View(articleFromDb);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}