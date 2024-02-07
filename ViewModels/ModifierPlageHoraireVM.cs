using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class ModifierPlageHoraireVM
    {
        public Guid id { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateDebutPlageHoraire { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateFinPlageHoraire { get; set; }

        public int HeureDebutPlageHoraire { get; set; }
        public int MinutesDebutPlageHoraire { get; set; }
        public int HeureFinPlageHoraire { get; set; }
        public int MinutesFinPlageHoraire { get; set; }

        public bool EstPresent { get; set; }
        public string? Commentaire { get; set; }
    }
}
