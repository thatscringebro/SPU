using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class ModifierPlageHoraireVM
    {
        public Guid id { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateDebutPlageHoraire { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateFinPlageHoraire { get; set; }

        public int HeureDebutPlageHoraire { get; set; }
        public int MinutesDebutPlageHoraire { get; set; }
        public int HeureFinPlageHoraire { get; set; }
        public int MinutesFinPlageHoraire { get; set; }

        public bool EstPresent { get; set; }
        public string? Commentaire { get; set; }

        public class Validator : AbstractValidator<ModifierPlageHoraireVM>
        {
            public Validator()
            {

                RuleFor(vm => vm.DateDebutPlageHoraire)
                    .NotEmpty().WithMessage("Veuillez entrer une date de début de plage horaire")
                    .GreaterThan(DateTime.MinValue).WithMessage("La date de début de plage horaire ne peut pas être vide");

                RuleFor(vm => vm.DateFinPlageHoraire)
                    .NotEmpty().WithMessage("Veuillez entrer une date de fin de plage horaire")
                    .GreaterThan(DateTime.MinValue).WithMessage("La date de fin de plage horaire ne peut pas être vide")
                    .LessThan(x => x.DateDebutPlageHoraire.AddHours(x.HeureDebutPlageHoraire).AddMinutes(x.MinutesDebutPlageHoraire).AddHours(24)).WithMessage("La plage horaire doit avoir une durée de moins de 24 heures");

            }

        }
    }
}
