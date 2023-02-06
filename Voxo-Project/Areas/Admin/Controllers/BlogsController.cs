﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.BLL.ViewModels.BlogVM;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class BlogsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlogsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs
                .ToListAsync();

            var blogVM = _mapper.Map<List<BlogsVM>>(blogs);

            return View(blogVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            var existBlog = await _dbContext.Blogs
                .Where(blog => blog.Title == model.Title)
                .FirstOrDefaultAsync();

            if (existBlog != null)
            {
                ModelState.AddModelError("Name", "The brand with this title is already available");
                return View();
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Shekil Secmelisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            await _dbContext.Blogs.AddAsync(new Blog
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Content = model.Content,
                CreatedAt = DateTime.Now,
                CreatedBy = "Fakhrin Aliyev",
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            if (blog == null) return NotFound();

            var blogUpdateVM = _mapper.Map<BlogUpdateVM>(blog);

            return View(blogUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            if (id == null) return NotFound();

            var existBlog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
            .FirstOrDefaultAsync();

            if (existBlog == null) return NotFound();

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View();
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View();
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog", existBlog.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

                existBlog.ImageUrl = unicalName;
            }

            existBlog.Published = model.Published;
            existBlog.Title = model.Title;
            existBlog.Content = model.Content;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existBlog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            if (existBlog == null) return NotFound();

            _dbContext.Blogs.Remove(existBlog);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog", existBlog.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
