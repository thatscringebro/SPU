using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class UtilisateurEditVM
    {
        public string role { get; set; }
        public Guid id { get; set; }
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = " Courriel")]
        public string Email { get; set; }
    }

    public class UtilisateurEditVMValidation : AbstractValidator<UtilisateurEditVM> 
    { 
        public UtilisateurEditVMValidation() 
        {
            RuleFor(vm => vm.userName).NotEmpty().WithMessage("Le nom d'utilisateur est requis!");

            RuleFor(vm => vm.Prenom).NotEmpty().WithMessage("Le prénom est requis!");

            RuleFor(vm => vm.Nom).NotEmpty().WithMessage("Le nom est requis!");

            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty().WithMessage("Le numéro de cellulaire est requis!")
                .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");


            RuleFor(vm => vm.Email).NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");

        }

    }
}
