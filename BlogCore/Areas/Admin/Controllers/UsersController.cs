using BlogCore.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {//
           // return View(_unitOfWork.Users.GetAll());

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var CurrentUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_unitOfWork.Users.GetAll(u=> u.Id != CurrentUser.Value));

        }

        [HttpGet]
        public IActionResult Block(string id)
        {
            if (id == null)
            {
                return NotFound();

            }
            _unitOfWork.Users.BlockUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UnBlock(string id)
        {
            if (id == null)
            {
                return NotFound();

            }
            _unitOfWork.Users.UnBlockUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
