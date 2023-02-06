using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BrandsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _dbContext.Brands
                .ToListAsync();

            var brandsVM = _mapper.Map<List<BrandVM>>(brands);

            return View(brandsVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            var existBrand = await _dbContext.Brands
                .Where(brand=>brand.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existBrand != null)
            {
                ModelState.AddModelError("Name", "The brand with this name is already available");
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

            var unicalName = await model.Image.GenerateFile(Constants.BrandPath);

            await _dbContext.Brands.AddAsync(new Brand
            {
                ImageUrl = unicalName,
                Name = model.Name,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var brand = await _dbContext.Brands
                .Where(brand => brand.Id == id)
                .FirstOrDefaultAsync();

            if (brand == null) return NotFound();

            var brandsUpdateVM = _mapper.Map<BrandUpdateVM>(brand);

            return View(brandsUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BrandUpdateVM model)
        {           

            if (id == null) return NotFound();

            var existBrand = await _dbContext.Brands
                .Where(brand => brand.Id == id)
            .FirstOrDefaultAsync();

            if (existBrand == null) return NotFound();            

            var viewModel = new BrandUpdateVM
            {
                ImageUrl = existBrand.ImageUrl,
            };

            if (!ModelState.IsValid) return View(viewModel);

            if (existBrand.Name != model.Name)
            {
                var sameNameBrand = await _dbContext.Brands
                .Where(brand => brand.Name == model.Name)
            .FirstOrDefaultAsync();

                if (sameNameBrand != null)
                {
                    ModelState.AddModelError("", "The category with this name is already available");
                    return View(viewModel);
                }
            }

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-brand", existBrand.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.BrandPath);

                existBrand.ImageUrl = unicalName;
            }

            existBrand.Published = model.Published;
            existBrand.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existBrand = await _dbContext.Brands
                .Where(brand => brand.Id == id)
                .FirstOrDefaultAsync();

            if (existBrand == null) return NotFound();

            _dbContext.Brands.Remove(existBrand);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-brand", existBrand.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
