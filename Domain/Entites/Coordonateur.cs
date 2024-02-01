using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Coordonateur : Utilisateur
    {

        //public Guid Id { get; set; }
        //public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))]
        //public virtual Utilisateur utilisateur { get; set; }

        public List<Enseignant> enseignants = new();
        public List<Stagiaire> stagiaires = new();
        public List<Chat> chats = new();

        public Coordonateur(string userName) : base(userName)
        {
        }
    }
}
