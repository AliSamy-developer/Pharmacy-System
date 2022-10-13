using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace Pharmacy.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        [ValidateNever]
        public ICollection<Medicine> Medicines { get; set; }
    }
}
