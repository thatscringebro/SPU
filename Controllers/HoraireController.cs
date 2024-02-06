using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPU.Domain;
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

            return View();
        }

        public IActionResult PlageHoraireMDS()
        {
            //PlageHoraireMdsVM vmPlageHoraireTest = new PlageHoraireMdsVM();
            
            //vmPlageHoraireTest.id = Guid.NewGuid();
            //vmPlageHoraireTest.HeureDebut

            return View();
        }

        [HttpPost]
        public IActionResult AjoutPlageHoraireMDS(PlageHoraireMdsVM vm)
        {
            vm.id = Guid.NewGuid();

            return RedirectToAction("Index", "Horaire");
        }

        // Ajout d'une confirmation
        public IActionResult ConfirmationHoraireMDS()
        {
            return RedirectToAction("Index", "Horaire");
        }

        public async Task<IActionResult> ObtenirInfoPlageHoraire(string Id)
        {
            JourneeTravailleVM? journeeTravailles = _context.PlageHoraires.Where(x => x.horaire.Id.ToString() == Id).Select(x => new JourneeTravailleVM
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
    }
}
