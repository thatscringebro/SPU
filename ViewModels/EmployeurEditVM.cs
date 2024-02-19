using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class EmployeurEditVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Le nom d'utilisateur est requis!")]
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis!")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis!")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage ="Le numéro de téléphone est requis!")]
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "Mot de passe")]
        //public string pwd { get; set; }
        //[Display(Name = "Confirmation mot de passe")]
        //public string confirmationpwd { get; set; }

        [Required(ErrorMessage ="Le nom de rue est requis!")]
        [Display(Name = "Numéro de rue")]
        public string NumeroDeRue { get; set; }
        [Display(Name = "Nom de rue")]
        public string NomDeRue { get; set; }
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        [Display(Name = "Province")]
        public string Province { get; set; }
        [Display(Name = "Pays")]
        public string Pays { get; set; }
        [Display(Name = "Code postal")]
        public string CodePostal { get; set; }
        [Display(Name = " Courriel")]
        public string Email { get; set; }

    }

    public class EmployeurEditVMValidation : AbstractValidator<EmployeurEditVM>
    {
        public EmployeurEditVMValidation()
        {
            RuleFor(vm => vm.PhoneNumber)
    .NotEmpty().WithMessage("Le numéro de cellulaire est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.NumeroDeRue).NotEmpty().WithMessage("Le numéro de rue est requis!");

            RuleFor(vm => vm.NomDeRue).NotEmpty().WithMessage("Le nom de rue est requis!");

            RuleFor(vm => vm.Ville).NotEmpty().WithMessage("La ville est requise!");

            RuleFor(vm => vm.Province).NotEmpty().WithMessage("La province est requise!");

            RuleFor(vm => vm.Pays).NotEmpty().WithMessage("Le pays est requis!");

            RuleFor(vm => vm.CodePostal)
                .NotEmpty().WithMessage("Le code postal est requis!")
                .Matches(new Regex(@"^[A-Z]{1}\d{1}[A-Z]{1} \d{1}[A-Z]{1}\d{1}$")).WithMessage("Le format du code postal est invalide! Utilisez le format A1B 2C3.");

            RuleFor(vm => vm.Email)
                .NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");
        }
    }
}
