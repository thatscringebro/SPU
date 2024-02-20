using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class StagiairesEditVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom {  get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true), Display(Name = "Début de stage ")]
        public DateTime? debutStage { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true), Display(Name = "Fin de stage")]
        public DateTime? finStage { get; set; }

        [Display(Name = "Choisir maître de stage 1")]
        public Guid? idMdsSelectionne1 { get; set; }

        [Display(Name = "Choisir maître de stage 2")]
        public Guid? idMdsSelectionne2 { get; set; }

        public List<SelectListItem> Mds { get; set; } = new List<SelectListItem>();

        [Display(Name = "Choisir l'enseignant")]
        public Guid? idEnseignantSelectionne { get; set; }

        public List<SelectListItem> Enseignants { get; set; } = new List<SelectListItem>();
        //public Horaire? horaire { get; set; }
    }

    public class StagiairesEditVMValidation : AbstractValidator<StagiairesEditVM>
    {
        public StagiairesEditVMValidation()
        {
            RuleFor(vm => vm.Prenom).NotEmpty().WithMessage("Le prénom est requis!");
            RuleFor(vm => vm.Nom).NotEmpty().WithMessage("Le nom est requis!");

            //RuleFor(vm => vm.debutStage)
            //    .NotEmpty().WithMessage("La date de début de stage est requise!")
            //    .LessThan(vm => vm.finStage.Value).When(vm => vm.finStage.HasValue).WithMessage("La date de début doit être antérieure à la date de fin!");

            //RuleFor(vm => vm.finStage)
            //    .NotEmpty().WithMessage("La date de fin de stage est requise!")
            //    .GreaterThan(vm => vm.debutStage.Value).When(vm => vm.debutStage.HasValue).WithMessage("La date de fin doit être postérieure à la date de début!");

            //RuleFor(vm => vm.idMdsSelectionne1)
            //    .NotEmpty().WithMessage("Le choix du maître de stage 1 est requis!");

            //RuleFor(vm => vm.idMdsSelectionne2)
            //    .NotEmpty().WithMessage("Le choix du maître de stage 2 est requis!");

            RuleFor(vm => vm.idEnseignantSelectionne)
                .NotEmpty().WithMessage("Le choix de l'enseignant est requis!");
        }
    }
}
