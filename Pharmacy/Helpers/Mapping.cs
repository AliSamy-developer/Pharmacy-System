using AutoMapper;
using Pharmacy.Models;
using Pharmacy.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Pharmacy.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<MedicineVM, Medicine>().ReverseMap();
            CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<ApplicationUser, SaveUserAccount>().ForMember(m => m.Email, s => s.MapFrom(st => st.UserName)).ReverseMap();

        }
    }
}
