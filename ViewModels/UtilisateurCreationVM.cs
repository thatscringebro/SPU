using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurCreationVM
    {
        //Pour tous les utilisateurs
        //MDS et Employeur hérite de cette classe-ci
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Numéro de cellulaire")]
        public string PhoneNumber { get; set; }
    }
}
