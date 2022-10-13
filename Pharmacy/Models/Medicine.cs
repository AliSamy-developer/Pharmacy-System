using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        [MaxLength(30, ErrorMessage = "Name mustn't exceed  30 char")]
        public string Name { get; set; }
        public int TabletsNumber { get; set; }
        public double Price { get; set; }
        [MinLength(10, ErrorMessage = "Description mustn't be less than 10 char")]
        [MaxLength(500, ErrorMessage = "Description mustn't exceed  500 char")]
        public string Description { get; set; }
        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        [Display(Name = "Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid department")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
