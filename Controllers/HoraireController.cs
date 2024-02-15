using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPU.Domain;
using SPU.Domain.Entites;
using SPU.ViewModels;
using System.Security.Claims;

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
        public IActionResult Index(ListeHoraireVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            //Liste horaire maître de stage
            if (mds != null)
            {
                vm.listeHoraire = _context.Horaires.Where(x => x.MDSId == mds.Id).ToList();
            }
            //Ajout liste pour stagiaire
            else if(stag != null)
            {
                vm.listeHoraire = _context.Horaires.Where(x => x.StagiaireId == stag.Id).ToList();
            }

            return View(vm);
        }


        public IActionResult Horaire(Guid horaireId, HorairePageVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);
            Horaire horaire = new Horaire();

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

                //Ajout champs horaireId dans bd pour stagiaire
                //Stagiaire? stagiaire = _context.Stagiaires.Where(x => x);


                if (mds != null)
                {
                    vm.nomMds =  string.Concat(mds.utilisateur.Prenom + " " + mds.utilisateur.Nom);
                
                    horaire = _context.Horaires.Where(x => x.MDSId == mds.Id && x.Id == horaireId).FirstOrDefault();

                    if (horaire != null)
                    {
                        ViewBag.horaireId = horaire.Id;
                        vm.DateDebutStage = horaire.DateDebutStage.Date;
                        vm.DateFinStage = horaire.DateFinStage.Date;
                    }
                }
            }

            return View(vm);
        }


        public IActionResult AjoutNouvelHoraireMDS()
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            AjoutNouvelHoraireMdsVM vm = new AjoutNouvelHoraireMdsVM();

            if(mds != null)
            {
                vm.listeMaitreDeStage.Add(mds);
            }
            else if(coordo != null)
            {
                vm.listeMaitreDeStage = _context.MDS.ToList();
            }

            return View(vm); 
        }


        [HttpPost]
        public IActionResult AjoutNouvelHoraireMDS(AjoutNouvelHoraireMdsVM vm)
        {

            //Doit entrer le id de la personne
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            //Changer ce code pour le user

            vm.choixMds1 = mds;

            //if (!ModelState.IsValid)
            //{
            //    return View(vm);
            //}

            Horaire nouvelleHoraire = new Horaire();
            
            // Si c'est un maître de stage connecté
            if (mds != null)
            {
                nouvelleHoraire.mds = vm.choixMds1;
                nouvelleHoraire.MDSId = vm.choixMds1.Id;
                nouvelleHoraire.Id = Guid.NewGuid();

                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                DateTime plageHoraireDebut = TimeZoneInfo.ConvertTime(new DateTime(vm.DateTimeDebutStage.Year,
                                          vm.DateTimeDebutStage.Month,
                                          vm.DateTimeDebutStage.Day,
                                          0,0,0), localTimeZone);

                DateTime plageHoraireFin = TimeZoneInfo.ConvertTime(new DateTime(vm.DateTimeFinStage.Year,
                                          vm.DateTimeFinStage.Month,
                                          vm.DateTimeFinStage.Day,
                                          23,59,59), localTimeZone);

                //Données temporaires
                nouvelleHoraire.DateDebutStage = vm.DateTimeDebutStage.ToUniversalTime();
                nouvelleHoraire.DateFinStage = vm.DateTimeFinStage.ToUniversalTime();

                _context.Add(nouvelleHoraire);
                _context.SaveChanges();

                ViewBag.horaireId = nouvelleHoraire.Id;
            }

            // Si c'est un coordonateur qui est connecté
            else if(coordo != null)
            {
                 
            }

            return RedirectToAction("Horaire", "Horaire", new { horaireId = nouvelleHoraire.Id });
        }

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

            //Si il y a de la récurrence
            if (vm.Recurrence)
            {

                if (horaire != null)
                {
                    DateTime dateDebutStage = horaire.DateDebutStage;
                    DateTime dateFinStage = horaire.DateFinStage;

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

                    while (plageHoraireDebut <= dateFinStage && plageHoraireFin <= dateFinStage)
                    {
                        PlageHoraire plageHoraireRecurrence = new PlageHoraire();
                        plageHoraireRecurrence.Id = Guid.NewGuid();
                        plageHoraireRecurrence.HoraireId = horaireId;
                        plageHoraireRecurrence.DateDebut = plageHoraireDebut;
                        plageHoraireRecurrence.DateFin = plageHoraireFin;
                        plageHoraireRecurrence.ConfirmationPresence = true;

                        //Aller vérifier si il y a une plage horaire déjà existante
                        PlageHoraire verificationPlageHoraire = _context.PlageHoraires
                        .FirstOrDefault(ph =>
                            (plageHoraireRecurrence.DateDebut <= ph.DateDebut.ToLocalTime() && plageHoraireRecurrence.DateFin > ph.DateDebut.ToLocalTime()) ||
                            (plageHoraireRecurrence.DateDebut >= ph.DateDebut.ToLocalTime() && plageHoraireRecurrence.DateFin <= ph.DateFin.ToLocalTime()) ||
                            (plageHoraireRecurrence.DateDebut <  ph.DateFin.ToLocalTime() && plageHoraireRecurrence.DateFin >= ph.DateFin.ToLocalTime()) ||
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
                ph.ConfirmationPresence = true;

                //Aller chercher les plages horaires en lien avec le maître de stage

                PlageHoraire verificationPlageHoraire = _context.PlageHoraires
                    .FirstOrDefault(x =>
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin > x.DateDebut.ToLocalTime()) ||
                        (ph.DateDebut >= x.DateDebut.ToLocalTime() && ph.DateFin <= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut < x.DateFin.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime())
                    );

                if(verificationPlageHoraire != null)
                {
                    string errorMessage = "La plage horaire chevauche une autre plage horaire existante.";
                    ModelState.AddModelError("PlageHoraire", errorMessage);
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                }

                if(ph.DateDebut.ToLocalTime() < horaire.DateDebutStage.ToLocalTime() || ph.DateFin.ToLocalTime() < horaire.DateDebutStage.ToLocalTime()
                    || ph.DateDebut.ToLocalTime() > horaire.DateFinStage.ToLocalTime() || ph.DateFin.ToLocalTime() > horaire.DateFinStage.ToLocalTime())
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
            PlageHoraire ph = _context.PlageHoraires.Where(x => x.Id== idPlageHoraire).FirstOrDefault();

            vm.id = ph.Id;
            vm.DateDebutPlageHoraire = ph.DateDebut.ToLocalTime();
            vm.DateFinPlageHoraire= ph.DateFin.ToLocalTime();
            vm.HeureDebutPlageHoraire = ph.DateDebut.ToLocalTime().Hour;
            vm.HeureFinPlageHoraire = ph.DateFin.ToLocalTime().Hour;
            vm.MinutesDebutPlageHoraire = ph.DateDebut.ToLocalTime().Minute;
            vm.MinutesFinPlageHoraire = ph.DateFin.ToLocalTime().Minute;
            vm.Commentaire = ph.Commentaire;
            vm.EstPresent = ph.ConfirmationPresence;

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
            else if(actionType == "modifier")
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
                    // Mettre à jour les propriétés de la plage horaire avec les nouvelles valeurs
                    ph.HoraireId = horaireId;
                    ph.Commentaire = vm.Commentaire;
                    ph.ConfirmationPresence = vm.EstPresent;
                    ph.DateDebut = plageHoraireDebut;
                    ph.DateFin = plageHoraireFin;

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

        [HttpPost]
        public IActionResult SupprimerPlageHoraire(ModifierPlageHoraireVM vm)
        {
            return RedirectToAction("Index", "Horaire");
        }

        public async Task<IActionResult> ObtenirInfoPlageHoraire(string Id)
        {
            JourneeTravailleVM? journeeTravailles = _context.PlageHoraires.Where(x => x.Id.ToString() == Id).Select(x => new JourneeTravailleVM
            {
                DateDebutQuart = x.DateDebut,
                DateFinQuart = x.DateFin,
                Id = x.Id,
                Present = x.ConfirmationPresence
            }).FirstOrDefault();

            if (journeeTravailles != null)
                return Ok(journeeTravailles);
            return NotFound("Erreur, la plage horaire n'est pas valide");
        }

        public IActionResult MettreAbsent(string Id)
        {
            PlageHoraire? plage = _context.PlageHoraires.Where(x => x.Id.ToString() == Id).FirstOrDefault();

            if (plage == null)
                return BadRequest("Erreur, la plage horaire n'est pas valide");

            plage.ConfirmationPresence = false;
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
    }
}
