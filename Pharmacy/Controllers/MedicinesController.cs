using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
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
        private readonly ApplicationDbContext _context;

        public MedicinesController(IGenericServices<Medicine> medService, IMapper mapper,
            IWebHostEnvironment env, IGenericServices<Category> deptService, ApplicationDbContext context)
        {
            _medService = medService;
            _mapper = mapper;
            _env = env;
            _deptService = deptService;
            _context = context;
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
        public async Task<ActionResult> ListMedicinesForCustomer(string? search, string? sortType,
            string? sortOrder, int pageSize = 3,int pageNumber = 1)
        {
            IQueryable<Medicine> medicines = _context.medicines.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                medicines = _context.medicines.Where(m => m.Name.Contains(search)); 
            }
            if (!string.IsNullOrEmpty(sortType) && !string.IsNullOrEmpty(sortOrder))
            {
                if (sortType == "Name")
                {
                    if (sortOrder == "asc")
                        medicines = medicines.OrderBy(x => x.Name);
                    else if (sortOrder == "desc")
                        medicines = medicines.OrderByDescending(x => x.Name);
                }
                else if(sortType == "Price")
                {
                    if (sortOrder == "asc")
                    {
                        medicines = medicines.OrderBy(x => x.Price);
                    }
                    else if (sortOrder == "desc")
                        medicines = medicines.OrderByDescending(x => x.Price);
                }
            }
            medicines = medicines.Skip(pageSize *(pageNumber - 1)).Take(pageSize);
            double pageCount = (double)((decimal)this._context.medicines.Count() / Convert.ToDecimal(pageSize));
            ViewBag.CurrentSearch = search;
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageCount = pageCount;
            return View(medicines);
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
                    return RedirectToAction("ListMedicinesForPharmacy");
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
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,TabletsNumber,Price,Description,ImageFile,ImageUrl,CategoryId")] EditMedicineVM model)
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
                    model.ImageUrl = EditProcessUploadedFile(model);

                }
                var medicine = _mapper.Map<Medicine>(model);
                var result = await _medService.Update(medicine);
                if (result)
                    return RedirectToAction("ListMedicinesForPharmacy");
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
        private string EditProcessUploadedFile(EditMedicineVM model)
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

