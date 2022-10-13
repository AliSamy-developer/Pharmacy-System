using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.ViewModels;

namespace Pharmacy.Controllers
{
    public class AccountsController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Name,Email,PhoneNumber,Password,ConfirmPassword")] SaveUserAccount model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var customerEmail = await _userManager.FindByEmailAsync(model.Email);
            if (customerEmail != null)
            {
                ViewBag.Error = "This email address is already in use";
                return View(model);
            }
            var customer = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(customer, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(customer, "Customer");
                return RedirectToAction("index");
            }
            else
            {
                foreach (var er in result.Errors)
                {
                    ModelState.AddModelError("", er.Description);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Email = await _userManager.FindByEmailAsync(model.Email);
            if (Email != null)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, model.Password, false, false);
                if (result.Succeeded)
                    return RedirectToAction("index","home");
                else
                {
                    ViewBag.LoginFaild = "Invalid Details";
                    return View(model);
                }
            }
            ViewBag.LoginFaild = "Invalid Details";
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

    }
}
