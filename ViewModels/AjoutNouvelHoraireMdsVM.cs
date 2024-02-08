using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class AjoutNouvelHoraireMdsVM
    {
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeDebutStage { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeFinStage { get; set; }
    }
}
