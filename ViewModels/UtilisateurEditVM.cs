using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurEditVM
    {
        public string role { get; set; }
        public Guid id { get; set; }
        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = " Courriel")]
        public string Email { get; set; }
    }
}
