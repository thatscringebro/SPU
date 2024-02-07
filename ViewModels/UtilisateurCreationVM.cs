using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurCreationVM
    {
        public string role { get; set; }
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Le prénom est requis.")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Le nom est requis.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string pwd { get; set; }
        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Compare("pwd", ErrorMessage = "Le mot de passe et la confirmation du mot de passe ne correspondent pas.")]
        [Display(Name = "Confirmation mot de passe")]
        public string confirmationpwd { get; set; }
        [Display(Name = "Veuillez choisir un établissement")]
        public Ecole Ecole { get; set; }
        [Display(Name = "École")]
        [Required(ErrorMessage = "La sélection d'un établissement est requise.")]
        //[Display(Name = "Veuillez choisir un établissement")]
        public int idEcoleSelectionne { get; set; }
        public List<SelectListItem> Ecoles { get; set; } = new List<SelectListItem>();

    }
}
