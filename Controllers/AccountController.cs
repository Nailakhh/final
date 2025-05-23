using Dewi.DAL;
using Dewi.Helpers.Enums;
using Dewi.Models;
using Dewi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dewi.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserApp> _userManager;

        public AccountController(AppDbContext context,SignInManager<UserApp> signInManager,
            RoleManager<IdentityRole> roleManager,
            UserManager<UserApp> userManager)
        {
            _context = context;
            _signInManager = signInManager;
           _roleManager = roleManager;
            _userManager = userManager;
        }

       
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task <IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            UserApp user = new UserApp()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username
            };
          var result= await _userManager.CreateAsync(user,registerVM.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }

            //await _userManager.AddToRoleAsync(user,UserRoles.Admin.ToString());
            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());

            await _context.SaveChangesAsync();

            return RedirectToAction("Login");

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]  
        public async Task <IActionResult> Login(LoginVM loginVM,string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserApp user=await _userManager.FindByEmailAsync(loginVM.EmailorUsername)
                ?? await _userManager.FindByNameAsync(loginVM.EmailorUsername);

            if (user == null)
            {
                return BadRequest();
            }
            var result=await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password,true);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "bele user yoxdur");
                return View();
            }
            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "LockOuta dusunuz ");
                return View();
            }
            await _signInManager.SignInAsync(user, loginVM.RememberME);
            if(ReturnUrl != null)
            {
                return RedirectToAction(ReturnUrl);
            }


            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach(var role in Enum.GetValues(typeof(UserRoles)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = role.ToString(),
                });
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
