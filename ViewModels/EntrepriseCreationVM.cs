using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class EntrepriseCreationVM : UtilisateurCreationVM
    {
        [Display(Name = "Numéro de rue")]
        public string NumeroDeRue { get; set; }
        [Display(Name = "Nom de rue")]
        public string NomDeRue { get; set; }
        [Display(Name = "Ville")]
        public string ville { get; set; }
        [Display(Name = "Pays")]
        public string pays { get; set; }
        [Display(Name = "Code postal")]
        public string codePostal { get; set; }
    }
}
