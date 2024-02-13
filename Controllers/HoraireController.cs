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


        public IActionResult Index(ListeHoraireVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            if (mds != null)
            {
                vm.listeHoraireMds = _context.Horaires.Where(x => x.MDSId == mds.Id).ToList();
            }

            return View(vm);
        }


        public IActionResult Horaire(Guid horaireId, HorairePageVM vm)
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);
            Horaire horaire = new Horaire();

            if (user != null)
            {
                Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
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
            return View(); 
        }


        [HttpPost]
        public IActionResult AjoutNouvelHoraireMDS(AjoutNouvelHoraireMdsVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            DateTime test = DateTime.MinValue;

            //Doit entrer le id de la personne
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            Horaire nouvelleHoraire = new Horaire();
            
            if (mds != null)
            {
                nouvelleHoraire.mds = mds;
                nouvelleHoraire.MDSId = mds.Id;
                nouvelleHoraire.Id = Guid.NewGuid();

                //Données temporaires
                nouvelleHoraire.DateDebutStage = vm.DateTimeDebutStage.ToUniversalTime();
                nouvelleHoraire.DateFinStage = vm.DateTimeFinStage.ToUniversalTime();

                _context.Add(nouvelleHoraire);
                _context.SaveChanges();

                ViewBag.horaireId = nouvelleHoraire.Id;
            }

            return RedirectToAction("Horaire", "Horaire", new { horaireId = nouvelleHoraire.Id });
        }

        public IActionResult AjoutPlageHoraireMDS()
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            if (mds != null)
            {
                Horaire horaire = _context.Horaires.Where(x => x.MDSId == mds.Id).FirstOrDefault();
                ViewBag.horaireId = horaire.Id;
            }

            return View();
        }

        [HttpPost]
        public IActionResult AjoutPlageHoraireMDS(PlageHoraireMdsVM vm, Guid horaireId)
        {
            //Si il y a de la récurrence
            if(vm.Recurrence)
            {
                Horaire horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();

                if(horaire != null)
                {
                    DateTime dateDebutStage = horaire.DateDebutStage.ToUniversalTime();
                    DateTime dateFinStage = horaire.DateFinStage.ToUniversalTime();

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

                    while (plageHoraireDebut <= dateFinStage && plageHoraireFin <= dateFinStage)
                    {
                        PlageHoraire plageHoraireRecurrence = new PlageHoraire();
                        plageHoraireRecurrence.Id = Guid.NewGuid();
                        plageHoraireRecurrence.HoraireId = horaireId;
                        plageHoraireRecurrence.DateDebut = plageHoraireDebut;
                        plageHoraireRecurrence.DateFin = plageHoraireFin;
                        plageHoraireRecurrence.ConfirmationPresence = true;

                        _context.Add(plageHoraireRecurrence);
                        _context.SaveChanges();

                        plageHoraireDebut = plageHoraireDebut.AddDays(14);
                        plageHoraireFin = plageHoraireFin.AddDays(14);
                    }
                }
            }
            else
            {
                vm.id = Guid.NewGuid();

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

                //Save context
                PlageHoraire ph = new PlageHoraire();
                ph.Id = Guid.NewGuid();
                ph.HoraireId = horaireId;
                ph.DateDebut = plageHoraireDebut;
                ph.DateFin = plageHoraireFin;
                ph.ConfirmationPresence = true;

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
            if(actionType == "supprimer")
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
