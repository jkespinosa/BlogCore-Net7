using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository Category { get; }
        IArticleRepository Articles { get; }
        ISliderRepository Sliders { get; }
        IUserRepository Users { get; }

        void Save();
    }
}
