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

        //instanciation du view component
        public async Task<IViewComponentResult> InvokeAsync(Guid horaireId, Guid? MDSId1)
        {
            CalendrierHoraireVM calendrier = new CalendrierHoraireVM()
            {
                ListJournees = new List<JourneeTravailleVM>()
            };

            Utilisateur? user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

            Coordonnateur? coordo = _context.Coordonnateurs.FirstOrDefault(x => x.UtilisateurId == user.Id);

            MDS? mds = new MDS();

            if (coordo != null)
                mds = _context.MDS.FirstOrDefault(x => x.Id == MDSId1);
            else
                mds = _context.MDS.FirstOrDefault(x => x.UtilisateurId == user.Id);


            Enseignant? ens = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId == user.Id);
            Stagiaire? stag = _context.Stagiaires.FirstOrDefault(x => x.UtilisateurId == user.Id);

            List<JourneeTravailleVM> journeeTravailles = new List<JourneeTravailleVM>();
            List<PlageHoraire> listePlageHoraire = new List<PlageHoraire>();

            //Va cherher toutes les plage horaires en lien avec l'horaire
            if (mds != null)
            {
                calendrier.role = "mds";
                //va trouver l'horaire avec le id de l'horaire
                Horaire? horaire = _context.Horaires.Where(x => x.Id == horaireId).FirstOrDefault();

                if (horaire != null)
                { 
                    journeeTravailles = _context.PlageHoraires.Where(x => x.HoraireId == horaireId).ToList().Select(x => new JourneeTravailleVM
                    {
                        DateDebutQuart = x.DateDebut.ToLocalTime(),
                        DateFinQuart = x.DateFin.ToLocalTime(),
                        Id = x.Id,
                        StagiairePresent = x.StagiairePresent,
                        MdsAbsent1 = x.MDS1absent,
                        MdsAbsent2 = x.MDS2absent,
                    }).ToList();

                    calendrier.ListJournees = journeeTravailles;
                }
            }
            else if (stag != null)
            {
                calendrier.role = "stagiaire";
                //Va trouver l'horaire avec l'id de stagiaire
                Horaire horaire = _context.Horaires.Where(x => x.StagiaireId == stag.Id).FirstOrDefault();

                if (horaire != null)
                { 
                    journeeTravailles = _context.PlageHoraires.Where(x => x.HoraireId == horaireId && x.DateDebut.Date >= stag.debutStage && x.DateDebut.Date <= stag.finStage).ToList().Select(x => new JourneeTravailleVM
                    {
                        DateDebutQuart = x.DateDebut.ToLocalTime(),
                        DateFinQuart = x.DateFin.ToLocalTime(),
                        Id = x.Id,
                        StagiairePresent = x.StagiairePresent,
                        MdsAbsent1 = x.MDS1absent,
                        MdsAbsent2 = x.MDS2absent,
                    }).ToList();

                    calendrier.ListJournees = journeeTravailles;
                }
            }

            // Renvoyer la vue avec les données
            return View(calendrier);
        }

    }
}
