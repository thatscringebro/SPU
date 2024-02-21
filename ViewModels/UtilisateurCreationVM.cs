using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class UtilisateurCreationVM
    {
        public string role { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est requis!")]
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis!")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis!")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le numéro de cellulaire est requis!")]
        [Display(Name = "Numéro de cellulaire")]
        [RegularExpression(@"^\s*\d{3}-\d{3}-\d{4}\s*$", ErrorMessage = "Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis!")]
        [Display(Name = "Mot de passe")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Le mot de passe doit commencer par une majuscule!")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères!")]
        public string pwd { get; set; }

        [Display(Name = "Confirmation mot de passe")]
        [Compare("pwd", ErrorMessage = "La confirmation du mot de passe ne correspond pas!")]
        public string confirmationpwd { get; set; }

        [Display(Name = "Veuillez choisir un établissement")]
        public Guid idEcoleSelectionne { get; set; }

        [Required(ErrorMessage = "Le courriel est requis!")]
        [Display(Name = "Courriel")]
        [EmailAddress(ErrorMessage = "Le courriel n'est pas valide!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Voulez-vous partager vos informations de contact")]
        public bool PartagerInfoContact { get; set; }

        public List<SelectListItem> Ecoles { get; set; } = new List<SelectListItem>();

    }

}
