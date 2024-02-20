using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class MDSEditVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "Mot de passe")]
        //public string pwd { get; set; }
        //[Display(Name = "Confirmation mot de passe")]
        //public string confirmationpwd { get; set; }
        [Display(Name = "Numéro de matricule")]
        public string MatriculeId { get; set; }
        [Display(Name = "Civilité")]
        public Civilite Civilite { get; set; }
        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }
        [Display(Name = "Téléphone de maison")]
        public string telMaison { get; set; }
        //[Display(Name = "Nom de l'employeur")]
        //public string NomEmployeur { get; set; }
        [Display(Name = " Courriel")]
        public string Email { get; set; }

        [Display(Name = "Veuillez choisir une entreprise")]
        public Guid? idEmployeurSelectionne { get; set; }

        public List<SelectListItem> Employeurs { get; set; } = new List<SelectListItem>();
    }

    public class MDSEditVMValidation : AbstractValidator<MDSEditVM>
    {
        public MDSEditVMValidation() 
        {
            RuleFor(vm => vm.userName).NotEmpty().WithMessage("Le nom d'utilisateur est requis!");

            RuleFor(vm => vm.Prenom).NotEmpty().WithMessage("Le prénom est requis!");

            RuleFor(vm => vm.Nom).NotEmpty().WithMessage("Le nom est requis!");

            RuleFor(vm => vm.PhoneNumber)
    .NotEmpty().WithMessage("Le numéro de cellulaire est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.MatriculeId)
            .NotEmpty().WithMessage("Le numéro de matricule est requis!")
            .Matches(new Regex(@"^[A-Z]{1}\d{4}$")).WithMessage("Le format du matricule est invalide! Utilisez le format XXX1234.");

            RuleFor(vm => vm.Civilite).IsInEnum().WithMessage("La civilité est requis!");

            RuleFor(vm => vm.TypeEmployeur).IsInEnum().WithMessage("Le type d'employeur est requis!");

            RuleFor(vm => vm.telMaison)
    .NotEmpty().WithMessage("Le numéro de téléphone à la maison est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.Email)
                .NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");

            RuleFor(vm => vm.idEmployeurSelectionne)
                .NotEmpty().WithMessage("Le choix de l'entreprise est requis!");
        }
    }
}
