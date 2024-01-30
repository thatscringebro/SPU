
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using SPU.Enum;

namespace SPU.Data
{
    public class MDS
    {

        public Guid id;
        public string idMatricule;
        public Status status; 
        public Civilite civilite;
        public TypeEmployeur typeEmployeur;
        public bool actif;
        public string telMaison;
        public string accreditation;
        public string commentaire;
        public string commentaireCIUSS;
        public string NomEmployeur;
        public Guid idStagiaire;
        public Guid idEmployeur;
        public Guid idHoraire;
        //public Utilisateur utilisateur;

        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire horaire { get; set; }
        [ForeignKey(nameof(idEmployeur))]
        public virtual Employeur employeur { get; set; }
        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire stagiaire { get; set; }



    }
}
