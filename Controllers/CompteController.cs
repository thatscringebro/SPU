using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPU.Domain.Entites;
using SPU.ViewModels;

namespace SPU.Controllers
{
    [Authorize]
    public class CompteController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public CompteController(UserManager<Utilisateur> userManager, SignInManager<Utilisateur> signInManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;

        }


        [AllowAnonymous]
        public IActionResult LogIn(string? returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LogoutSuccessMessage = TempData["LogoutSuccessMessage"] as string;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(ConnexionVM vm, string? returnUrl = "")
        {
			if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }

            try
            {
                var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Pwd, vm.RememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Échec de connexion. Veuillez essayer de nouveau.");
                    ViewBag.ReturnUrl = returnUrl;
                    return View(vm);
                }

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Échec de connexion. Veuillez essayer de nouveau.");
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["LogoutSuccessMessage"] = "Déconnexion réussi";
            return RedirectToAction(nameof(LogIn));
        }
        //CRUD pour utilisateur 
        [Authorize(Roles = "Coordonateur")]
        public async Task<IActionResult> Manage(bool success = false, string actionType = "")
        {
            var vm = new List<UtilisateurDetailVM>();

            try
            {
                foreach (var user in _userManager.Users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    vm.Add(new UtilisateurDetailVM
                    {
                        role = userRoles.SingleOrDefault(),
                        Prenom = user.Prenom,
                        Nom = user.Nom
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.UserListErrorMessage = "Erreur d'affichage des utilisateurs. Veuillez réessayé." + ex.Message;
            }

            if (success)
            {
                if (actionType == "Remove")
                {
                    ViewBag.RemoveSuccessMessage = "L'utilisateur est retiré.";
                }
                else if (actionType == "Create")
                {
                    ViewBag.CreateSuccessMessage = "L'utilisateur est créer";
                }
            }

            return View(vm);
        }

        [Authorize(Roles = "Coordinateur")]
        [HttpGet]
        public IActionResult ChoisirRole(string role)
        {
           
            ViewBag.SelectedRole = role;

            return View();
        }

        //[Authorize(Roles = "Coordinateur")]
        //[HttpPost]
        //public async Task<IActionResult> Creation(UtilisateurCreationVM vm, string selectedRole)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }

        //    var roles = await _roleManager.Roles.ToListAsync();

        //    var RoleChoisi = roles.FirstOrDefault(r => r.Name == selectedRole);

        //    if (selectedRole == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid role selected. Please try again.");
        //        return View(vm);
        //    }

        //    var existingUser = await _userManager.FindByNameAsync(vm.Nom);
        //    if (existingUser != null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Username already exists. Please choose a different username.");
        //        return View(vm);
        //    }

        //    var toCreate = new Utilisateur(vm.userName);
        //    var result = await _userManager.CreateAsync(toCreate, vm.userName);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError(string.Empty, "User creation failed. Please try again.");
        //        return View(vm);
        //    }

        //    result = await _userManager.AddToRoleAsync(toCreate, RoleChoisi);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Unable to add user to the role {RoleChoisi}. Please try again.");
        //        return View(vm);
        //    }

        //    return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        //}

        //    [Authorize(Roles = "Administrator")]
        //    [HttpPost]
        //    public async Task<IActionResult> Remove(Guid id)
        //    {
        //        var user = await _userManager.FindByIdAsync(id.ToString());
        //        _asserts.Exists(user, "User not found. Please try again.");

        //        var result = await _userManager.DeleteAsync(user!);

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError(string.Empty, "Unable to remove user. Please try again or call emergency services if you are in danger.");
        //            return View();
        //        }

        //        return RedirectToAction(nameof(Manage), new { success = true, actionType = "Remove" });
        //    }
        //}


    }
}
