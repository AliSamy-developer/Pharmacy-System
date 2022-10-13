using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using Pharmacy.Services;
using Pharmacy.ViewModels;

namespace Pharmacy.Controllers
{
    [Route("Medicines")]
    public class MedicinesController : Controller
    {
        private readonly IGenericServices<Medicine> _medService;
        private readonly IGenericServices<Category> _deptService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public MedicinesController(IGenericServices<Medicine> medService, IMapper mapper,
            IWebHostEnvironment env, IGenericServices<Category> deptService)
        {
            _medService = medService;
            _mapper = mapper;
            _env = env;
            _deptService = deptService;
        }
        [HttpGet("index")]

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("medicinesForPharmacy")]
        public ActionResult ListMedicinesForPharmacy()
        {
            return View();
        }
        [HttpGet("listOfMedicineForCustomer")]
        public async Task<ActionResult> ListMedicinesForCustomer()
        {
            return View(await _medService.ListAllAsync());
        }
        [HttpGet("create")]
        public async Task<ActionResult> Create()
        {
            ViewBag.AllDepartments = await _deptService.ListAllAsync();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost("create")]
        public async Task<ActionResult> Create([Bind("Id,Name,TabletsNumber,Price,Description,ImageFile,CategoryId")] MedicineVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = ProcessUploadedFile(model);
                }
                var medicine = _mapper.Map<Medicine>(model);
                var result = await _medService.Create(medicine);
                if (result)
                    return RedirectToAction("ListMedicines");
            }
            ViewBag.AllDepartments = _deptService.ListAllAsync();
            return View(model);
        }
        [HttpGet("edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var medicine = await _medService.GetAsync(id);
            if (medicine == null)
                return NotFound();
            ViewBag.AllDepartments = await _deptService.ListAllAsync();
            return View(_mapper.Map<MedicineVM>(medicine));
        }
        [ValidateAntiForgeryToken]
        [HttpPost("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,TabletsNumber,Price,Description,ImageFile,ImageUrl,CategoryId")] MedicineVM model)
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
                var medicine = _mapper.Map<Medicine>(model);
                var result = await _medService.Update(medicine);
                if (result)
                    return RedirectToAction("ListMedicines");
            }
            return View(model);
        }
        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var medicine = await _medService.GetAsync(m=>m.Id==id,source=>source.Include(m=>m.Category));
            return View(medicine);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var medicine = await _medService.GetAsync(id);
            if (medicine == null)
                return NotFound();
            if (medicine.ImageUrl != null)
            {
                string filePath = Path.Combine(_env.WebRootPath,
                            "images", medicine.ImageUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            var result = await _medService.Delete(medicine);
            if (result)
                return Ok();
            return BadRequest();

        }
        private string ProcessUploadedFile(MedicineVM model)
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

