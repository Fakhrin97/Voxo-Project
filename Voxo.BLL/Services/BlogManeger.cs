using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DAL.Repositories;
using University.DAL.Repositories.Contracts;
using Voxo.BLL.Data;
using Voxo.BLL.Services.Contracts;
using Voxo.BLL.ViewModels;
using Voxo.BLL.ViewModels.BlogVM;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;




namespace Voxo.BLL.Services
{
    public class BlogManeger : EfCoreRepository<Blog>, IBlogService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Blog> _blogRepository;
        private readonly AppDbContext _dbContext;

       

        public BlogManeger(AppDbContext dbContext, IMapper mapper, IRepository<Blog> blogRepository) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _blogRepository = blogRepository;
        }

        public Task CreateBlog(BlogCreateVM model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BlogsVM>> GetBlogs()
        {
            var blogs = await GetAllAsync();
            var blogVM = _mapper.Map<List<BlogsVM>>(blogs);

            return blogVM;
        }

       
    }
}
