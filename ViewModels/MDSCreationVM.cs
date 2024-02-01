using SPU.Enum;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class MDSCreationVM : UtilisateurCreationVM
    {
        [Display(Name = "Numéro de matricule")]
        public string idMatricule { get; set; }
        [Display(Name = "Civilité")]
        public Civilite civilite { get; set; }
        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }
        [Display(Name = "Téléphne de maison")]
        public string telMaison { get; set; }
        [Display(Name = "Nom de l'employeur")]
        public string NomEmployeur { get; set; }
    }
}
