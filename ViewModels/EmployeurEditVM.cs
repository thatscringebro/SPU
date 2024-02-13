using System.ComponentModel.DataAnnotations;

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
}
