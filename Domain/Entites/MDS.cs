
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using SPU.Enum;

namespace SPU.Domain.Entites
{
    public class MDS
    {

        public Guid Id { get; set; }
        public string idMatricule { get; set; }
        public Status status { get; set; }
        public Civilite civilite { get; set; }
        public TypeEmployeur typeEmployeur { get; set; }
        public bool actif { get; set; }
        public string telMaison { get; set; }
        public string accreditation { get; set; }
        public string commentaire { get; set; }
        public string commentaireCIUSS { get; set; }
        public string NomEmployeur { get; set; }
        public Guid idStagiaire { get; set; }
        public Guid idEmployeur { get; set; }
        public Guid idHoraire { get; set; }
        public Guid UtilisateurId { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        public virtual Utilisateur utilisateur { get; set; }

        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire horaire { get; set; }
        [ForeignKey(nameof(idEmployeur))]
        public virtual Employeur employeur { get; set; }
        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire stagiaire { get; set; }



    }
}
