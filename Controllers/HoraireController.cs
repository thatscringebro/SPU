using Microsoft.AspNetCore.Mvc;
using SPU.ViewModels;

namespace SPU.Controllers
{
    public class HoraireController : Controller
    {
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
    }
}
