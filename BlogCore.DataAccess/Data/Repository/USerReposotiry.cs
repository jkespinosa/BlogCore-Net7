using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class UserReposotiry : Repository<ApplicationUSer>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserReposotiry(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void BlockUser(string IdUser)
        {
            var userFromDb = _db.ApplicationUSers.FirstOrDefault(u =>u.Id == IdUser);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void UnBlockUser(string IdUser)
        {
            var userFromDb = _db.ApplicationUSers.FirstOrDefault(u => u.Id == IdUser);
            userFromDb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
