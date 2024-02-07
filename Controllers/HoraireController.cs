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

        public IActionResult AjoutPlageHoraireMDS()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjoutPlageHoraireMDS(PlageHoraireMdsVM vm)
        {
            vm.id = Guid.NewGuid();

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

            //Save context


            return RedirectToAction("Index", "Horaire");
        }

        // Lorsque le maître de stage à confirmer tous les plages horaires sur 2 semaines, il appuie sur confirmer dans la vue horaire
        public IActionResult ConfirmationHoraireMDS()
        {
            return RedirectToAction("Index", "Horaire");
        }


        // Ajout d'une confirmation
        public IActionResult ModifierPlageHoraire()
        {
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
    }
}
