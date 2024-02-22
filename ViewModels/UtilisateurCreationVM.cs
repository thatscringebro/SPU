using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using SPU.Domain;
using Microsoft.AspNetCore.Mvc;

namespace SPU.ViewModels
{
    public class UtilisateurCreationVM
    {
        public string role { get; set; }

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

        [Required(ErrorMessage = "Le mot de passe est requis!")]
        [Display(Name = "Mot de passe")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Le mot de passe doit commencer par une majuscule!")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères!")]
        public string pwd { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise!")]
        [Display(Name = "Confirmation mot de passe")]
        [Compare("pwd", ErrorMessage = "La confirmation du mot de passe ne correspond pas!")]
        public string confirmationpwd { get; set; }

        //[Required(ErrorMessage = "L'école est requis!")]
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

    //public class UniqueUsernameAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        // Récupération de SpuContext à partir du ValidationContext
    //        var spuContext = (SpuContext)validationContext.GetService(typeof(SpuContext));
    //        if (spuContext == null)
    //        {
    //            throw new InvalidOperationException("SpuContext not available");
    //        }

    //        var userName = value as string;

    //        var user = spuContext.Users.FirstOrDefault(u => u.UserName == userName);

    //        if (user != null)
    //        {
    //            return new ValidationResult("Le nom d'utilisateur est déjà utilisé.");
    //        }

    //        return ValidationResult.Success;
    //    }
    //}
}
