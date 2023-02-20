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
    public class SliderReposotiry : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderReposotiry(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Slider slider)
        {
            var objDb = _db.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            objDb.Name = slider.Name;
            objDb.State = slider.State;
            objDb.UrlImage = slider.UrlImage;


        }

    }
}
