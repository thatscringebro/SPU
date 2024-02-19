using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class PlageHoraireMdsVM
    {
        public Guid id { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateDebutPlageHoraire { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFinPlageHoraire { get; set; }

        public int HeureDebutPlageHoraire { get; set; }
        public int MinutesDebutPlageHoraire { get; set; }
        public int HeureFinPlageHoraire { get; set; }
        public int MinutesFinPlageHoraire { get; set; }

        public bool Recurrence { get; set; }

        public class Validator : AbstractValidator<PlageHoraireMdsVM>
        {
            public Validator()
            {

                RuleFor(vm => vm.DateDebutPlageHoraire)
                    .NotEmpty().WithMessage("Veuillez entrer une date de début de plage horaire")
                    .GreaterThan(DateTime.MinValue).WithMessage("La date de début de plage horaire ne peut pas être vide");

                RuleFor(vm => vm.DateFinPlageHoraire)
                    .NotEmpty().WithMessage("Veuillez entrer une date de fin de plage horaire")
                    .GreaterThan(DateTime.MinValue).WithMessage("La date de fin de plage horaire ne peut pas être vide");

                RuleFor(vm => vm.DateDebutPlageHoraire.AddHours(vm.HeureDebutPlageHoraire).AddMinutes(vm.MinutesDebutPlageHoraire))
                    .LessThan(x => x.DateFinPlageHoraire.AddHours(x.HeureFinPlageHoraire).AddMinutes(x.MinutesFinPlageHoraire)).WithMessage("La date de début de la plage horaire doit être inférieur à la date de fin");

                RuleFor(vm => vm.DateFinPlageHoraire.AddHours(vm.HeureFinPlageHoraire).AddMinutes(vm.MinutesFinPlageHoraire))
                    .GreaterThan(x => x.DateDebutPlageHoraire.AddHours(x.HeureDebutPlageHoraire).AddMinutes(x.MinutesDebutPlageHoraire)).WithMessage("La date de fin de la plage horaire doit être supérieur à la date de début");

                //Vérifier plus petit que 24 heures et plus grand ou égal à 15 minutes
                RuleFor(vm => vm.DateFinPlageHoraire.AddHours(vm.HeureFinPlageHoraire).AddMinutes(vm.MinutesFinPlageHoraire))
                    .LessThan(x => x.DateDebutPlageHoraire.AddHours(x.HeureDebutPlageHoraire).AddMinutes(x.MinutesDebutPlageHoraire).AddHours(24)).WithMessage("La plage horaire doit avoir une durée de moins de 24 heures")
                    .GreaterThanOrEqualTo(x => x.DateDebutPlageHoraire.AddHours(x.HeureDebutPlageHoraire).AddMinutes(x.MinutesDebutPlageHoraire).AddMinutes(15)).WithMessage("La plage horaire doit être au minimum de 15 minutes");
            }

        }

    }
}
