using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class UtilisateurEditVM
    {
        public string role { get; set; }
        public Guid id { get; set; }
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

        [Required(ErrorMessage = "Le courriel est requis!")]
        [Display(Name = "Courriel")]
        [EmailAddress(ErrorMessage = "Le courriel n'est pas valide!")]
        public string Email { get; set; }
    }
}
