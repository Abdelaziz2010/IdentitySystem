using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySystem.Controllers
{
    public class AccountController : Controller
    {
        #region UserManager<IdentityUser> vs SignInManager<IdentityUser>
        /* UserManager<IdentityUser>:-
         * 1.Create User
         * 2.Update User
         * 3.Delete User
         * SignInManager<IdentityUser>:-
         * 1.Signin
         * 2.Signout
         * 3.IsSignedIn
         */
        #endregion

        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager)
        {
            this.signInManager = _signInManager;
            this.userManager = _userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if(ModelState.IsValid)
            {
                //1. copy data from RegisterViewModel to IdentityUser

                IdentityUser user = new IdentityUser()
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };

                //2. store user in DB using UserManager class

                var result = await userManager.CreateAsync(user,userModel.Password);   //wait until user is created

                //3. Process ? Successed or Fail

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");   //redirect to Home page 

                    //return RedirectToAction("Login", "Account");   //in case you want to redirect Login page 
                }


                //4. in case of any error in registerations

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult>Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    await signInManager.PasswordSignInAsync(userModel.Email,userModel.Password,userModel.RememberMe,false);

                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", "Home");
                    
                }
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            }
            return View(userModel);
        }

    }
}
