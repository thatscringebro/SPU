using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
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
            Employeur? employeur = _context.Employeurs.FirstOrDefault(x => x.UtilisateurId == user.Id);

            HoraireIndexVM vm = new HoraireIndexVM();

            //Liste horaire maître de stage
            if (mds != null)
            {
                Horaire? horaire = _context.Horaires.Where(x => 
                (x.MDSId1 == mds.Id && x.StagiaireId == null) ||
                (x.MDSId1 == mds.Id && x.StagiaireId != null) || 
                (x.MDSId2 == mds.Id && x.StagiaireId != null)).FirstOrDefault();
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
            else if (ens != null)
            {
                vm.role = "enseignant";

                List<Stagiaire?> listeStagiaire = _context.Stagiaires.Where(x => x.EnseignantId == ens.Id).Include(c => c.utilisateur).ToList();

                foreach (var item in listeStagiaire)
                {
                    Horaire? horaireStagiaire = _context.Horaires.Where(x => x.StagiaireId == item.Id).FirstOrDefault();
                    AssociationStagiaireEnseignantVM association = new AssociationStagiaireEnseignantVM();

                    MDS trouveMdsAssocierHoraire = new MDS();
                    trouveMdsAssocierHoraire = _context.MDS.Where(x => x.StagiaireId == item.Id).FirstOrDefault();

                    if(trouveMdsAssocierHoraire != null)
                    {
                        Horaire? horaireMds = _context.Horaires.Where(x => x.MDSId1 == trouveMdsAssocierHoraire.Id).FirstOrDefault();

                        if(horaireMds != null)
                        {
                            if(horaireStagiaire != null)
                            {
                                association.dateDebutStage = item.debutStage ?? DateTime.MinValue;
                                association.dateFinStage = item.finStage ?? DateTime.MinValue;
                                association.idHoraire = horaireStagiaire.Id;
                                association.nomStagiaire = item.utilisateur.NomComplet;
                                association.idHoraireMDS = horaireMds.Id;

                                vm.associationStagEns.Add(association);
                            }
                        }
                    }

                }

                return View(vm);
            }

            //Employeur
            else if(employeur != null)
            {
                vm.role = "employeur";

                //Horaire? horaireMds = _context.Horaires.Where(x => x.MDSId == item.Id).FirstOrDefault();

                //List<MDS?> listeMds = _context.MDS.Where(x => x.EmployeurId == employeur.Id).Include(c => c.utilisateur).ToList();

                return View(vm);
            }


            return View(vm);
        }

        [Authorize]
        public IActionResult Horaire(Guid horaireId, HorairePageVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);
            Horaire horaire = new Horaire();

            horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();
            if(horaire == null)
            {
                TempData["ErrorMessage"] = "Horaire vide";
                return View();
            }
            else
            {

            ViewBag.horaireId = horaire.Id;
            ViewBag.MDSId1 = null;
            }

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

                MDS mdsHoraire = _context.MDS.Where(x => x.Id == horaire.MDSId1).Include(c => c.utilisateur).FirstOrDefault();
                Stagiaire? stagHoraire = _context.Stagiaires.Where(x => x.Id == horaire.StagiaireId).Include(c => c.utilisateur).FirstOrDefault();

                MDS? mdsHoraire2 = _context.MDS.Where(x => x.Id == horaire.MDSId2).Include(c => c.utilisateur).FirstOrDefault();

                vm.nomMds = string.Concat(mdsHoraire.utilisateur.Prenom + " " + mdsHoraire.utilisateur.Nom);
                vm.nomStagiaire = string.Concat(stagHoraire?.utilisateur.Prenom + " " + stagHoraire?.utilisateur.Nom);
                vm.nomMds2 = string.Concat(mdsHoraire2.utilisateur.Prenom + " " + mdsHoraire2.utilisateur.Nom);

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
        [Authorize]
        public IActionResult HoraireMds(Guid userId, HorairePageVM vm)
        {
            //var vm = new HorairePageVM();

            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id == userId);
            Horaire horaire = new Horaire();

            var Mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == userId);
            //Guid horaireId = _context.Horaires.Where(x => x.MDSId1 == Mds.Id).Select(h => h.Id).FirstOrDefault();
            horaire = _context.Horaires.Where(x => x.MDSId1 == Mds.Id).FirstOrDefault();

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
            ViewBag.MDSId1 = Mds.Id;

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

                MDS mdsHoraire = _context.MDS.Where(x => x.Id == horaire.MDSId1).Include(c => c.utilisateur).FirstOrDefault();
                Stagiaire? stagHoraire = _context.Stagiaires.Where(x => x.Id == horaire.StagiaireId).Include(c => c.utilisateur).FirstOrDefault();
                MDS? mdsHoraire2 = _context.MDS.Where(x => x.Id == horaire.MDSId2).Include(c => c.utilisateur).FirstOrDefault();


                vm.nomMds = string.Concat(mdsHoraire.utilisateur.Prenom + " " + mdsHoraire.utilisateur.Nom);
                vm.nomStagiaire = string.Concat(stagHoraire?.utilisateur.Prenom + " " + stagHoraire?.utilisateur.Nom);
                vm.nomMds2 = string.Concat(mdsHoraire2.utilisateur.Prenom + " " + mdsHoraire2.utilisateur.Nom);


                vm.DateCreationHoraire = mdsHoraire.DateCreationHoraire;
                vm.DateExpiration = mdsHoraire.DateExpiration;

                if(stagHoraire!= null)
                {
                    vm.DateDebutStage = stagHoraire.debutStage;
                    vm.DateFinStage = stagHoraire.finStage;
                }
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
        //    nouvelleHoraire.MDSId1 = mds.Id;

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
        [Authorize]
        public IActionResult AjoutPlageHoraireMDS(Guid horaireId)
        {
            ViewBag.horaireId = horaireId;

            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AjoutPlageHoraireMDS(PlageHoraireMdsVM vm, Guid horaireId)
        {
            ViewBag.horaireId = horaireId;
            var idMDS = new Guid(_loggedUserId);
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            Horaire horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();
            MDS mds = _context.MDS.Where(x => x.UtilisateurId == idMDS).FirstOrDefault();

            
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

                    while (plageHoraireDebut >= mds.DateCreationHoraire.ToLocalTime() && plageHoraireFin <= mds.DateExpiration.ToLocalTime())
                    {
                        PlageHoraire plageHoraireRecurrence = new PlageHoraire();
                        plageHoraireRecurrence.Id = Guid.NewGuid();
                        plageHoraireRecurrence.HoraireId = horaireId;
                        plageHoraireRecurrence.DateDebut = plageHoraireDebut;
                        plageHoraireRecurrence.DateFin = plageHoraireFin;
                        plageHoraireRecurrence.StagiairePresent = true;

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

                        if (plageHoraireRecurrence.DateDebut.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime() || plageHoraireRecurrence.DateFin.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime()
                            || plageHoraireRecurrence.DateDebut.ToLocalTime() > mds.DateExpiration.ToLocalTime() || plageHoraireRecurrence.DateFin.ToLocalTime() > mds.DateExpiration.ToLocalTime())
                        {
                            string errorMessage = "La date et l'heure de la plage horaire doivent correspondre aux dates de début et de fin d'assignation du maître de stage. (entre le " +
                                        (mds.DateCreationHoraire.ToString("yyyy-MM-dd HH:mm:ss")) +
                                        " et le " +
                                        (mds.DateExpiration.ToString("yyyy-MM-dd HH:mm:ss"));
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
                ph.StagiairePresent = true;

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

                if (ph.DateDebut.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime() || ph.DateFin.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime()
                    || ph.DateDebut.ToLocalTime() > mds.DateExpiration.ToLocalTime() || ph.DateFin.ToLocalTime() > mds.DateExpiration.ToLocalTime())
                {
                    string errorMessage = "La date et l'heure de la plage horaire doivent correspondre aux dates de début et de fin d'assignation du maître de stage. (entre le " + 
                        (mds.DateCreationHoraire.ToString("yyyy-MM-dd HH:mm:ss")) + " et le " + (mds.DateExpiration.ToString("yyyy-MM-dd HH:mm:ss"));
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
        [Authorize]
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
            vm.EstPresent = ph.StagiairePresent;

            ViewBag.PlageHoraireId = idPlageHoraire;

            return View(vm);
        }

        [HttpPost]
        public IActionResult ModifierPlageHoraire(ModifierPlageHoraireVM vm, string actionType, Guid PlageHoraireId)
        {
            ViewBag.PlageHoraireId = PlageHoraireId;
            var idMDS = new Guid(_loggedUserId);
            PlageHoraire phBD = _context.PlageHoraires.FirstOrDefault(x => x.Id == PlageHoraireId);
            PlageHoraire ph = new PlageHoraire();
            Horaire horaire = _context.Horaires.FirstOrDefault(x => x.Id == phBD.HoraireId);
            MDS mds = _context.MDS.Where(x => x.Id == idMDS).FirstOrDefault();

            if (actionType == "annuler")
            {
                return RedirectToAction("Horaire", "Horaire", new { horaireId = horaire.Id });
            }

            if (actionType == "supprimer")
            {
                // Récupérer la plage horaire existante depuis la base de données


                if (phBD != null)
                {
                    _context.PlageHoraires.Remove(phBD);
                    _context.SaveChanges();

                    // Rediriger vers l'action "Horaire" du contrôleur "Horaire"
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = phBD.HoraireId });
                }
                else
                {
                    // Gérer le cas où la plage horaire n'est pas trouvée
                    // Vous pouvez renvoyer une vue avec un message d'erreur approprié, par exemple.
                    return NotFound();
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            if (actionType == "modifier")
            {
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


                Guid horaireId = _context.PlageHoraires.Where(x => x.Id == PlageHoraireId).FirstOrDefault().HoraireId;

                ph.HoraireId = horaireId;
                ph.DateDebut = plageHoraireDebut;
                ph.DateFin = plageHoraireFin;

                // Vérifier si la plage horaire existe
                if (ph != null)
                {

                    PlageHoraire verificationPlageHoraire = _context.PlageHoraires
                    .FirstOrDefault(x =>
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin > x.DateDebut.ToLocalTime()) ||
                        (ph.DateDebut >= x.DateDebut.ToLocalTime() && ph.DateFin <= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut < x.DateFin.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime()) ||
                        (ph.DateDebut <= x.DateDebut.ToLocalTime() && ph.DateFin >= x.DateFin.ToLocalTime())
                    );

                    if (verificationPlageHoraire.DateDebut != phBD.DateDebut && verificationPlageHoraire.DateFin != phBD.DateFin)
                    {
                        if (verificationPlageHoraire != null)
                        {
                            string errorMessage = "La plage horaire chevauche une autre plage horaire existante.";
                            ModelState.AddModelError("PlageHoraire", errorMessage);
                            TempData["ErrorMessage"] = errorMessage;
                            return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                        }

                        if (ph.DateDebut.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime() || ph.DateFin.ToLocalTime() < mds.DateCreationHoraire.ToLocalTime()
                            || ph.DateDebut.ToLocalTime() > mds.DateExpiration.ToLocalTime() || ph.DateFin.ToLocalTime() > mds.DateExpiration.ToLocalTime())
                        {
                            string errorMessage = "La date et l'heure de la plage horaire doivent correspondre aux dates de début et de fin d'assignation du maître de stage. (entre le " +
                                (mds.DateCreationHoraire.ToString("yyyy-MM-dd HH:mm:ss")) +
                                " et le " +
                                (mds.DateExpiration.ToString("yyyy-MM-dd HH:mm:ss"));  
                            ModelState.AddModelError("PlageHoraire", errorMessage);
                            TempData["ErrorMessage"] = errorMessage;
                            return RedirectToAction("Horaire", "Horaire", new { horaireId = horaireId });
                        }
                    }

                    // Mettre à jour les propriétés de la plage horaire avec les nouvelles valeurs
                    phBD.HoraireId = horaireId;
                    phBD.Commentaire = vm.Commentaire;
                    //phBD.ConfirmationPresence = vm.EstPresent;
                    phBD.DateDebut = plageHoraireDebut.ToUniversalTime();
                    phBD.DateFin = plageHoraireFin.ToUniversalTime();

                    //ICI CLAUDEL POUR LA MODIF DE LA PLAGE HORAIRE
                    if (!vm.EstPresent) //Verifier si il est deja dans la BD
                    {
                        if (phBD.MDS1absent == null)
                        {
                            phBD.MDS1absent = new Guid(_loggedUserId);
                        }
                        else if (phBD.MDS2absent == null)
                        {
                            phBD.MDS2absent = new Guid(_loggedUserId);
                        }
                    }
                    else if (vm.EstPresent && phBD.MDS1absent != null)
                    {
                        phBD.MDS1absent = null;
                    }
                    {
                        phBD.MDS2absent = null;
                    }


                    // Enregistrer les modifications dans la base de données
                    _context.Update(phBD);
                    _context.SaveChanges();

                    // Rediriger vers l'action "Horaire" du contrôleur "Horaire"
                    return RedirectToAction("Horaire", "Horaire", new { horaireId = phBD.HoraireId });
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
        [Authorize]
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
                StagiairePresent = x.StagiairePresent
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

            plage.StagiairePresent = false;
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

        //reprendre une journee si le stagiaire s'absente
        [Authorize]
        public IActionResult ReprendreJournee()
        {
            Stagiaire? user = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId.ToString() == _loggedUserId);
            if (user == null)
                return BadRequest("La plage horaire sélectionnée n'est pas valide. La journée n'à pas pu être reprise");


            if (user.finStage.HasValue)
            {
                List<PlageHoraire> listPlagesHoraire = _context.PlageHoraires.Include(x => x.horaire).ThenInclude(x => x.stagiaire).Where(x => x.horaire.stagiaire.UtilisateurId.ToString() == _loggedUserId).ToList();

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

        //Afficher form de remplacent
        [Authorize]
        [HttpGet]
        public IActionResult MdsRemplacement(RemplacementPlageHoraireVM vm, Guid idPlageHoraire)
        {
            PlageHoraire? plageHoraire = _context.PlageHoraires.Where(x => x.Id == idPlageHoraire).FirstOrDefault();
            if (plageHoraire == null)
                return BadRequest("Erreur, plage horaire invalide");

            if (plageHoraire.remplacant != null)
            {
                string[] listMatricule = plageHoraire.remplacant.Split(',');
                vm.MatriculeRemplacent1 = listMatricule[0];
                if(listMatricule.Length > 1)
                    vm.MatriculeRemplacent1 = listMatricule[1];
            }

            vm.DateDebut = plageHoraire.DateDebut.ToLocalTime();
            vm.DateFin = plageHoraire.DateFin.ToLocalTime();
            vm.StagiairePresent = plageHoraire.StagiairePresent;

            ViewBag.PlageHoraireId = idPlageHoraire;

            return View(vm);
        }
        //Remplacent
        [Authorize]
        [HttpPost]
        public IActionResult MdsRemplacement(RemplacementPlageHoraireVM vm,  string PlageHoraireId, string actionType)
        {
            if(actionType == "annuler")
                return RedirectToAction("Index", "Horaire");

            PlageHoraire? plageHoraire = _context.PlageHoraires.FirstOrDefault(x => x.Id.ToString() == PlageHoraireId);
            if (plageHoraire == null)
                return BadRequest("Erreur, plage horaire invalide");

            plageHoraire.StagiairePresent = vm.StagiairePresent;

            if (vm.MatriculeRemplacent1 != null && vm.MatriculeRemplacent2 == null)
                plageHoraire.remplacant = vm.MatriculeRemplacent1;
            else if (vm.MatriculeRemplacent1 == null && vm.MatriculeRemplacent2 != null)
                plageHoraire.remplacant = vm.MatriculeRemplacent2;
            else if (vm.MatriculeRemplacent1 != null && vm.MatriculeRemplacent2 != null)
                plageHoraire.remplacant = vm.MatriculeRemplacent1 + "," + vm.MatriculeRemplacent2;
            else
                plageHoraire.remplacant = null;

            _context.Update(plageHoraire);
            _context.SaveChanges();

            return RedirectToAction("Index", "Horaire");
        }
    }
}
