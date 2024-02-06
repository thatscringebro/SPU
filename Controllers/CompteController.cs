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

		[AllowAnonymous]
		[HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [Authorize(Roles = "Coordinateur")]
        [HttpGet]
        public IActionResult ChoisirRole(string role)
        {
           
            ViewBag.SelectedRole = role;

            return View();
        }

        [Authorize(Roles = "Coordinateur")]
        [HttpPost]
        public async Task<IActionResult> CreationNormal(UtilisateurCreationVM vm)
        {
            //CREATION POUR TOUS LES USER SAUF ENTREPRISE ET MDS
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == ViewBag.SelectedRole);

            if (selectedRole == null)
            {
                ModelState.AddModelError(string.Empty, "Rôle invalide. Veuillez réessayer.");
                return View(vm);
            }

            var existingUser = await _userManager.FindByNameAsync(vm.Nom);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Nom d'utilisateur déjà utilisé. Veuillez utiliser un autre nom d'utilisateur.");
                return View(vm);
            }


            var toCreate = new Utilisateur(vm.Nom);
            var result = await _userManager.CreateAsync(toCreate, vm.pwd);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Création d'utilisateur échoué. Veuillez réessayer.");
                return View(vm);
            }

            result = await _userManager.AddToRoleAsync(toCreate, selectedRole.Name);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Impossible d'ajouter le rôle {selectedRole.Name}. Veuillez réessayer.");
                return View(vm);
            }

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }

        [Authorize(Roles = "Coordinateur")]
        [HttpPost]
        public async Task<IActionResult> CreationMDS(MDSCreationVM vm)
        {
            //CREATION POUR MDS
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == ViewBag.SelectedRole);

            if (selectedRole == null)
            {
                ModelState.AddModelError(string.Empty, "Rôle invalide. Veuillez réessayer.");
                return View(vm);
            }

            var existingUser = await _userManager.FindByNameAsync(vm.Nom);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Nom d'utilisateur déjà utilisé. Veuillez utiliser un autre nom d'utilisateur.");
                return View(vm);
            }

           

            var toCreate = new Utilisateur(vm.Nom);
            var result = await _userManager.CreateAsync(toCreate, vm.pwd);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Création d'utilisateur échoué. Veuillez réessayer.");
                return View(vm);
            }

            result = await _userManager.AddToRoleAsync(toCreate, selectedRole.Name);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Impossible d'ajouter le rôle {selectedRole.Name}. Veuillez réessayer.");
                return View(vm);
            }

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }


        [Authorize(Roles = "Coordinateur")]
        [HttpPost]
        public async Task<IActionResult> CreationEntreprise(EntrepriseCreationVM vm)
        {
            //CREATION POUR ENTREPRISE 
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == ViewBag.SelectedRole);

            if (selectedRole == null)
            {
                ModelState.AddModelError(string.Empty, "Rôle invalide. Veuillez réessayer.");
                return View(vm);
            }

            var existingUser = await _userManager.FindByNameAsync(vm.Nom);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Nom d'utilisateur déjà utilisé. Veuillez utiliser un autre nom d'utilisateur.");
                return View(vm);
            }


            var toCreate = new Utilisateur(vm.Nom);
            var result = await _userManager.CreateAsync(toCreate, vm.pwd);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Création d'utilisateur échoué. Veuillez réessayer.");
                return View(vm);
            }

            result = await _userManager.AddToRoleAsync(toCreate, selectedRole.Name);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Impossible d'ajouter le rôle {selectedRole.Name}. Veuillez réessayer.");
                return View(vm);
            }

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }

        [Authorize(Roles = "Coordinateur")]
     
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());


            if(user == null)
            {
                return NotFound();
            }
          
                var modifUser = new UtilisateurEditVM
                {
                    id = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    PhoneNumber = user.PhoneNumber,
                    userName = user.UserName

                };
     

            return View(modifUser);
        }

        [Authorize(Roles = "Coordinateur")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, UtilisateurEditVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            var aEditer = await _userManager.FindByIdAsync(id.ToString());
            if(aEditer == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            aEditer.Nom = vm.Nom;
            aEditer.Prenom = vm.Prenom;
            aEditer.PhoneNumber = vm.PhoneNumber;
            //COURRIEL ? 
            aEditer.UserName = vm.userName;

           var works = _userManager.UpdateAsync(aEditer);
            if(works != null) 
                TempData["SuccessMessage"] = "Modifications succeeded";
            else
                TempData["ErrorMessage"] = "Request failed";
           

            return RedirectToAction(nameof(Manage));
        }



        [Authorize(Roles = "Coordinateur")]
        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
         

            var result = await _userManager.DeleteAsync(user!);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer un utilisateur. Veuillez réessayer.");
                return View();
            }

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Remove" });
        }
    }


}

