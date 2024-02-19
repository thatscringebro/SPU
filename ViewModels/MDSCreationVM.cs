﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPU.ViewModels
{
    public class MDSCreationVM 
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
        [Display(Name = "Numéro de matricule")]
        public string MatriculeId { get; set; }
        [Display(Name = "Civilité")]
        public Civilite civilite { get; set; }
        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }
        [Display(Name = "Téléphone de maison")]
        public string telMaison { get; set; }
        [Display(Name = "Nom de l'employeur")]
        public string? NomEmployeur { get; set; }
        public string role { get; set; }
        [Display(Name = " Courriel")]
        public string Email { get; set; }

        [Display(Name = "Veuillez choisir une entreprise")]
        public Guid idEmployeurSelectionne { get; set; }

        public List<SelectListItem> Employeurs { get; set; } = new List<SelectListItem>();

    }

    public class MDSCreationVMValidation : AbstractValidator<MDSCreationVM>
    {
        public MDSCreationVMValidation()
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

            RuleFor(vm => vm.MatriculeId)
            .NotEmpty().WithMessage("Le numéro de matricule est requis!")
            .Matches(new Regex(@"^[A-Z]{3}\d{4}$")).WithMessage("Le format du matricule est invalide! Utilisez le format XXX1234.");

            RuleFor(vm => vm.civilite).IsInEnum().WithMessage("La civilité est invalide!");

            RuleFor(vm => vm.TypeEmployeur).IsInEnum().WithMessage("Le type d'employeur est invalide!");

            RuleFor(vm => vm.telMaison)
    .NotEmpty().WithMessage("Le numéro de téléphone à la maison est requis!")
    .Matches(new Regex(@"^\s*\d{3}-\d{3}-\d{4}\s*$")).WithMessage("Le format du téléphone est invalide! Utilisez le format XXX-XXX-XXXX.");

            RuleFor(vm => vm.Email)
                .NotEmpty().WithMessage("L'email est requis!")
                .EmailAddress().WithMessage("L'email n'est pas valide!");

            RuleFor(vm => vm.idEmployeurSelectionne).NotEmpty().WithMessage("Le choix de l'entreprise est requis!");
        }
    }
}
