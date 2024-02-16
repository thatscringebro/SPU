using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurDetailVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Rôle")]
        public string role { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Nom")]
        public string Nom {  get; set; }

        public string Matricule { get; set; }

        [Display(Name = "Courriel")]
        public string Courriel { get; set; }
        [Display(Name = "Numéro de téléphone")]
        public string Telephone { get; set; }
      

    }
}
