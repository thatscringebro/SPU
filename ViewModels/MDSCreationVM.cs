using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class MDSCreationVM 
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis!")]
        [Display(Name = "Nom d'utilisateur")]
        [Remote(action: "VerifierUsername", controller: "Compte", HttpMethod = "POST", ErrorMessage = "Cet utilisateur existe déjà.")]

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

        [Required(ErrorMessage = "Le numéro de matricule est requis!")]
        [Display(Name = "Numéro de matricule")]
        [RegularExpression(@"^[A-Z]{1}\d{4}$", ErrorMessage = "Le format du matricule est invalide! Utilisez le format X1234.")]
        public string MatriculeId { get; set; }


        [Required(ErrorMessage = "La civilité est requise!")]
        [Display(Name = "Civilité")]
        public Civilite civilite { get; set; }


        [Required(ErrorMessage = "Le type de l'employeur est requis!")]
        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone à la maison est requis!")]
        [Display(Name = "Téléphone de maison")]
        [RegularExpression(@"^\s*\d{3}-\d{3}-\d{4}\s*$", ErrorMessage = "Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.")]
        public string telMaison { get; set; }

        [Required(ErrorMessage = "Le nom de l'employeur est requis!")]
        [Display(Name = "Nom de l'employeur")]
        public string? NomEmployeur { get; set; }
        public string role { get; set; }
        [Required(ErrorMessage = "Le courriel est requis!")]
        [EmailAddress(ErrorMessage = "Le courriel n'est pas valide!")]
        [Display(Name = "Courriel")]
        public string Email { get; set; }

        [Display(Name = "Veuillez choisir une entreprise")]
        public Guid idEmployeurSelectionne { get; set; }

        public List<SelectListItem> Employeurs { get; set; } = new List<SelectListItem>();

    }
}
