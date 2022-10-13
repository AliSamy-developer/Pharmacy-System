using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;
using System.Linq.Dynamic.Core;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndPointsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EndPointsController(
            ApplicationDbContext context)
        {
            _context = context;
        }
        //[HttpPost]
        //public IActionResult GetMedicines()
        //{
        //    var medicens = _context.medicines.ToList();
        //    var recordTotal = medicens.Count();
        //    var jsonData = new { recordsFiltered = recordTotal, recordTotal, data = medicens };
        //    return Ok(jsonData);
        //}
        [HttpPost]
        public IActionResult GetMedicines()
        {
            // get records per page
            var pageSize = int.Parse(Request.Form["length"]);

            // get skiped records
            var skip = int.Parse(Request.Form["start"]);

            // get search term
            var searchTerm = Request.Form["search[value]"];

            // get order direction
            var orderDirection = Request.Form["order[0][dir]"];

            // get clicked column index to order
            var columnIndex = Request.Form["order[0][column]"];

            // get clicked column name to order
            var columnName = Request.Form["columns[" + columnIndex + "][name]"];

            // apply filtering with search term if it has value
            IQueryable<Medicine> medicines = _context.medicines.Where(m => string.IsNullOrEmpty(searchTerm) ||
                m.Name.Contains(searchTerm) ||
                m.Description.Contains(searchTerm) ||
                m.TabletsNumber.ToString().Contains(searchTerm) ||
                m.Price.ToString().Contains(searchTerm) ||
                m.Category.Name.ToString().Contains(searchTerm));


            // apply ordering
            if (!(string.IsNullOrEmpty(orderDirection) && string.IsNullOrEmpty(columnIndex) && string.IsNullOrEmpty(columnName)))
            {
                medicines = medicines.OrderBy($"{columnName} {orderDirection}");
            }

            // get total records count
            var recordsTotal = medicines.Count();

            // implements pagination
            var data = medicines.Skip(skip).Take(pageSize).ToList();

            // return json object as a result
            var jsonData = new
            {
                recordsFiltered = recordsTotal,
                recordsTotal,
                data
            };

            return Ok(jsonData);
        }
    }
}
