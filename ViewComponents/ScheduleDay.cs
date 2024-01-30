using Microsoft.AspNetCore.Mvc;

namespace SPU.ViewComponents
{
    public class ScheduleDay : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int Annee = DateTime.Now.Year;
            List<DateTime> Jours = new List<DateTime>();
            for (int mois = 1; mois <= 12; mois++)
            {
                for (int jour = 1; jour <= DateTime.DaysInMonth(Annee, mois); jour++)
                {
                    Jours.Add(new DateTime(Annee, mois, jour));
                }
            }

            // Renvoyer la vue avec les données
            return View(Jours);
        }

    }
}
