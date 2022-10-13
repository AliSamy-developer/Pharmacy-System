using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Pharmacy.Models;

namespace Pharmacy.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        [ValidateNever]
        public ICollection<Medicine> Medicines { get; set; }
    }
}
