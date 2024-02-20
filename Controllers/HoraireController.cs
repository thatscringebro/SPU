using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPU.Domain;
using SPU.Domain.Entites;
using SPU.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SPU.Controllers
{
    public class HoraireController : Controller
    {
        private readonly string _loggedUserId;
        private readonly SpuContext _context;
        public HoraireController(SpuContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _loggedUserId = claim?.Value;
        }

        [Authorize]
        public IActionResult Index()
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            HoraireIndexVM vm = new HoraireIndexVM();

            //Liste horaire maître de stage
            if (mds != null)
            {
                Horaire horaire = _context.Horaires.Where(x => x.MDSId == mds.Id).FirstOrDefault();
                vm.role = "MDS";

                if (horaire != null)
                {
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = horaire.Id });
                }
                else
                {
                    return View(vm);
                }
            }

            //Ajout liste pour stagiaire
            else if (stag != null)
            {
                Horaire horaire = _context.Horaires.Where(x => x.StagiaireId == stag.Id).FirstOrDefault();
                vm.role = "stagiaire";

                if (horaire != null)
                {
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = horaire.Id });
                }
                else
                {
                    return View(vm);
                }
            }
            //Faire les autres rôles

            return View(vm);
        }


        public IActionResult Horaire(Guid horaireId, HorairePageVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);
            Horaire horaire = new Horaire();

            horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();
            ViewBag.horaireId = horaire.Id;
            ViewBag.mdsId = null;

            // Récupérer le message d'erreur de la session temporaire
            string errorMessage = TempData["ErrorMessage"] as string;

            // Passer le message d'erreur à la vue
            ViewBag.ErrorMessage = errorMessage;

            if (user != null)
            {
                Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
                MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

                MDS mdsHoraire = _context.MDS.Where(x => x.Id == horaire.MDSId).Include(c => c.utilisateur).FirstOrDefault();
                Stagiaire? stagHoraire = _context.Stagiaires.Where(x => x.Id == horaire.StagiaireId).Include(c => c.utilisateur).FirstOrDefault();

                vm.nomMds = string.Concat(mdsHoraire.utilisateur.Prenom + " " + mdsHoraire.utilisateur.Nom);
                vm.nomStagiaire = string.Concat(stagHoraire?.utilisateur.Prenom + " " + stagHoraire?.utilisateur.Nom);


                vm.DateCreationHoraire = mdsHoraire.DateCreationHoraire;
                vm.DateExpiration = mdsHoraire.DateExpiration;

                if (stagHoraire != null)
                {
                    vm.DateDebutStage = stagHoraire.debutStage;
                    vm.DateFinStage = stagHoraire.finStage;
                }

                //Pour gestion des rôles
                if (mds != null)
                {
                    vm.Role = "MDS";
                }
                else if (stag != null)
                {
                    vm.Role = "stagiaire";
                }
            }

            return View(vm);
        }

        public IActionResult HoraireMds(Guid userId, HorairePageVM vm)
        {
            //var vm = new HorairePageVM();

            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id == userId);
            Horaire horaire = new Horaire();

            var Mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == userId);
            //Guid horaireId = _context.Horaires.Where(x => x.MDSId == Mds.Id).Select(h => h.Id).FirstOrDefault();
            horaire = _context.Horaires.Where(x => x.MDSId == Mds.Id).FirstOrDefault();

            //horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();

            if (horaire == null || Mds == null)
            {
                string errorMessage = horaire == null ? "L'horaire du maître de stage est introuvable ou inexistant!"
                                                         : "Le maître de stage est introuvable ou inexistant!";
                TempData["ErrorMessage"] = errorMessage;

                // Determine the appropriate redirection based on the user's role
                if (User.IsInRole("Coordonnateur"))
                {
                    return RedirectToAction("Manage", "Compte");
                }
                else if (User.IsInRole("Employeur"))
                {
                    return RedirectToAction("ManageMds", "Compte");
                }
            }

            ViewBag.horaireId = horaire.Id;
            ViewBag.mdsId = Mds.Id;

            // Récupérer le message d'erreur de la session temporaire
            //string errorMessage = TempData["ErrorMessage"] as string;

            // Passer le message d'erreur à la vue
            //ViewBag.ErrorMessage = errorMessage;

            if (Mds != null)
            {
                Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
                MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

                MDS mdsHoraire = _context.MDS.Where(x => x.Id == horaire.MDSId).Include(c => c.utilisateur).FirstOrDefault();
                Stagiaire? stagHoraire = _context.Stagiaires.Where(x => x.Id == horaire.StagiaireId).Include(c => c.utilisateur).FirstOrDefault();

                vm.nomMds = string.Concat(mdsHoraire.utilisateur.Prenom + " " + mdsHoraire.utilisateur.Nom);
                vm.nomStagiaire = string.Concat(stagHoraire?.utilisateur.Prenom + " " + stagHoraire?.utilisateur.Nom);


                vm.DateCreationHoraire = mdsHoraire.DateCreationHoraire;
                vm.DateExpiration = mdsHoraire.DateExpiration;
                vm.DateDebutStage = stagHoraire.debutStage;
                vm.DateFinStage = stagHoraire.finStage;


            }

            //return View(vm);
            return View("Horaire", vm);
        }


        //public IActionResult AjoutNouvelHoraireMDS()
        //{
        //Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

        //Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
        //MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);
        //Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);

        //Horaire nouvelleHoraire = new Horaire();

        //if (mds != null)
        //{
        //    nouvelleHoraire.Id = Guid.NewGuid();
        //    nouvelleHoraire.mds = mds;
        //    nouvelleHoraire.MDSId = mds.Id;

        //    // Obtenir la date et l'heure actuelles dans le fuseau horaire local
        //    DateTime debutHoraire = DateTime.Now;


        //    // Démarrer l'horaire à partir du dimanche prochain
        //    while (debutHoraire.DayOfWeek != DayOfWeek.Sunday)
        //    {
        //        debutHoraire = debutHoraire.AddDays(1);
        //    }

        //    debutHoraire = new DateTime(debutHoraire.Year, debutHoraire.Month, debutHoraire.Day, 0, 0, 0);

        //    // Ajouter deux ans
        //    DateTime finHoraire = debutHoraire.AddYears(2);

        //    TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        //    debutHoraire = TimeZoneInfo.ConvertTime(debutHoraire, localTimeZone);
        //    finHoraire = TimeZoneInfo.ConvertTime(finHoraire, localTimeZone);

        //    mds.DateCreationHoraire = debutHoraire.ToUniversalTime();
        //    mds.DateExpiration = finHoraire.ToUniversalTime();

        //    // !!! ---- Stagiaire fixif pour association horaire ----- !!!
        //    Stagiaire stagiaireFixif = _context.Stagiaires.Include(c => c.utilisateur).FirstOrDefault();

        //    if (stagiaireFixif != null)
        //    {
        //        stagiaireFixif.DateDebutStage = new DateTime(2024,2,26).ToUniversalTime();
        //        stagiaireFixif.DateFinStage = new DateTime(2024,5,26).ToUniversalTime();

        //        nouvelleHoraire.stagiaire = stagiaireFixif;
        //        nouvelleHoraire.StagiaireId = stagiaireFixif.Id;
        //    }
        //    // !!! --- !!!

        //    _context.Add(nouvelleHoraire);
        //    _context.SaveChanges();

        //    ViewBag.horaireId = nouvelleHoraire.Id;
        //}

        //return RedirectToAction("Horaire", "Horaire", new { horaireId = nouvelleHoraire.Id });
        //}

        public IActionResult AjoutPlageHoraireMDS(Guid horaireId)
        {
            ViewBag.horaireId = horaireId;

            return View();
        }

        [HttpPost]
        public IActionResult AjoutPlageHoraireMDS(PlageHoraireMdsVM vm, Guid horaireId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.horaireId = horaireId;
                return View(vm);
            }

            Horaire horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();
            MDS mds = _context.MDS.Where(x => x.Id == horaire.MDSId).FirstOrDefault();

            //Si il y a de la récurrence
            if (vm.Recurrence)
            {

                if (horaire != null)
                {
                    //DateTime dateDebutStage = horaire.DateDebutStage;
                    //DateTime dateFinStage = horaire.DateFinStage;

                    // Obtenir l'heure locale
                    TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                    DateTime plageHoraireDebut = TimeZoneInfo.ConvertTime(new DateTime(vm.DateDebutPlageHoraire.Year,
                                              vm.DateDebutPlageHoraire.Month,
                                              vm.DateDebutPlageHoraire.Day,
                                              vm.HeureDebutPlageHoraire,
                                              vm.MinutesDebutPlageHoraire,
                                              0), localTimeZone);

                    DateTime plageHoraireFin = TimeZoneInfo.ConvertTime(new DateTime(vm.DateFinPlageHoraire.Year,
                                              vm.DateFinPlageHoraire.Month,
                                              vm.DateFinPlageHoraire.Day,
                                              vm.HeureFinPlageHoraire,
                                              vm.MinutesFinPlageHoraire,
                                              0), localTimeZone);

                    List<PlageHoraire> listePlageHoraire = new List<PlageHoraire>();

                    while (plageHoraireDebut >= mds.DateCreationHoraire?.ToLocalTime() && plageHoraireFin <= mds.DateExpiration?.ToLocalTime())
                    {
                        PlageHoraire plageHoraireRecurrence = new PlageHoraire();
                        plageHoraireRecurrence.Id = Guid.NewGuid();
                        plageHoraireRecurrence.HoraireId = horaireId;
                        plageHoraireRecurrence.DateDebut = plageHoraireDebut;
                        plageHoraireRecurrence.DateFin = plageHoraireFin;
                        plageHoraireRecurrence.StagiaireAbsent = true;

                        //Aller vérifier si il y a une plage horaire déjà existante
                        PlageHoraire verificationPlageHoraire = _context.PlageHoraires
                        .FirstOrDefault(ph =>
                            (plageHoraireRecurrence.DateDebut <= ph.DateDebut.ToLocalTime() && plageHoraireRecurrence.DateFin > ph.DateDebut.ToLocalTime()) ||
                            (plageHoraireRecurrence.DateDebut >= ph.DateDebut.ToLocalTime() && plageHoraireRecurrence.DateFin <= ph.DateFin.ToLocalTime()) ||
                            (plageHoraireRecurrence.DateDebut < ph.DateFin.ToLocalTime() && plageHoraireRecurrence.DateFin >= ph.DateFin.ToLocalTime()) ||
                            (plageHoraireRecurrence.DateDebut <= ph.DateDebut.ToLocalTime() && plageHoraireRecurrence.DateFin >= ph.DateFin.ToLocalTime())
                        );

                        if (verificationPlageHoraire != null)
                        {
                            string errorMessage = "La plage horaire chevauche une autre plage horaire existante.";
                            ModelState.AddModelError("PlageHoraire", errorMessage);
                            TempData["ErrorMessage"] = errorMessage;
                            return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                        }

                        listePlageHoraire.Add(plageHoraireRecurrence);

                        plageHoraireDebut = plageHoraireDebut.AddDays(14);
                        plageHoraireFin = plageHoraireFin.AddDays(14);
                    }

                    foreach (PlageHoraire ph in listePlageHoraire)
                    {
                        // Conserver les dates en tant qu'UTC pour PostgreSQL
                        ph.DateDebut = ph.DateDebut.ToUniversalTime();
                        ph.DateFin = ph.DateFin.ToUniversalTime();
                        _context.Add(ph);
                    }

                    _context.SaveChanges();
                }
            }
            // Pas de récurrence
            else
            {
                vm.id = Guid.NewGuid();

                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                DateTime plageHoraireDebut = TimeZoneInfo.ConvertTime(new DateTime(vm.DateDebutPlageHoraire.Year,
                                          vm.DateDebutPlageHoraire.Month,
                                          vm.DateDebutPlageHoraire.Day,
                                          vm.HeureDebutPlageHoraire,
                                          vm.MinutesDebutPlageHoraire,
                                          0), localTimeZone);

                DateTime plageHoraireFin = TimeZoneInfo.ConvertTime(new DateTime(vm.DateFinPlageHoraire.Year,
                                          vm.DateFinPlageHoraire.Month,
                                          vm.DateFinPlageHoraire.Day,
                                          vm.HeureFinPlageHoraire,
                                          vm.MinutesFinPlageHoraire,
                                          0), localTimeZone);

                PlageHoraire ph = new PlageHoraire();
                ph.Id = Guid.NewGuid();
                ph.HoraireId = horaireId;
                ph.DateDebut = plageHoraireDebut;
                ph.DateFin = plageHoraireFin;
                ph.StagiaireAbsent = true;

                //Aller chercher les plages horaires en lien avec le maître de stage

                PlageHoraire verificationPlageHoraire = _context.PlageHoraires
                    .FirstOrDefault(x =>
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin > x.DateDebut.ToLocalTime()) ||
                        (ph.DateDebut >= x.DateDebut.ToLocalTime() && ph.DateFin <= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut < x.DateFin.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime())
                    );

                if (verificationPlageHoraire != null)
                {
                    string errorMessage = "La plage horaire chevauche une autre plage horaire existante.";
                    ModelState.AddModelError("PlageHoraire", errorMessage);
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                }

                if (ph.DateDebut.ToLocalTime() < mds.DateCreationHoraire?.ToLocalTime() || ph.DateFin.ToLocalTime() < mds.DateCreationHoraire?.ToLocalTime()
                    || ph.DateDebut.ToLocalTime() > mds.DateExpiration?.ToLocalTime() || ph.DateFin.ToLocalTime() > mds.DateExpiration?.ToLocalTime())
                {
                    string errorMessage = "La date et heure de la plage horaire doit correspondre aux dates de début et fin du stage.";
                    ModelState.AddModelError("PlageHoraire", errorMessage);
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                }

                ph.DateDebut = ph.DateDebut.ToUniversalTime();
                ph.DateFin = ph.DateFin.ToUniversalTime();

                _context.Add(ph);
                _context.SaveChanges();
            }
            return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
        }


        // Ajout d'une confirmation
        public IActionResult ModifierPlageHoraire(ModifierPlageHoraireVM vm, Guid idPlageHoraire, Guid idHoraire)
        {
            //Aller chercher les informations avec le id de l'horaire et le id du div
            PlageHoraire ph = _context.PlageHoraires.Where(x => x.Id == idPlageHoraire).FirstOrDefault();

            vm.id = ph.Id;
            vm.DateDebutPlageHoraire = ph.DateDebut.ToLocalTime();
            vm.DateFinPlageHoraire = ph.DateFin.ToLocalTime();
            vm.HeureDebutPlageHoraire = ph.DateDebut.ToLocalTime().Hour;
            vm.HeureFinPlageHoraire = ph.DateFin.ToLocalTime().Hour;
            vm.MinutesDebutPlageHoraire = ph.DateDebut.ToLocalTime().Minute;
            vm.MinutesFinPlageHoraire = ph.DateFin.ToLocalTime().Minute;
            vm.Commentaire = ph.Commentaire;
            vm.EstPresent = ph.StagiaireAbsent;

            ViewBag.PlageHoraireId = idPlageHoraire;

            return View(vm);
        }

        [HttpPost]
        public IActionResult ModifierPlageHoraire(ModifierPlageHoraireVM vm, string actionType, Guid PlageHoraireId)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (actionType == "supprimer")
            {
                // Récupérer la plage horaire existante depuis la base de données
                PlageHoraire ph = _context.PlageHoraires.FirstOrDefault(x => x.Id == PlageHoraireId);

                if (ph != null)
                {
                    _context.PlageHoraires.Remove(ph);
                    _context.SaveChanges();

                    // Rediriger vers l'action "Horaire" du contrôleur "Horaire"
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = ph.HoraireId });
                }
                else
                {
                    // Gérer le cas où la plage horaire n'est pas trouvée
                    // Vous pouvez renvoyer une vue avec un message d'erreur approprié, par exemple.
                    return NotFound();
                }
            }
            else if (actionType == "modifier")
            {
                DateTime plageHoraireDebut = new DateTime(vm.DateDebutPlageHoraire.Year,
                              vm.DateDebutPlageHoraire.Month,
                              vm.DateDebutPlageHoraire.Day,
                              vm.HeureDebutPlageHoraire,
                              vm.MinutesDebutPlageHoraire,
                              0).ToUniversalTime();

                DateTime plageHoraireFin = new DateTime(vm.DateFinPlageHoraire.Year,
                                  vm.DateFinPlageHoraire.Month,
                                  vm.DateFinPlageHoraire.Day,
                                  vm.HeureFinPlageHoraire,
                                  vm.MinutesFinPlageHoraire,
                                  0).ToUniversalTime();

                Guid horaireId = _context.PlageHoraires.Where(x => x.Id == PlageHoraireId).FirstOrDefault().HoraireId;

                // Récupérer la plage horaire existante depuis la base de données
                PlageHoraire ph = _context.PlageHoraires.FirstOrDefault(x => x.Id == PlageHoraireId);

                // Vérifier si la plage horaire existe
                if (ph != null)
                {
                    var horaire = _context.Horaires.FirstOrDefault(x => x.Id == ph.HoraireId);

                    

                    // Mettre à jour les propriétés de la plage horaire avec les nouvelles valeurs
                    ph.HoraireId = horaireId;
                    ph.Commentaire = vm.Commentaire;
                    //ph.StagiaireAbsent = vm.EstPresent;
                    ph.DateDebut = plageHoraireDebut;
                    ph.DateFin = plageHoraireFin;

                    //ICI CLAUDEL POUR LA MODIF DE LA PLAGE HORAIRE
                    if (!vm.EstPresent)
                    {
                        if(ph.MDS1absent == null)
                        {
                            ph.MDS1absent = new Guid(_loggedUserId);
                        }
                        else if(ph.MDS2absent == null)
                        {
                            ph.MDS2absent = new Guid(_loggedUserId);
                        }
                    }
                    else if(vm.EstPresent && ph.MDS1absent != null) {
                        ph.MDS1absent = null;
                    }
                    else if(vm.EstPresent && ph.MDS2absent != null)
                    {
                        ph.MDS2absent = null;
                    }


                    // Enregistrer les modifications dans la base de données
                    _context.SaveChanges();

                    // Rediriger vers l'action "Horaire" du contrôleur "Horaire"
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = ph.HoraireId });
                }
                else
                {
                    // Gérer le cas où la plage horaire n'est pas trouvée
                    // Vous pouvez renvoyer une vue avec un message d'erreur approprié, par exemple.
                    return NotFound();
                }
            }
            return RedirectToAction("Index", "Horaire");
        }


        //Allo :) 
        [HttpPost]
        public IActionResult SupprimerPlageHoraire(ModifierPlageHoraireVM vm)
        {
            return RedirectToAction("Index", "Horaire");
        }


        //affichage info
        public async Task<IActionResult> ObtenirInfoPlageHoraire(string Id)
        {
            JourneeTravailleVM? journeeTravailles = _context.PlageHoraires.Where(x => x.Id.ToString() == Id).Select(x => new JourneeTravailleVM
            {
                DateDebutQuart = x.DateDebut,
                DateFinQuart = x.DateFin,
                Id = x.Id,
                Present = x.StagiaireAbsent
            }).FirstOrDefault();

            if (journeeTravailles != null)
                return Ok(journeeTravailles);
            return NotFound("Erreur, la plage horaire n'est pas valide");
        }


        //Stagiaire
        public IActionResult MettreAbsent(string Id)
        {
            PlageHoraire? plage = _context.PlageHoraires.Where(x => x.Id.ToString() == Id).FirstOrDefault();

            if (plage == null)
                return BadRequest("Erreur, la plage horaire n'est pas valide");

            plage.StagiaireAbsent = false;
            try
            {
                _context.PlageHoraires.Update(plage);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest("Erreur, la plage horraire n'a pas pu être modifiée");
            }
        }

        public IActionResult ReprendreJournee()
        {
            Stagiaire? user = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId.ToString() == _loggedUserId);
            if (user == null)
                return BadRequest("La plage horaire sélectionnée n'est pas valide. La journée n'à pas pu être reprise");


            if (user.finStage.HasValue)
            {
                List<PlageHoraire> listPlagesHoraire = _context.PlageHoraires.Include(x => x.horaire).Where(x => x.horaire.StagiaireId.ToString() == _loggedUserId).ToList();

                bool plageTrouve = false;

                while (!plageTrouve)
                {
                    user.finStage = user.finStage.Value.AddDays(1);
                    foreach (PlageHoraire plage in listPlagesHoraire)
                    {
                        if (plage.DateDebut.Date == user.finStage.Value.Date)
                        {
                            _context.Update(user);
                            _context.SaveChanges();
                            plageTrouve = true;
                            break;
                        }
                    }
                }
            }
            else
                return BadRequest("La fin de votre stage n'est pas encore définie");

            return Ok(user.finStage);

        }
    }
}
