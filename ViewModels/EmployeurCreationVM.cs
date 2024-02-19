using FluentValidation;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class EmployeurCreationVM 
    {
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
        [Display(Name = "Numéro de rue")]
        public string NumeroDeRue { get; set; }
        [Display(Name = "Nom de rue")]
        public string NomDeRue { get; set; }
        [Display(Name = "Ville")]
        public string ville { get; set; }
        [Display(Name = "Province")]
        public string province { get; set; }
        [Display(Name = "Pays")]
        public string pays { get; set; }
        [Display(Name = "Code postal")]
        public string codePostal { get; set; }
        [Display(Name = " Courriel")]
        public string Email { get; set; }
        public string role { get; set; }
    }

    public class EmployeurCreationVMValidation : AbstractValidator<EmployeurCreationVM>
    {
        public EmployeurCreationVMValidation()
        {
            RuleFor(vm => vm.userName).NotEmpty().WithMessage("Le nom d'utilisateur est requis!");

            RuleFor(vm => vm.Prenom).NotEmpty().WithMessage("Le prénom est requis!");

            RuleFor(vm => vm.Nom).NotEmpty().WithMessage("Le nom est requis!");

            RuleFor(vm => vm.PhoneNumber)
    .NotEmpty().WithMessage("Le numéro de cellulaire est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.pwd).NotEmpty().WithMessage("Le mot de passe est requis!")
                .Matches(new Regex(@"^[A-Z]")).WithMessage("Le mot de passe doit commencer par une majuscule!")
                .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères!")
                .Matches(new Regex(@"[!@#$%^&*(),.?\"":{ } |<>]")).WithMessage("Le mot de passe doit contenir au moins un caractère spécial!");

            RuleFor(vm => vm.confirmationpwd)
                .Equal(vm => vm.pwd).WithMessage("La confirmation du mot de passe ne correspond pas!");

            RuleFor(vm => vm.NumeroDeRue).NotEmpty().WithMessage("Le numéro de rue est requis!");

            RuleFor(vm => vm.NomDeRue).NotEmpty().WithMessage("Le nom de rue est requis!");

            RuleFor(vm => vm.ville).NotEmpty().WithMessage("La ville est requise!");

            RuleFor(vm => vm.province).NotEmpty().WithMessage("La province est requise!");

            RuleFor(vm => vm.pays).NotEmpty().WithMessage("Le pays est requis!");

            RuleFor(vm => vm.codePostal)
                .NotEmpty().WithMessage("Le code postal est requis!")
                .Matches(new Regex(@"^[A-Z]{1}\d{1}[A-Z]{1} \d{1}[A-Z]{1}\d{1}$")).WithMessage("Le format du code postal est invalide! Utilisez le format A1B 2C3.");

            RuleFor(vm => vm.Email)
                .NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");
        }
    }
}
