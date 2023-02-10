using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DAL.Repositories.Contracts;
using Voxo.BLL.ViewModels;
using Voxo.BLL.ViewModels.BlogVM;
using Voxo.DAL.Entities;

namespace Voxo.BLL.Services.Contracts
{
    public interface IBlogService : IRepository<Blog>
    {
        Task<List<BlogsVM>> GetBlogs();
        Task CreateBlog(BlogCreateVM model);
    }
}
