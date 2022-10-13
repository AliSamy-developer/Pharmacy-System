﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using Pharmacy.Services;
using Pharmacy.ViewModels;

namespace Pharmacy.Controllers
{
    [Route("Categories")]
    public class CategoriesController : Controller
    {
        private readonly IGenericServices<Medicine> _medService;
        private readonly IGenericServices<Category> _cateService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(IGenericServices<Medicine> medService,
            IGenericServices<Category> cateService, IMapper mapper, IWebHostEnvironment env)
        {
            _medService = medService;
            _cateService = cateService;
            _mapper = mapper;
            _env = env;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("ListOfCategories")]
        public async Task<ActionResult> ListCategories()
        {
            return View(await _cateService.ListAllAsync());
        }
        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [Bind("Id,Name,ImageFile")] CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = ProcessUploadedFile(model);
                }
                var category = _mapper.Map<Category>(model);
                var result = await _cateService.Create(category);
                if (result)
                    return RedirectToAction("ListCategories");
            }
            return View(model);
        }
        [HttpGet("edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _cateService.GetAsync(id);
            if (category == null)
                return NotFound();
            return View(_mapper.Map<CategoryVM>(category));
        }
        [ValidateAntiForgeryToken]
        [HttpPost("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,ImageFile,ImageUrl")] CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return NotFound();
                if (model.ImageFile != null)
                {
                    string filePath = Path.Combine(_env.WebRootPath,
                            "images", model.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    model.ImageUrl = ProcessUploadedFile(model);

                }
                var category = _mapper.Map<Category>(model);
                var result = await _cateService.Update(category);
                if (result)
                    return RedirectToAction("ListCategories");
            }
            return View(model);
        }
        [HttpGet("medicineOfCategory/{id}")]
        public async Task<ActionResult> GetMedicineOfCategory(int id)
        {
            var category = await _cateService.GetAsync(t => t.Id == id, source => source.Include(s => s.Medicines));
            if (category == null)
                return NotFound();
            return View(category);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _cateService.GetAsync(id);
            if (category == null)
                return NotFound();
            if (category.ImageUrl != null)
            {
                string filePath = Path.Combine(_env.WebRootPath,
                            "images", category.ImageUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            var result = await _cateService.Delete(category);
            if (result)
                return Ok();
            return BadRequest();

        }
        private string ProcessUploadedFile(CategoryVM model)
        {
            string uniqueFileName = null;
            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }
    }
}

