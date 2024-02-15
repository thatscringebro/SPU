using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class AjoutNouvelHoraireMdsVM
    {
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeDebutStage { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeFinStage { get; set; }

        public List<MDS> listeMaitreDeStage { get; set; } = new List<MDS>();

        public MDS choixMds1 { get; set; }
        public MDS? choixMds2 { get; set; }
    }

    public class Validator : AbstractValidator<AjoutNouvelHoraireMdsVM>
    {
        DateTime dateAnneePrecedente = DateTime.Now.AddYears(-1);
        DateTime dateAnneeSuivante = DateTime.Now.AddYears(1);

        public Validator()
        {
            RuleFor(vm => vm.DateTimeDebutStage)
            .NotEmpty().WithMessage("Veuillez entrer une date de début de stage")
            .GreaterThan(DateTime.MinValue).WithMessage("La date de début de stage ne peut pas être vide")
            .GreaterThanOrEqualTo(dateAnneePrecedente).WithMessage("La date de début de stage doit être supérieure ou égale à un an avant aujourd'hui")
            .LessThanOrEqualTo(dateAnneeSuivante).WithMessage("La date de début de stage doit être inférieure ou égale à un an après aujourd'hui");

            RuleFor(vm => vm.DateTimeFinStage)
            .NotEmpty().WithMessage("Veuillez entrer une date de fin de stage")
            .GreaterThan(DateTime.MinValue).WithMessage("La date de fin de stage ne peut pas être vide")
            .GreaterThan(vm => vm.DateTimeDebutStage).WithMessage("La date de fin de stage doit être supérieur à la date de début de stage")
            .GreaterThanOrEqualTo(dateAnneePrecedente).WithMessage("La date de fin de stage doit être supérieure ou égale à un an avant aujourd'hui")
            .LessThanOrEqualTo(dateAnneeSuivante).WithMessage("La date de fin de stage doit être inférieure ou égale à un an après aujourd'hui");
        }

    }
}
