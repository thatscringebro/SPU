using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPU.Domain;
using SPU.Domain.Entites;
using SPU.ViewModels;
using System.Security.Claims;

namespace SPU.ViewComponents
{
    public class ScheduleDay : ViewComponent
    {
        private readonly string _loggedUserId;
        private readonly SpuContext _context;
        public ScheduleDay(SpuContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _loggedUserId = claim?.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid horaireId, Guid? mdsId)
        {
            CalendrierHoraireVM calendrier = new CalendrierHoraireVM()
            {
                ListJournees = new List<JourneeTravailleVM>()
            };

            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);

            MDS? mds = new MDS();

            if (coordo != null)
                mds = _context.MDS.FirstOrDefault(x => x.Id == mdsId);
            else
                mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);


            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);

            List<JourneeTravailleVM> journeeTravailles = new List<JourneeTravailleVM>();
            List<PlageHoraire> listePlageHoraire = new List<PlageHoraire>();

            if (mds != null)
            {
                calendrier.role = "mds";
                Horaire horaire = _context.Horaires.Where(x => x.MDSId == mds.Id).FirstOrDefault();

                if (horaire != null)
                { 
                    journeeTravailles = _context.PlageHoraires.Where(x => x.HoraireId == horaireId).ToList().Select(x => new JourneeTravailleVM
                    {
                        DateDebutQuart = x.DateDebut.ToLocalTime(),
                        DateFinQuart = x.DateFin.ToLocalTime(),
                        Id = x.Id,
                        StagiairePresent = x.StagiaireAbsent
                    }).ToList();

                    calendrier.ListJournees = journeeTravailles;
                }
            }
            else if (stag != null)
            {
                calendrier.role = "stagiaire";
                //À vérfier ici
                Horaire horaire = _context.Horaires.Where(x => x.StagiaireId == stag.Id).FirstOrDefault();

                if (horaire != null)
                { 
                    journeeTravailles = _context.PlageHoraires.Where(x => x.HoraireId == horaireId).ToList().Select(x => new JourneeTravailleVM
                    {
                        DateDebutQuart = x.DateDebut.ToLocalTime(),
                        DateFinQuart = x.DateFin.ToLocalTime(),
                        Id = x.Id,
                        StagiairePresent = x.StagiaireAbsent
                    }).ToList();

                    calendrier.ListJournees = journeeTravailles;
                }
            }

            // Aller chercher toutes les journées où la personne connectée travaille
            //List<JourneeTravailleVM> donneesTest = new List<JourneeTravailleVM>();

            // Ajouter des données de test
            //donneesTest.Add(new JourneeTravailleVM
            //{
            //    Id = Guid.NewGuid(),
            //    DateDebutQuart = new DateTime(2024, 1, 1, 8, 0, 0), // 1er janvier 2024 à 8h00
            //    DateFinQuart = new DateTime(2024, 1, 1, 16, 30, 0), // 1er janvier 2024 à 12h00
            //    Present = true
            //});

            //donneesTest.Add(new JourneeTravailleVM
            //{
            //    Id = Guid.NewGuid(),
            //    DateDebutQuart = new DateTime(2024, 1, 2, 13, 0, 0), // 2 janvier 2024 à 13h00
            //    DateFinQuart = new DateTime(2024, 1, 2, 21, 0, 0), // 2 janvier 2024 à 17h00
            //    Present = true
            //});
            //donneesTest.Add(new JourneeTravailleVM
            //{
            //    Id = Guid.NewGuid(),
            //    DateDebutQuart = new DateTime(2024, 1, 3, 20, 0, 0), // 3 janvier 2024 à 20h00
            //    DateFinQuart = new DateTime(2024, 1, 4, 3, 0, 0), // 4 janvier 2024 à 3h00
            //    Present = true
            //});
            //donneesTest.Add(new JourneeTravailleVM
            //{
            //    Id = Guid.NewGuid(),
            //    DateDebutQuart = new DateTime(2024, 1, 4, 20, 0, 0), // 4 janvier 2024 à 20h00
            //    DateFinQuart = new DateTime(2024, 1, 5, 3, 0, 0), // 5 janvier 2024 à 3h00
            //    Present = true
            //});
            // Renvoyer la vue avec les données
            return View(calendrier);
        }

    }
}
