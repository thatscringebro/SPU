using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurDetailVM
    {
        [Display(Name = "Rôle")]
        public string role { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom {  get; set; }
      

    }
}
