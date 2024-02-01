using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Stagiaire : Utilisateur
    {
        //public Guid Id { get; set; }
        public Guid idEnseignant { get; set; }
        public Guid idHoraire { get; set; }
        public Guid idChat { get; set; }
        //public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))]
        //public virtual Utilisateur utilisateur { get; set; }

        [ForeignKey(nameof(idChat))]
        public virtual Chat chat { get; set; }
        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire horaire { get; set;}

        [ForeignKey(nameof(idEnseignant))]
        public virtual Enseignant enseignant { get; set; }

        public List<MDS> mds = new();

        public Stagiaire(string userName) : base(userName)
        {
        }
    }
}
