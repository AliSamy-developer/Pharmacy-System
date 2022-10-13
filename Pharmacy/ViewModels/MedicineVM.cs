using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Pharmacy.Models;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels
{
    public class MedicineVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TabletsNumber { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Categories { get; set; }
    }
}
