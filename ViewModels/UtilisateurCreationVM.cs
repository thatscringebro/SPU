using Microsoft.AspNetCore.Mvc.Rendering;
using SPU.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class UtilisateurCreationVM
    {
        public string role { get; set; }

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
        [Display(Name = "Veuillez choisir un établissement")]
        public Ecole Ecole { get; set; }

        public int ecoleSelectionne { get; set; }
        public List<SelectListItem> Ecoles { get; set; } = new List<SelectListItem>();

    }
}
