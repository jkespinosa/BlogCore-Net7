using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BlogCore.DataAccess.Data.Repository
{
    public class ArticleReposotiry : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticleReposotiry(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Article article)
        {
            var objDb = _db.Articles.FirstOrDefault(s => s.Id == article.Id);
            objDb.Name = article.Name;
            objDb.Description = article.Description;
            objDb.UrlImage = article.UrlImage;
            objDb.CategoryId = article.CategoryId;


          //  _db.SaveChanges();
        }

   
    }
}
