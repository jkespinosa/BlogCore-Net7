using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUSer>
    {
        void BlockUser(string IdUser);
        void UnBlockUser(string IdUser);

    }
}
