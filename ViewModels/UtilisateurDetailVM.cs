using Microsoft.AspNetCore.Mvc;
using SPU.Enum;
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

        [Display(Name = "Courriel")]
        public string Courriel { get; set; }
        [Display(Name = "Numéro de téléphone")]
        public string Telephone { get; set; }

        [Display(Name = "Nom d'utilisateur")]
        public string userName { get; set; }

        //Stagiaire
        [Display(Name = "Partager contact")]
        public bool PartagerInfoContact { get; set; }
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Début de stage ")]
        public string? debutStage { get; set; }
       // [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT}", ApplyFormatInEditMode = true), Display(Name = "Fin de stage")]
        public string? finStage { get; set; }

        //Enseignant
        public string Ecole { get; set; }

        //employeur
        #region addresse
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
        #endregion

        //mds
        #region mds
        public string Matricule { get; set; }
        public Civilite civilite { get; set; }
        [Display(Name = "Type d'employeur")]
        public TypeEmployeur TypeEmployeur { get; set; }
        [Display(Name = "Téléphone de maison")]
        public string telMaison { get; set; }
        [Display(Name = "Nom de l'employeur")]
        public string? NomEmployeur { get; set; }
        public Status status { get; set; }
        public bool actif { get; set; }
        public string? accreditation { get; set; }
        public string? commentaire { get; set; }
        public string? commentaireCIUSS { get; set; }
        #endregion



    }
}
