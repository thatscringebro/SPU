using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Enseignant
    {
        public Guid Id { get; set; }
        public Guid UtilisateurId { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        public virtual Utilisateur utilisateur { get; set; }
        public List<Stagiaire> stagiaires = new();




    }
}
