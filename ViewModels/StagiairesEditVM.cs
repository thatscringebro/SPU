using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class StagiairesEditVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Le prénom est requis.")]
        [Display(Name = "Prénom")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 25 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [Display(Name = "Nom")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 25 caractères.")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "La date de début est requis.")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true), Display(Name = "Début de stage ")]
        public DateTime? debutStage { get; set; }
        [Required(ErrorMessage = "La date de fin est requise.")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true), Display(Name = "Fin de stage")]
        public DateTime? finStage { get; set; }

        [Display(Name = "Choisir maître de stage 1")]
        public Guid? idMdsSelectionne1 { get; set; }

        [Display(Name = "Choisir maître de stage 2")]
        public Guid? idMdsSelectionne2 { get; set; }

        public List<SelectListItem> Mds { get; set; } = new List<SelectListItem>();

        [Display(Name = "Choisir l'enseignant")]
        public Guid? idEnseignantSelectionne { get; set; }

        public List<SelectListItem> Enseignants { get; set; } = new List<SelectListItem>();
        //public Horaire? horaire { get; set; }
    }

}
