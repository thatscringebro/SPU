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

        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Mot de passe")]
        public string pwd { get; set; }
        [Display(Name = "Confirmation mot de passe")]
        public string confirmationpwd { get; set; }
        [Display(Name = "Veuillez choisir un établissement")]
        public Guid idEcoleSelectionne { get; set; }
        [Display(Name = " Courriel")]
        public string Email { get; set; }

        [Display(Name = "Voulez-vous partager vos informations de contact")]
        public bool PartagerInfoContact { get; set; }

        public List<SelectListItem> Ecoles { get; set; } = new List<SelectListItem>();

    }

    public class UtilisateurCreationVMValidation : AbstractValidator<UtilisateurCreationVM>
    {
        public UtilisateurCreationVMValidation()
        {
            RuleFor(vm => vm.userName).NotEmpty().WithMessage("Le nom d'utilisateur est requis!");

            RuleFor(vm => vm.Prenom).NotEmpty().WithMessage("Le prénom est requis!");

            RuleFor(vm => vm.Nom).NotEmpty().WithMessage("Le nom est requis!");

            RuleFor(vm => vm.PhoneNumber)
    .NotEmpty().WithMessage("Le numéro de cellulaire est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.pwd).NotEmpty().WithMessage("Le mot de passe est requis!")
                 .Matches(new Regex(@"^[A-Z]")).WithMessage("Le mot de passe doit commencer par une majuscule!")
                 .Matches(new Regex(@"[!@#$%^&*(),.?\"":{ } |<>]")).WithMessage("Le mot de passe doit contenir au moins un caractère spécial!")
                 .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères!");
                 
            RuleFor(vm => vm.confirmationpwd).Equal(vm => vm.pwd).WithMessage("La confirmation du mot de passe ne correspond pas!");

            RuleFor(vm => vm.idEcoleSelectionne).NotEmpty().WithMessage("Veuillez choisir un établissement!");

            RuleFor(vm => vm.Email).NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");

            RuleFor(vm => vm.PartagerInfoContact).NotNull().WithMessage("Veuillez indiquer si vous voulez partager vos informations de contact!");

        }
    }    
}
