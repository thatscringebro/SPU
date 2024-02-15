using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

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
}
