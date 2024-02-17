using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Chat
    {
        public Guid Id { get; set; }

        //public Guid idStagiaire { get; set; }
        public Guid? EnseignantId { get; set; }
        //public Guid? CoordonateurId { get; set; }

        //[ForeignKey(nameof(idStagiaire))]
        //public virtual Stagiaire stagiaire { get; set; }
        //[ForeignKey(nameof(idEnseignant))]virtual
        public Enseignant enseignant { get; set; }
        //public  Coordonnateur coordonateur { get; set; }

        //public List<MDS> mds = new();
        //public List<Message> message = new();
    }
}
