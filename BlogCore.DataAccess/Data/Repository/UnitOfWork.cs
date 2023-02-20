using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryReposotiry(_db);
            Articles = new ArticleReposotiry(_db);
            Sliders = new SliderReposotiry(_db);

        }

        public ICategoryRepository Category { get; private set; }
        public IArticleRepository Articles { get; private set; }
        public ISliderRepository Sliders { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
