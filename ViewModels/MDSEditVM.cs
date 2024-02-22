using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class MDSEditVM
    {
        public Guid Id { get; set; }
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
        [Display(Name = "Numéro de matricule")]
        [Required(ErrorMessage = "Le numéro de matricule est requis!")]
        [RegularExpression(@"^[A-Z]{1}\d{4}$", ErrorMessage = "Le format du matricule est invalide! Utilisez le format X1234.")]
        public string MatriculeId { get; set; }

        [Display(Name = "Civilité")]
        public Civilite Civilite { get; set; }

        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone à la maison est requis!")]
        [Display(Name = "Téléphone de maison")]
        [RegularExpression(@"^\s*\d{3}-\d{3}-\d{4}\s*$", ErrorMessage = "Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.")]
        public string telMaison { get; set; }
        [Required(ErrorMessage = "Le courriel est requis!")]
        [EmailAddress(ErrorMessage = "Le courriel n'est pas valide!")]
        [Display(Name = "Courriel")]
        public string Email { get; set; }

        [Display(Name = "Veuillez choisir une entreprise")]
        public Guid? idEmployeurSelectionne { get; set; }

        public List<SelectListItem> Employeurs { get; set; } = new List<SelectListItem>();
    }

}
