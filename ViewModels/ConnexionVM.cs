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
}
