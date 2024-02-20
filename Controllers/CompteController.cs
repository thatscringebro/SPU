using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPU.Domain;
using SPU.Domain.Entites;
using SPU.Enum;
using SPU.Models;
using SPU.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging.Abstractions;
using Humanizer;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Drawing;

namespace SPU.Controllers
{
    [Authorize]
    public class CompteController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SpuContext _spuContext;

        public CompteController(SpuContext spuContext, UserManager<Utilisateur> userManager, SignInManager<Utilisateur> signInManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _spuContext = spuContext;

        }

        /// <summary>
        /// Méthode pour hasher le rôle de l'utilisateur pour le comparé au url
        /// </summary>
        /// <param name="strData">Rôle</param>
        /// <returns>Le nom hasher</returns>
        public string CreateSHA512(string strData)
        {
            var message = Encoding.UTF8.GetBytes(strData);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += string.Format("{0:x2}", x);
                }
                return hex;
            }
        }


        #region login
        /// <summary>
        /// Action pour se connecter
        /// </summary>
        /// <param name="returnUrl">Url de retour</param>
        /// <returns>la vue</returns>
        [AllowAnonymous]
        public IActionResult LogIn(string? returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LogoutSuccessMessage = TempData["LogoutSuccessMessage"] as string;

            return View();
        }

        /// <summary>
        /// Retourne le formulaire pour la confirmation des données
        /// </summary>
        /// <param name="vm">View Model de la connexion</param>
        /// <param name="returnUrl">url de retour</param>
        /// <returns>Redirige au bon endroit</returns>
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

                var user = _userManager.Users.FirstOrDefault(r => r.UserName == vm.UserName);
                var role = await _userManager.GetRolesAsync(user);
                var roleUser = role.FirstOrDefault();

                if (roleUser == "Coordonnateur")
                    return RedirectToAction(nameof(Manage));

                if (roleUser == "Enseignant")
                    return RedirectToAction(nameof(ManageEnseignant));

                if (roleUser == "Employeur")
                    return RedirectToAction(nameof(ManageMds));
                    
                //Redirection home ? ou horaire ? mais si les mds/stagiaire n'as pas encore d'horaire ? 

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

        /// <summary>
        /// Déconnexion de l'utilisateur
        /// </summary>
        /// <returns>Déconnecte l'utilisateur</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["LogoutSuccessMessage"] = "Déconnexion réussi";
            return RedirectToAction(nameof(LogIn));
        }

        #endregion

        #region Manage
        /// <summary>
        /// Donne un choix pour la création des utilisateurs pour le coordonnateur
        /// </summary>
        /// <returns>La vue choisi par le coordonnateur</returns>
        //[AllowAnonymous]
        [Authorize(Roles = "Coordonnateur")]
        public ActionResult ChoixCreation()
        {
            return View();
        }


        //CRUD pour utilisateur 
        /// <summary>
        /// Affiche les listes des utilisateurs pour la gestion des utilisateurs
        /// </summary>
        /// <param name="success"></param>
        /// <param name="actionType"></param>
        /// <returns>La vue manage</returns>
        //[AllowAnonymous] 
        [Authorize(Roles = "Coordonnateur")]
        public async Task<IActionResult> Manage(bool success = false, string actionType = "")
        {
            var vm = new List<UtilisateurDetailVM>();

            try
            {
                foreach (var user in await _userManager.Users.ToListAsync())
                {
                    foreach (var userRoles in await _userManager.GetRolesAsync(user))
                    {
                        var Mds = _spuContext.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);
                        var Stagiaire = _spuContext.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
                        var Employeur = _spuContext.Employeurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
                        var Enseignant = _spuContext.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);

                        var userDetail = new UtilisateurDetailVM
                        {
                            role = userRoles,
                            Id = user.Id,
                            userName = user.UserName,
                            Nom = user.Nom,
                            Prenom = user.Prenom,
                            Courriel = user.Email,
                            Telephone = user.PhoneNumber,
                        };

                        if (Stagiaire != null)
                        {
                            var Ecole = _spuContext.Ecole.FirstOrDefault(x => x.id == Stagiaire.EcoleId);
                            userDetail.PartagerInfoContact = Stagiaire?.PartagerInfoContact ?? false;

                            if (Stagiaire?.debutStage.HasValue == true)
                            {
                                userDetail.debutStage = Stagiaire.debutStage.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd"); ;
                                //Value.Date.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                userDetail.debutStage = null;
                            }

                            if (Stagiaire?.finStage.HasValue == true)
                            {
                                userDetail.finStage = Stagiaire.finStage.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd"); ;
                            }
                            else
                            {
                                userDetail.finStage = null;
                            }
                            userDetail.Ecole = Ecole.Nom;
                        }

                        if (Enseignant != null)
                        {
                            var Ecole = _spuContext.Ecole.FirstOrDefault(x => x.id == Enseignant.EcoleId);
                            userDetail.Ecole = Ecole.Nom;
                        }

                        if (Mds != null)
                        {
                            userDetail.actif = Mds?.actif ?? false;
                            userDetail.status = Mds.status;
                            userDetail.Matricule = Mds?.MatriculeId;
                            userDetail.civilite = Mds.civilite;
                            userDetail.NomEmployeur = Mds?.NomEmployeur;
                            userDetail.TypeEmployeur = Mds.typeEmployeur;
                            userDetail.telMaison = Mds?.telMaison;
                            userDetail.accreditation = Mds?.accreditation;
                            userDetail.commentaire = Mds?.commentaire;
                            userDetail.commentaireCIUSS = Mds?.commentaireCIUSS;
                        }

                        if (Employeur != null)
                        {
                            var Adresse = _spuContext.Adresses.FirstOrDefault(x => x.Id == Employeur.AdresseId);
                            if (Adresse != null)
                            {
                                userDetail.NumeroDeRue = Adresse.NoCivique;
                                userDetail.NomDeRue = Adresse.Rue;
                                userDetail.Ville = Adresse.Ville;
                                userDetail.Province = Adresse.Province;
                                userDetail.Pays = Adresse.Pays;
                                userDetail.CodePostal = Adresse.CodePostal;
                            }
                        }

                        vm.Add(userDetail);
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.UserListErrorMessage = "Erreur d'affichage des utilisateurs. Veuillez réessayer." + ex.Message;
            }

            //if (success)
            //{
            //    if (actionType == "Remove")
            //    {
            //        ViewBag.RemoveSuccessMessage = "L'utilisateur est retiré.";
            //    }
            //    else if (actionType == "Create")
            //    {
            //        ViewBag.CreateSuccessMessage = "L'utilisateur est créer";
            //    }
            //}
            return View(vm);
        }

        /// <summary>
        /// Retourne la liste des maîtres de stage pour les employeurs
        /// </summary>
        /// <returns>La vue avec la liste</returns>
        [Authorize(Roles = "Employeur")]
        public async Task<IActionResult> ManageMds()
        {
            var vm = new List<MdsDetailVM>();
            try
            {
                var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (idUser == null)
                {
                    ViewBag.UserListErrorMessage = "Erreur d'affichage. Veuillez réessayer!";
                }

                Guid idEmp = _spuContext.Employeurs.FirstOrDefault(a => a.UtilisateurId == Guid.Parse(idUser)).Id;


                foreach (var user in await _spuContext.MDS.Where(m => m.EmployeurId == idEmp).Include(u => u.utilisateur).ToListAsync())
                {
                    vm.Add(new MdsDetailVM
                    {
                        Id = user.Id,
                        Prenom = user.utilisateur.Prenom,
                        Nom = user.utilisateur.Nom
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.UserListErrorMessage = "Erreur d'affichage des maîtres de stages. Veuillez réessayer!" + ex.Message;
            }

            return View(vm);
        }

        /// <summary>
        /// Retourne la liste des stagiaires associé à l'enseignant
        /// </summary>
        /// <returns>Les stagiaires</returns>
        [Authorize(Roles = "Enseignant")]
        public async Task<IActionResult> ManageEnseignant()
        {
            var vm = new List<UtilisateurDetailVM>();
            try
            {
                var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (idUser == null)
                {
                    ViewBag.UserListErrorMessage = "Erreur d'affichage. Veuillez réessayer!";
                }

                Guid idEns = _spuContext.Enseignants.FirstOrDefault(a => a.UtilisateurId == Guid.Parse(idUser)).Id;


                foreach (var user in await _spuContext.Stagiaires.Where(m => m.EnseignantId == idEns).Include(u => u.utilisateur).ToListAsync())
                {
                    vm.Add(new UtilisateurDetailVM
                    {
                        Id = user.Id,
                        Prenom = user.utilisateur.Prenom,
                        Nom = user.utilisateur.Nom
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.UserListErrorMessage = "Erreur d'affichage des maîtres de stages. Veuillez réessayer!" + ex.Message;
            }

            return View(vm);
        }

        #endregion

        #region CreationNormal et EditNormal
        /// <summary>
        /// Affiche la vue pour le formulaire de création des stagiaires/enseignants
        /// </summary>
        /// <param name="vue"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Compte/CreationNormal")]
        [Route("Compte/CreationNormal/{hash?}")]
        [HttpGet]
        public ActionResult CreationNormal(string vue, string hash)
        {
            ViewBag.Ecoles = _spuContext.Ecole.Select(e => new SelectListItem
            {
                Value = e.id.ToString(),
                Text = e.Nom
            }).ToList();

            var stagiaire = CreateSHA512("Stagiaire");
            var enseignant = CreateSHA512("Enseignant");
            var Coordo = CreateSHA512("Coordonnateur");

            if (hash == null)
            {
                switch (vue)
                {
                    case "CreationCoordonnateur": //Creation coordo
                        return View("CreationCoordonnateur");
                    case "CreationEnseignant": //Creation enseignant
                        return View("CreationEnseignant");
                    default:
                        //Retourne STAGIAIRE si aucun choix
                        return View();
                }
            }
            else
            {
                if (hash == enseignant)
                    return View("CreationEnseignant");
                else if (hash == Coordo)
                    return View("CreationCoordonnateur");
                else
                    return View();
            }
        }

        private IEnumerable<SelectListItem> PopulateEcoles()
        {
            return _spuContext.Ecole.Select(e => new SelectListItem
            {
                Value = e.id.ToString(),
                Text = e.Nom
            }).ToList();
        }


        //CREATION STAGIAIRE/COORDO/ENSEIGNANT
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreationNormal(UtilisateurCreationVM vm, string hash)
        {

            ViewBag.Ecoles = _spuContext.Ecole.Select(e => new SelectListItem
            {
                Value = e.id.ToString(),
                Text = e.Nom
            }).ToList();


            /*
            //var stagiaire = CreateSHA512("Stagiaire");
            //var enseignant = CreateSHA512("Enseignant");
            //var Coordo = CreateSHA512("Coordonnateur");

            //if (hash == null)
            //{
            //    switch (vm.role)
            //    {
            //        case "Coordonnateur": 
            //            return View("CreationCoordonnateur");
            //        case "Enseignant": 
            //            return View("CreationEnseignant");
            //        default:
            //            //Retourne STAGIAIRE si aucun choix
            //            return View(vm);
            //    }
            //}
            //else
            //{

            //    if (hash == enseignant)
            //        return View("CreationEnseignant");
            //    else if (hash == Coordo)
            //        return View("CreationCoordonnateur");
            //    else
            //        return View();
            //} */

            ViewBag.Ecoles = PopulateEcoles();

            if (!ModelState.IsValid)
            {
                return View(vm);
                //RedirectToAction("CreationNormal", vm);
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == vm.role);

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


            var toCreate = new Utilisateur(vm.userName);

            toCreate.Prenom = vm.Prenom;
            toCreate.Nom = vm.Nom;
            toCreate.PhoneNumber = vm.PhoneNumber;
            toCreate.Email = vm.Email;

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
            var ecole = _spuContext.Ecole.Where(x => x.id == vm.idEcoleSelectionne).FirstOrDefault();

            if (selectedRole.Name == "Stagiaire")
            {
                var Chat = new Chat();

                var Stagiaire = new Stagiaire
                {
                    utilisateur = toCreate,
                    UtilisateurId = toCreate.Id,
                    ecole = ecole,
                    EcoleId = vm.idEcoleSelectionne,
                    chat = Chat,
                    ChatId = Chat.Id,
                    PartagerInfoContact = vm.PartagerInfoContact
                };

                _spuContext.Stagiaires.Add(Stagiaire);

            }
            else if (selectedRole.Name == "Coordonnateur")
            {
                var Coordo = new Coordonnateur
                {
                    UtilisateurId = toCreate.Id,
                    utilisateur = toCreate,
                    ecole = ecole,
                    EcoleId = vm.idEcoleSelectionne
                };
                _spuContext.Coordonnateurs.Add(Coordo);

            }
            else if (selectedRole.Name == "Enseignant")
            {
                var Enseignant = new Enseignant
                {
                    UtilisateurId = toCreate.Id,
                    utilisateur = toCreate,
                    ecole = ecole,
                    EcoleId = vm.idEcoleSelectionne
                };
                _spuContext.Enseignants.Add(Enseignant);

            }
            await _spuContext.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }

        //[AllowAnonymous]
        //EDIT STAGIAIRE/COORDO/ENSEIGNANT
        [Authorize(Roles = "Coordonnateur")]
        [HttpGet]
        public async Task<IActionResult> EditUtilisateur(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            var roleCh = await _userManager.GetRolesAsync(user);

            var modifUser = new UtilisateurEditVM
            {
                Nom = user.Nom,
                Prenom = user.Prenom,
                PhoneNumber = user.PhoneNumber,
                userName = user.UserName,
                role = roleCh.FirstOrDefault()
            };


            return View(modifUser);
        }

        //[AllowAnonymous]
        [Authorize(Roles = "Coordonnateur")]
        [HttpPost]
        public async Task<IActionResult> EditUtilisateur(Guid id, UtilisateurEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var aEditer = await _userManager.FindByIdAsync(id.ToString()) ?? throw new ArgumentOutOfRangeException(nameof(id));

            aEditer.Nom = vm.Nom;
            aEditer.Prenom = vm.Prenom;
            aEditer.PhoneNumber = vm.PhoneNumber;
            aEditer.Email = vm.Email;
            aEditer.UserName = vm.userName;

            var works = _userManager.UpdateAsync(aEditer);
            if (works != null)
                TempData["SuccessMessage"] = "Modifications succeeded";
            else
                TempData["ErrorMessage"] = "Request failed";



            await _spuContext.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }


        #endregion

        #region CreationMDS et EditMDS

        [AllowAnonymous]
        [Route("Compte/CreationMDS")]
        [Route("Compte/CreationMDS/{hash?}")]
        [HttpGet]
        public ActionResult CreationMDS(string hash)
        {
            ViewBag.Employeurs = _spuContext.Employeurs.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.utilisateur.UserName
            }).ToList();

            var MDS = CreateSHA512("MDS");

            if (hash == MDS)
                return View("CreationMDS");
            else
                return View(); // a changer avec le role du coordo quand on va être rendu au role !! 
        }

        //CREATION MDS
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreationMDS(MDSCreationVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == vm.role);

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

            var toCreate = new Utilisateur(vm.userName);
            toCreate.Prenom = vm.Prenom;
            toCreate.Nom = vm.Nom;
            toCreate.PhoneNumber = vm.PhoneNumber;
            toCreate.Email = vm.Email;
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

            var Entreprise = _spuContext.Employeurs.Where(x => x.Id == vm.idEmployeurSelectionne).Include(u => u.utilisateur).FirstOrDefault();
            //Nom utilisateur pour l'entreprise = nom d'entreprise OBLIGATOIRE

            Horaire nouvelleHoraire = new Horaire();

            var MDs = new MDS
            {
                utilisateur = toCreate,
                UtilisateurId = toCreate.Id,
                MatriculeId = vm.MatriculeId,
                civilite = vm.civilite,
                typeEmployeur = vm.TypeEmployeur,
                telMaison = vm.telMaison,
                NomEmployeur = Entreprise.utilisateur.UserName,
                EmployeurId = Entreprise.Id

            };



            nouvelleHoraire.Id = Guid.NewGuid();
            nouvelleHoraire.mds = MDs;
            nouvelleHoraire.MDSId = MDs.Id;
         
            // Obtenir la date et l'heure actuelles dans le fuseau horaire local
            DateTime debutHoraire = DateTime.Now;


            // Démarrer l'horaire à partir du dimanche prochain
            while (debutHoraire.DayOfWeek != DayOfWeek.Sunday)
            {
                debutHoraire = debutHoraire.AddDays(1);
            }

            debutHoraire = new DateTime(debutHoraire.Year, debutHoraire.Month, debutHoraire.Day, 0, 0, 0);

            // Ajouter deux ans
            DateTime finHoraire = debutHoraire.AddYears(2);

            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            debutHoraire = TimeZoneInfo.ConvertTime(debutHoraire, localTimeZone);
            finHoraire = TimeZoneInfo.ConvertTime(finHoraire, localTimeZone);

            MDs.DateCreationHoraire = debutHoraire.ToUniversalTime();
            MDs.DateExpiration = finHoraire.ToUniversalTime();

      

            _spuContext.Horaires.Add(nouvelleHoraire);
            _spuContext.Add(MDs);
            await _spuContext.SaveChangesAsync();
            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }



        //EDIT MDS
        [Authorize(Roles = "Coordonnateur")]
        [HttpGet]
        public async Task<IActionResult> EditMDS(Guid id)
        {
            ViewBag.Employeurs = _spuContext.Employeurs.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.utilisateur.UserName
            }).ToList();

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();


            var userMDS = await _spuContext.MDS.Where(x => x.UtilisateurId == user.Id).FirstOrDefaultAsync();

            if (userMDS == null)
                return NotFound();

            //var empId = await _spuContext.Employeurs.Where(userMDS.emp)

            var modifUser = new MDSEditVM
            {
                Id = userMDS.Id,
                userName = userMDS.utilisateur.UserName,
                Nom = userMDS.utilisateur.Nom,
                Prenom = userMDS.utilisateur.Prenom,
                PhoneNumber = userMDS.utilisateur.PhoneNumber,
                MatriculeId = userMDS.MatriculeId,
                Civilite = userMDS.civilite,
                //NomEmployeur = userMDS.NomEmployeur,
                telMaison = userMDS.telMaison,
                TypeEmployeur = userMDS.typeEmployeur,
                idEmployeurSelectionne = userMDS.EmployeurId,
                Email = userMDS.utilisateur.Email,

            };


            return View(modifUser);
        }



        [Authorize(Roles = "Coordonnateur")]
        [HttpPost]
        public async Task<IActionResult> EditMDS(Guid id, MDSEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var aEditer = await _userManager.FindByIdAsync(id.ToString()) ?? throw new ArgumentOutOfRangeException(nameof(id));
            var MdsaEditer = _spuContext.MDS.Where(x => x.UtilisateurId == aEditer.Id).FirstOrDefault();

            aEditer.Nom = vm.Nom;
            aEditer.Prenom = vm.Prenom;
            aEditer.PhoneNumber = vm.PhoneNumber;
            aEditer.Email = vm.Email;
            aEditer.UserName = vm.userName;

            MdsaEditer.utilisateur = aEditer;

            MdsaEditer.MatriculeId = vm.MatriculeId;
            MdsaEditer.civilite = vm.Civilite;
            //MdsaEditer.NomEmployeur = vm.NomEmployeur;
            MdsaEditer.EmployeurId = vm.idEmployeurSelectionne;
            MdsaEditer.telMaison = vm.telMaison;
            MdsaEditer.typeEmployeur = vm.TypeEmployeur;


            var works = await _userManager.UpdateAsync(aEditer);

            if (works != null)
            {
                TempData["SuccessMessage"] = "Modifications succeeded";

                _spuContext.MDS.Update(MdsaEditer);
                await _spuContext.SaveChangesAsync();

                return RedirectToAction(nameof(Manage));
            }
            else
            {
                TempData["ErrorMessage"] = "Request failed";
                return View(vm);
            }

        }

        #endregion

        #region CreationEmployeur et EditEmployeur
        [AllowAnonymous]
        [Route("Compte/CreationEmployeur")]
        [Route("Compte/CreationEmployeur/{hash?}")]
        [HttpGet]
        public ActionResult CreationEmployeur(string hash)
        {
            var Employeur = CreateSHA512("Employeyr");

            if (hash == Employeur)
                return View("CreationEmployeur");
            else
                return View(); // a changer avec le role du coordo quand on va être rendu au role !! 
        }

        //[Authorize(Roles = "Coordinateur")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreationEmployeur(EmployeurCreationVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);


            var roles = await _roleManager.Roles.ToListAsync();

            var selectedRole = roles.FirstOrDefault(r => r.Name == vm.role);

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


            var toCreate = new Utilisateur(vm.userName);
            toCreate.Prenom = vm.Prenom;
            toCreate.Nom = vm.Nom;
            toCreate.PhoneNumber = vm.PhoneNumber;
            toCreate.Email = vm.Email;
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

            var ToAddadresse = new Adresse
            {
                Ville = vm.ville,
                Pays = vm.pays,
                Province = vm.province,
                CodePostal = vm.codePostal,
                NoCivique = vm.NumeroDeRue,
                Rue = vm.NomDeRue
            };
            _spuContext.Adresses.Add(ToAddadresse);
            await _spuContext.SaveChangesAsync();

            var Entreprise = new Employeur
            {
                utilisateur = toCreate,
                UtilisateurId = toCreate.Id,
                adresse = ToAddadresse,
                AdresseId = ToAddadresse.Id,
            };
            _spuContext.Employeurs.Add(Entreprise);
            await _spuContext.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Create" });
        }







        //EDIT EMPLOYEUR
        [Authorize(Roles = "Coordonnateur")]
        [HttpGet]
        public async Task<IActionResult> EditEmployeur(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();


            var userEmployeur = await _spuContext.Employeurs.Where(x => x.UtilisateurId == user.Id).FirstOrDefaultAsync();

            var userAdresse = await _spuContext.Employeurs.Where(a => a.AdresseId == userEmployeur.AdresseId).Select(a => a.adresse).FirstOrDefaultAsync();

            if (userEmployeur == null)
                return NotFound();

            var modifUser = new EmployeurEditVM
            {
                Id = userEmployeur.Id,
                userName = userEmployeur.utilisateur.UserName,
                Nom = userEmployeur.utilisateur.Nom,
                Prenom = userEmployeur.utilisateur.Prenom,
                PhoneNumber = userEmployeur.utilisateur.PhoneNumber,
                Email = userEmployeur.utilisateur.Email,
                CodePostal = userAdresse.CodePostal,
                NomDeRue = userAdresse.Rue,
                NumeroDeRue = userAdresse.NoCivique,
                Pays = userAdresse.Pays,
                Province = userAdresse.Province,
                Ville = userAdresse.Ville

            };


            return View(modifUser);
        }


        //[AllowAnonymous]
        [Authorize(Roles = "Coordonnateur")]
        [HttpPost]
        public async Task<IActionResult> EditEmployeur(Guid id, EmployeurEditVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);

            var aEditer = await _userManager.FindByIdAsync(id.ToString()) ?? throw new ArgumentOutOfRangeException(nameof(id));
            var userEmployeur = _spuContext.Employeurs.Where(x => x.UtilisateurId == aEditer.Id).FirstOrDefault();

            var userAdresse = await _spuContext.Employeurs.Where(a => a.AdresseId == userEmployeur.AdresseId).Select(a => a.adresse).FirstOrDefaultAsync();

            aEditer.Nom = vm.Nom;
            aEditer.Prenom = vm.Prenom;
            aEditer.PhoneNumber = vm.PhoneNumber;
            aEditer.Email = vm.Email;
            aEditer.UserName = vm.userName;

            userEmployeur.utilisateur = aEditer;

            userAdresse.Province = vm.Province;
            userAdresse.NoCivique = vm.NumeroDeRue;
            userAdresse.Rue = vm.NomDeRue;
            userAdresse.CodePostal = vm.CodePostal;
            userAdresse.Ville = vm.Ville;


            var works = _userManager.UpdateAsync(aEditer);

            if (works != null)
            {
                TempData["SuccessMessage"] = "Modifications succeeded";

                _spuContext.Adresses.Update(userAdresse);
                _spuContext.Employeurs.Update(userEmployeur);
                await _spuContext.SaveChangesAsync();

                return RedirectToAction(nameof(Manage));
            }
            else
            {
                TempData["ErrorMessage"] = "Request failed";
                return View(vm);
            }

        }
        #endregion

        #region Remove 

        //REMOVE POUR TOUS LE MONDE 
        [Authorize(Roles = "Coordonnateur")]
        [HttpPost]
        public async Task<IActionResult> Remove(Guid id, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            switch (role)
            {
                case "Stagiaire":
                    var stagiaire = _spuContext.Stagiaires.FirstOrDefault(x => x.utilisateur == user);
                    if (stagiaire == null)
                        return NotFound();
                    _spuContext.Remove(stagiaire);
                    break;
                case "Coordonnateur":
                    var coordonnateur = _spuContext.Coordonnateurs.FirstOrDefault(x => x.utilisateur == user);
                    if (coordonnateur == null)
                        return NotFound();
                    _spuContext.Remove(coordonnateur);
                    break;
                case "Enseignant":
                    var enseignant = _spuContext.Enseignants.FirstOrDefault(x => x.utilisateur == user);
                    if (enseignant == null)
                        return NotFound();
                    _spuContext.Remove(enseignant);
                    break;
                case "Employeur":
                    var employeur = _spuContext.Employeurs.FirstOrDefault(x => x.utilisateur == user);
                    if (employeur == null)
                        return NotFound();
                    _spuContext.Remove(employeur);
                    break;
                case "MDS":
                    var mds = _spuContext.MDS.FirstOrDefault(x => x.utilisateur == user);
                    if (mds == null)
                        return NotFound();
                    _spuContext.Remove(mds);
                    break;
                default:
                    return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Impossible de supprimer un utilisateur. Veuillez réessayer.");
                return View();
            }

            await _spuContext.SaveChangesAsync();
            return RedirectToAction(nameof(Manage), new { success = true, actionType = "Remove" });
        }

        #endregion

        #region Relier 

        [Authorize(Roles = "Coordonnateur")]
        [HttpGet]
        public async Task<IActionResult> Relier()
        {
            var vm = new List<StagiairesEditVM>();

            ViewBag.Enseignants = _spuContext.Enseignants.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.utilisateur.UserName
            }).ToList();

            ViewBag.Mds = _spuContext.MDS.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                //Text = e.utilisateur.UserName
                Text = e.MatriculeId.ToString()
            }).ToList();

            try
            {
                foreach (var user in _spuContext.Stagiaires.Include(c => c.utilisateur).ToList())
                {
                    List<MDS> lstMds = _spuContext.MDS.Where(MDS => MDS.StagiaireId == user.Id).Include(u => u.utilisateur).ToList();
                   

                    if (user.finStage != null & user.debutStage != null)
                    {
                        vm.Add(new StagiairesEditVM
                        {
                            Id = user.Id,
                            Prenom = user.utilisateur?.Prenom,
                            Nom = user.utilisateur?.Nom,
                            idEnseignantSelectionne = user.EnseignantId,
                            idMdsSelectionne1 = lstMds.ElementAtOrDefault(0)?.Id,
                            idMdsSelectionne2 = lstMds.ElementAtOrDefault(1)?.Id,
                            debutStage = user.debutStage!.Value.Date,
                            finStage = user.finStage!.Value.Date
                            


                        }); 
                    }
                    else
                    {
                        vm.Add(new StagiairesEditVM
                        {
                            Id = user.Id,
                            Prenom = user.utilisateur?.Prenom,
                            Nom = user.utilisateur?.Nom,
                            idEnseignantSelectionne = user.EnseignantId,
                            idMdsSelectionne1 = lstMds.ElementAtOrDefault(0)?.Id,
                            idMdsSelectionne2 = lstMds.ElementAtOrDefault(1)?.Id,
                            debutStage = null,
                            finStage = null
                            
                        });
                    }
                    
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur d'affichage des stagiaires. Veuillez réessayer.";
            }
            return View(vm);
        }

        [Authorize(Roles = "Coordonnateur")]
        [HttpPost]
        public async Task<IActionResult> Relier(Guid idStagiaire, Guid? idMdsSelectionne1, Guid? idMdsSelectionne2, Guid? idEnseignantSelectionne, DateTime? debutStage, DateTime? finStage)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Relier");
            }


            if ((idMdsSelectionne1 == null && idMdsSelectionne2 == null && idEnseignantSelectionne == null) || (idMdsSelectionne1 != null && idMdsSelectionne1 == idMdsSelectionne2))
            {
                TempData["ErrorMessage"] = "Le même maître de stage a été sélectionné deux fois ou aucun maître de stage n'a été sélectionné. Veuillez sélectionner des maîtres de stage différents.";

                return RedirectToAction("Relier");
            }

            if (idEnseignantSelectionne == null)
            {
                TempData["ErrorMessage"] = "Veuillez sélectionner un enseignant.";

                return RedirectToAction("Relier");
            }

            var Stagiaire = _spuContext.Stagiaires.Find(idStagiaire);
            if (Stagiaire == null)
            {
                TempData["ErrorMessage"] = "Stagiaire introuvable.";
                return RedirectToAction("Relier");
            }


            var MdsaEditer1 = _spuContext.MDS.FirstOrDefault(x => x.Id == idMdsSelectionne1);
            var MdsaEditer2 = _spuContext.MDS.FirstOrDefault(x => x.Id == idMdsSelectionne2);
            var StagiaireAediter = _spuContext.Stagiaires.FirstOrDefault(x => x.Id == idStagiaire);
            var ChatsStagiaire = _spuContext.Chats.FirstOrDefault(x => x.Id == StagiaireAediter.ChatId);
            var anciensMds = _spuContext.MDS.Where(mds => mds.StagiaireId == idStagiaire).ToList();

            var horaireMDS1 = _spuContext.Horaires.FirstOrDefault(m => m.MDSId == idMdsSelectionne1);
    
            

            foreach (var mds in anciensMds)
            {
                if (mds.Id != idMdsSelectionne1 && mds.Id != idMdsSelectionne2)
                {
                    var ancienhoraire = _spuContext.Horaires.FirstOrDefault(m => m.MDSId == mds.Id);

                    if(ancienhoraire.stagiaire != null)
                    {
                        ancienhoraire.stagiaire = null;
                        ancienhoraire.StagiaireId = null;
                    }
                    mds.stagiaire = null;
                    mds.StagiaireId = null;
                    mds.ChatId = null;
                    mds.chat = null;                
                                 
                }
            }

            if (MdsaEditer1 != null)
            {
                MdsaEditer1.StagiaireId = idStagiaire;
                MdsaEditer1.chat = Stagiaire.chat;
                MdsaEditer1.ChatId = Stagiaire.ChatId;
                horaireMDS1.StagiaireId = Stagiaire.Id;
                horaireMDS1.stagiaire = Stagiaire;

                            
                    

             

                _spuContext.Horaires.Update(horaireMDS1);
                _spuContext.MDS.Update(MdsaEditer1);
            }

            if (MdsaEditer2 != null && idMdsSelectionne2 != idMdsSelectionne1)
            {
                MdsaEditer2.StagiaireId = idStagiaire;
                MdsaEditer2.chat = Stagiaire.chat;
                MdsaEditer2.ChatId = Stagiaire.ChatId;
                _spuContext.MDS.Update(MdsaEditer2);
            }

            if (StagiaireAediter != null)
            {
                StagiaireAediter.EnseignantId = idEnseignantSelectionne;
                ChatsStagiaire.EnseignantId = idEnseignantSelectionne;

                if (debutStage != null && finStage != null)
                {
                    StagiaireAediter.debutStage = debutStage.Value.ToUniversalTime();
                    StagiaireAediter.finStage = finStage.Value.ToUniversalTime();
                }
                else
                {
                    TempData["ErrorMessage"] = "Veuillez entré une date de début et de fin de stage";
                    return RedirectToAction("Relier");
                }

                _spuContext.Stagiaires.Update(StagiaireAediter);
            }

            await _spuContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Modifications réussies";

            return RedirectToAction("Relier");
        }
        #endregion

        #region Exportation
        [Authorize(Roles = "Coordonnateur")]
        public IActionResult ExportContrat()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ententes_de_stage");
                worksheet.Columns().Width = 15;
                worksheet.Row(1).Height = 40;
                worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#d3d3d3");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "Millieu stage";
                worksheet.Cell("C1").Value = "Titre";
                worksheet.Cell("D1").Value = "Signataire contrat";
                worksheet.Cell("E1").Value = "Fonction";
                worksheet.Cell("F1").Value = "Courriel du signataire";
                worksheet.Cell("G1").Value = "Tél.";
                worksheet.Cell("H1").Value = "Adresse";
                worksheet.Cell("I1").Value = "Ville";
                worksheet.Cell("J1").Value = "Prov";
                worksheet.Cell("K1").Value = "Code postal";
                worksheet.Cell("L1").Value = "Superviseur milieu stage";
                worksheet.Cell("M1").Value = "Stagiaire";
                worksheet.Cell("N1").Value = "Prénom";
                worksheet.Cell("O1").Value = "Secteur";
                worksheet.Cell("P1").Value = "Program";
                worksheet.Cell("Q1").Value = "Type de stage";
                worksheet.Cell("R1").Value = "Dates stage";
                worksheet.Cell("S1").Value = "Superviseur collège";
                worksheet.Cell("T1").Value = "Poste";

                List<Stagiaire> stagiaires = _spuContext.Stagiaires
                  .Include(x => x.utilisateur)
                  .Include(x => x.employeur).ThenInclude(x => x.utilisateur)
                  .Include(x => x.employeur).ThenInclude(x => x.adresse)
                  .Include(x => x.enseignant).ThenInclude(x => x.utilisateur)
                  .ToList();

                for (int i = 2; i < stagiaires.Count(); i++)
                {
                    MDS mds = _spuContext.MDS.Include(x => x.utilisateur).FirstOrDefault(x => x.StagiaireId == stagiaires[i].Id);

                    worksheet.Cell($"A{i}").Value = "";
                    worksheet.Cell($"B{i}").Value = stagiaires[i].employeur.utilisateur.UserName;
                    worksheet.Cell($"C{i}").Value = "";
                    worksheet.Cell($"E{i}").Value = "";
                    worksheet.Cell($"F{i}").Value = "";
                    worksheet.Cell($"G{i}").Value = stagiaires[i].employeur.utilisateur.PhoneNumber;
                    worksheet.Cell($"H{i}").Value = stagiaires[i].employeur.adresse.NoCivique + " " + stagiaires[i].employeur.adresse.Rue;
                    worksheet.Cell($"I{i}").Value = stagiaires[i].employeur.adresse.Ville;
                    worksheet.Cell($"J{i}").Value = stagiaires[i].employeur.adresse.Province;
                    worksheet.Cell($"K{i}").Value = stagiaires[i].employeur.adresse.CodePostal;
                    worksheet.Cell($"L{i}").Value = mds.utilisateur.NomComplet + " / " + mds.MatriculeId;
                    worksheet.Cell($"M{i}").Value = stagiaires[i].utilisateur.NomComplet;
                    worksheet.Cell($"N{i}").Value = stagiaires[i].utilisateur.Prenom;
                    worksheet.Cell($"O{i}").Value = mds.typeEmployeur == 0 ? "CISSS" : "CIUSSS";
                    worksheet.Cell($"P{i}").Value = "";
                    worksheet.Cell($"Q{i}").Value = "";
                    worksheet.Cell($"R{i}").Value = stagiaires[i].debutStage != null ? stagiaires[i].debutStage.Value.ToLocalTime().ToString() + " - " + stagiaires[i].finStage.Value.ToLocalTime().ToString() : "";
                    worksheet.Cell($"S{i}").Value = stagiaires[i].enseignant.utilisateur.NomComplet;
                    worksheet.Cell($"T{i}").Value = "";
                }

                var worksheet2 = workbook.Worksheets.Add("mds_source");
                worksheet2.Columns().Width = 15;
                worksheet2.Row(1).Height = 40;
                worksheet2.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#d3d3d3");

                worksheet2.Cell("A1").Value = "Matricule";
                worksheet2.Cell("B1").Value = "Statut";
                worksheet2.Cell("C1").Value = "Civilité";
                worksheet2.Cell("D1").Value = "Nom complet";
                worksheet2.Cell("D1").Value = "Nom";
                worksheet2.Cell("E1").Value = "Prénom";
                worksheet2.Cell("F1").Value = "Tél. maison";
                worksheet2.Cell("G1").Value = "Tél. cellulaire";
                worksheet2.Cell("H1").Value = "Courriel";
                worksheet2.Cell("I1").Value = "CISSS/CIUSSS";
                worksheet2.Cell("J1").Value = "Employeur";
                worksheet2.Cell("K1").Value = "CISSS/CIUSSS 2";
                worksheet2.Cell("L1").Value = "Employeur 2";
                worksheet2.Cell("M1").Value = "Commentaires";
                worksheet2.Cell("N1").Value = "commentaireCIUSS";

                List<MDS> maitresdestage = _spuContext.MDS
                  .Include(x => x.utilisateur)
                  .ToList();

                for (int i = 2; i < maitresdestage.Count(); i++)
                {
                  if(maitresdestage[i].status == Status.Incomplet)//incomplet
                  {
                    worksheet2.Cell($"B{i}").Value = "incomplet";
                    worksheet2.Row(i).Style.Fill.BackgroundColor = XLColor.Yellow;
                  }
                  if(maitresdestage[i].status == Status.Accepté)//accepté
                  {
                    worksheet2.Cell($"B{i}").Value = "accepté";
                    worksheet2.Row(i).Style.Fill.BackgroundColor = XLColor.Green;
                  }
                  if(maitresdestage[i].status == Status.Refusé)//refusé
                  {
                    worksheet2.Cell($"B{i}").Value = "refusé";
                    worksheet2.Row(i).Style.Fill.BackgroundColor = XLColor.Red;
                  }

                  worksheet2.Cell($"A{i}").Value = maitresdestage[i].MatriculeId;
                  worksheet2.Cell($"C{i}").Value = maitresdestage[i].civilite == 0 ? "M" : "Mme";
                  worksheet2.Cell($"D{i}").Value = maitresdestage[i].utilisateur.NomComplet;
                  worksheet2.Cell($"D{i}").Value = maitresdestage[i].utilisateur.Nom;
                  worksheet2.Cell($"E{i}").Value = maitresdestage[i].utilisateur.Prenom;
                  worksheet2.Cell($"F{i}").Value = maitresdestage[i].telMaison;
                  worksheet2.Cell($"G{i}").Value = maitresdestage[i].utilisateur.PhoneNumber;
                  worksheet2.Cell($"H{i}").Value = maitresdestage[i].utilisateur.Email;
                  worksheet2.Cell($"I{i}").Value = maitresdestage[i].typeEmployeur == 0 ? "CISSS" : "CIUSSS";
                  worksheet2.Cell($"J{i}").Value = maitresdestage[i].NomEmployeur;
                  worksheet2.Cell($"K{i}").Value = "";
                  worksheet2.Cell($"L{i}").Value = "";
                  worksheet2.Cell($"M{i}").Value = maitresdestage[i].commentaire;
                  worksheet2.Cell($"N{i}").Value = maitresdestage[i].commentaireCIUSS;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    stream.Seek(0, SeekOrigin.Begin);

                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(stream.ToArray(), contentType, "exportation_donnees_SPU.xlsx");
                }
            }
        }
        #endregion


        #region Filtre
        [HttpGet]
        public IActionResult GetFilteredData(string filter)
        {
            var filteredData = _spuContext.MDS.Where(item => item.MatriculeId.StartsWith(filter)).Select(item => item.MatriculeId).ToList();
            return Json(filteredData);
        }

        #endregion
    }



}



