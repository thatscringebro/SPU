using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Enseignant : Utilisateur
    {
        //public Guid Id { get; set; }
        //public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))]
        //public virtual Utilisateur utilisateur { get; set; }
        public Guid idChat { get; set; }


        [ForeignKey(nameof(idChat))]
        public virtual Chat chat { get; set; }
        public List<Stagiaire> stagiaires = new();

        public Enseignant(string userName) : base(userName)
        {
        }
    }
}
