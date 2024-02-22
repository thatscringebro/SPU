using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class EmployeurEditVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est requis!")]
        [Display(Name = "Nom d'utilisateur")]
        [Remote(action: "VerifierUsername", controller: "Compte", HttpMethod = "POST", ErrorMessage = "Cet utilisateur existe déjà.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 25 caractères.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis!")]
        [Display(Name = "Prénom")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 25 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis!")]
        [Display(Name = "Nom")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 25 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le numéro de cellulaire est requis!")]
        [Display(Name = "Numéro de cellulaire")]
        [RegularExpression(@"^\s*\d{3}-\d{3}-\d{4}\s*$", ErrorMessage = "Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "Mot de passe")]
        //public string pwd { get; set; }
        //[Display(Name = "Confirmation mot de passe")]
        //public string confirmationpwd { get; set; }
        [Required(ErrorMessage = "Le numéro de rue est requis!")]
        [Display(Name = "Numéro de rue")]
        public string NumeroDeRue { get; set; }
        [Required(ErrorMessage ="Le nom de rue est requis!")]
        [Display(Name = "Nom de rue")]
        public string NomDeRue { get; set; }
        [Required(ErrorMessage = "La ville est requise!")]
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        [Required(ErrorMessage = "La province est requise!")]
        [Display(Name = "Province")]
        public string Province { get; set; }
        [Required(ErrorMessage = "Le pays est requis!")]
        [Display(Name = "Pays")]
        public string Pays { get; set; }

        [Required(ErrorMessage = "Le code postal est requis!")]
        [Display(Name = "Code postal")]
        [RegularExpression(@"^[A-Z]{1}\d{1}[A-Z]{1} \d{1}[A-Z]{1}\d{1}$", ErrorMessage = "Le format du code postal est invalide! Utilisez le format A1B 2C3.")]
        public string CodePostal { get; set; }
        [Required(ErrorMessage = "L'email est requis!")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide!")]
        [Display(Name = " Courriel")]
        public string Email { get; set; }

    }

}
