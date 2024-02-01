using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Enseignant : Utilisateur
    {
        //public Guid Id { get; set; }
        //public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))]
        //public virtual Utilisateur utilisateur { get; set; }

        public List<Chat> chats = new List<Chat>();
        public List<Stagiaire> stagiaires = new();

        public Enseignant(string userName) : base(userName)
        {
        }
    }
}
