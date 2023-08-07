/*
using ImageCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var images = await _dbContext.Images.ToListAsync();
            return View(images);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Image model, IFormFile file)
        {
            if (ModelState.IsValid && file != null && file.Length > 0)
            {
                model.FileName = await SaveImage(file);
                _dbContext.Images.Add(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Image model, IFormFile file)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        // Delete old image file
                        DeleteImage(model.Id);

                        // Save new image file
                        model.FileName = await SaveImage(file);
                    }

                    _dbContext.Update(model);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            // Delete image file
            DeleteImage(id);

            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteImage(int id)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            var fileName = _dbContext.Images.AsNoTracking().FirstOrDefault(x => x.Id == id);

            var filePath = Path.Combine(uploadsFolder, fileName.FileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private bool ImageExists(int id)
        {
            return _dbContext.Images.Any(e => e.Id == id);
        }
    }
}
*/


using ImageCrud.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var images = (await _dbContext.Images.ToListAsync()).Where(x => !x.IsDelete).ToList();
            return View(images);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Image model, IFormFile file)
        {
            if (ModelState.IsValid && file != null && file.Length > 0)
            {
                model.FileName = await SaveImage(file);
                _dbContext.Images.Add(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Image model, IFormFile file)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        // Delete old image file
                        DeleteImage(model.Id);

                        // Save new image file
                        model.FileName = await SaveImage(file);
                    }

                    _dbContext.Update(model);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            // Delete image file
            DeleteImage(image.Id);
            image.IsDelete=true;

            _dbContext.Images.Update(image);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteImage(int id)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            var fileName = _dbContext.Images.AsNoTracking().FirstOrDefault(x => x.Id == id);

            var filePath = Path.Combine(uploadsFolder, fileName.FileName);



            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private bool ImageExists(int id)
        {
            return _dbContext.Images.Any(e => e.Id == id);
        }
    }
}
