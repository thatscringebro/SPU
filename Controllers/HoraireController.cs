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
        public IActionResult Index()
        {
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            if(user != null)
            {
                Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
                Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
                MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

                if (mds != null)
                {
                    Horaire horaire = _context.Horaires.Where(x => x.MDSId == mds.Id).FirstOrDefault();

                    if(horaire != null)
                        ViewBag.horaireId = horaire.Id;
                }
            }

            return View();
        }

        

        [HttpPost]
        public IActionResult AjoutNouvelHoraireMDS()
        {
            //Doit entrer le id de la personne
            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonateur? coordo = _context.Coordonateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);
            MDS? mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);

            if(mds != null)
            {
                Horaire nouvelleHoraire = new Horaire();
                nouvelleHoraire.mds = mds;
                nouvelleHoraire.MDSId = mds.Id;
                nouvelleHoraire.Id = Guid.NewGuid();

                //Données temporaires
                nouvelleHoraire.DateDebutStage = new DateTime(2024, 2, 12, 0, 0, 0).ToUniversalTime();
                nouvelleHoraire.DateFinStage = new DateTime(2024, 4, 8, 0, 0, 0).ToUniversalTime();

                _context.Add(nouvelleHoraire); 
                _context.SaveChanges();
            }

            //nouvelleHoraire.mds = user.Id;
                
            //

            return RedirectToAction("Index", "Horaire");
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

            return RedirectToAction("Index", "Horaire");
        }

        // Lorsque le maître de stage à confirmer tous les plages horaires sur 2 semaines, il appuie sur confirmer dans la vue horaire
        public IActionResult ConfirmationHoraireMDS()
        {
            return RedirectToAction("Index", "Horaire");
        }


        // Ajout d'une confirmation
        public IActionResult ModifierPlageHoraire(Guid horaireId)
        {
            //Aller chercher les informations avec le id de l'horaire et le id du div

            return View();
        }

        [HttpPost]
        public IActionResult ModifierPlageHoraire(ModifierPlageHoraireVM vm, string actionType)
        {
            DateTime plageHoraireDebut = new DateTime(vm.DateDebutPlageHoraire.Year,
                              vm.DateDebutPlageHoraire.Month,
                              vm.DateDebutPlageHoraire.Day,
                              vm.HeureDebutPlageHoraire,
                              vm.MinutesDebutPlageHoraire,
                              0);

            DateTime plageHoraireFin = new DateTime(vm.DateFinPlageHoraire.Year,
                              vm.DateFinPlageHoraire.Month,
                              vm.DateFinPlageHoraire.Day,
                              vm.HeureFinPlageHoraire,
                              vm.MinutesFinPlageHoraire,
                              0);
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
            PlageHoraire? plage = _context.PlageHoraires.Where(x => x.horaire.Id.ToString() == Id).FirstOrDefault();

            if (plage == null)
                return BadRequest("Erreur, la plage horaire n'est pas valide");

            plage.ConfirmationPresence = false;
            try
            {
                _context.PlageHoraires.Update(plage);
                _context.SaveChanges();
                return Ok();
            }
            catch {
                return BadRequest("Erreur, la plage horraire n'a pas pu être modifiée");
            }
        }
    }
}
