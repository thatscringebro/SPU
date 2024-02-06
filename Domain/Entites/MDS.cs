
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using SPU.Enum;

namespace SPU.Domain.Entites
{
    public class MDS { 
    
        public Guid Id { get; set; }
        public string MatriculeId { get; set; }
        public Status status { get; set; }
        public Civilite civilite { get; set; }
        public TypeEmployeur typeEmployeur { get; set; }
        public bool actif { get; set; }
        public string? telMaison { get; set; }
        public string? accreditation { get; set; }
        public string? commentaire { get; set; }
        public string? commentaireCIUSS { get; set; }
        public string NomEmployeur { get; set; }


        public Guid? StagiaireId { get; set; }
        public Guid EmployeurId { get; set; }
        //public Guid HoraireId { get; set; }
        public Guid? ChatId { get; set; }
        public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))] virtual
        public Utilisateur utilisateur { get; set; }

        //[ForeignKey(nameof(idChat))]virtual
        public Chat? chat { get; set; }
        //[ForeignKey(nameof(idHoraire))] virtual
        //public Horaire horaire { get; set; }
        //[ForeignKey(nameof(idEmployeur))] virtual
        public Employeur employeur { get; set; }
        //[ForeignKey(nameof(idStagiaire))] virtual
        public Stagiaire? stagiaire { get; set; }



    }
}
