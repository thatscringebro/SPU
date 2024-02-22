using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Stagiaire
    {
        public Guid Id { get; set; }
        public Guid? EnseignantId { get; set; }
        //public Guid HoraireId { get; set; }
        public Guid ChatId { get; set; }
        public Guid UtilisateurId { get; set; }
        public Guid? EmployeurId { get; set; }
        public Guid EcoleId { get; set; }

        public DateTime? debutStage { get; set; }
        public DateTime? finStage { get; set; }

        public bool PartagerInfoContact { get; set; }

        //[ForeignKey(nameof(MDS))] virtual
        //public MDS MDS2 { get; set; }
        //public Guid? MDSId12 { get; set; }
        //public MDS MDS1 { get; set; }
        //public Guid? MDSId11 { get; set; }

        //[ForeignKey(nameof(UtilisateurId))] virtual
        public Utilisateur utilisateur { get; set; }

        //[ForeignKey(nameof(idChat))] virtual
        public Chat chat { get; set; }
        //[ForeignKey(nameof(idHoraire))] virtual
        //public Horaire horaire { get; set;}

        //[ForeignKey(nameof(idEnseignant))] virtual
        public Enseignant? enseignant { get; set; }
        public Employeur? employeur { get; set; }    
        public Ecole ecole { get; set; }
    }
}
