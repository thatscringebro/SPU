using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class EmployeurEditVM
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
