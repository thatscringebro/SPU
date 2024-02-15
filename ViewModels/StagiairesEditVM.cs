using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class StagiairesEditVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom {  get; set; }



        [Display(Name = "Choisir maître de stage 1")]
        public Guid? idMdsSelectionne1 { get; set; }

        [Display(Name = "Choisir maître de stage 2")]
        public Guid? idMdsSelectionne2 { get; set; }

        public List<SelectListItem> Mds { get; set; } = new List<SelectListItem>();

        [Display(Name = "Choisir l'enseignant")]
        public Guid? idEnseignantSelectionne { get; set; }

        public List<SelectListItem> Enseignants { get; set; } = new List<SelectListItem>();
    }
}
