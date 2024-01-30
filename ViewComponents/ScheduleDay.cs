using Microsoft.AspNetCore.Mvc;

namespace SPU.ViewComponents
{
    public class ScheduleDay : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {


            // Renvoyer la vue avec les données
            return View();
        }

    }
}
