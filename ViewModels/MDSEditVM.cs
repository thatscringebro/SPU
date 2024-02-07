﻿using SPU.Enum;
using System.ComponentModel.DataAnnotations;

namespace SPU.ViewModels
{
    public class MDSEditVM
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
        [Display(Name = "Mot de passe")]
        public string pwd { get; set; }
        [Display(Name = "Confirmation mot de passe")]
        public string confirmationpwd { get; set; }
        [Display(Name = "Numéro de matricule")]
        public string MatriculeId { get; set; }
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