using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;

namespace Voxo_Project.ViewComponents
{
    public class BlogsSliderViewComponent : ViewComponent
    {    
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlogsSliderViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _dbContext.Blogs
                .Where(blog => !blog.Published)
            .ToListAsync();

            var model = _mapper.Map<List<BlogPageVM>>(blogs);

            return View(model);
        }
    }
}
