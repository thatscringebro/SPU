using FluentValidation;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class EmployeurCreationVM 
    {
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

        [Required(ErrorMessage = "La confirmation du mot de passe est requise!")]
        [Compare("pwd", ErrorMessage = "La confirmation du mot de passe ne correspond pas!")]
        [Display(Name = "Confirmation mot de passe")]
        public string confirmationpwd { get; set; }

        [Required(ErrorMessage = "Le numéro de rue est requis!")]
        [Display(Name = "Numéro de rue")]
        public string NumeroDeRue { get; set; }

        [Required(ErrorMessage = "Le nom de rue est requis!")]
        [Display(Name = "Nom de rue")]
        public string NomDeRue { get; set; }

        [Required(ErrorMessage = "La ville est requise!")]
        [Display(Name = "Ville")]
        public string ville { get; set; }

        [Required(ErrorMessage = "La province est requise!")]
        [Display(Name = "Province")]
        public string province { get; set; }

        [Required(ErrorMessage = "Le pays est requis!")]
        [Display(Name = "Pays")]
        public string pays { get; set; }

        [Required(ErrorMessage = "Le code postal est requis!")]
        [Display(Name = "Code postal")]
        [RegularExpression(@"^[A-Z]{1}\d{1}[A-Z]{1} \d{1}[A-Z]{1}\d{1}$", ErrorMessage = "Le format du code postal est invalide! Utilisez le format A1B 2C3.")]
        public string codePostal { get; set; }

        [Required(ErrorMessage = "L'email est requis!")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide!")]
        [Display(Name = " Courriel")]
        public string Email { get; set; }
        public string role { get; set; }
    }
}
