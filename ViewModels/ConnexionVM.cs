using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class ConnexionVM
    {
        [Display(Name = "Nom d'utilisateur")]
        public string? UserName { get; set; }

        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string? Pwd { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; } = false;

    }

    public class ConnexionVMValidation : AbstractValidator<ConnexionVM> 
    { 
        public ConnexionVMValidation() 
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Le nom d'utilisateur est requis!");

            RuleFor(vm => vm.Pwd).NotEmpty().WithMessage("Le mot de passe est requis!");
        }
    }
}
