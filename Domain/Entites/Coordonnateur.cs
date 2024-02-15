using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Coordonnateur 
    {

        public Guid Id { get; set; }
        public Guid UtilisateurId { get; set; }
        public Guid EcoleId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))] virtual
        public Utilisateur utilisateur { get; set; }
        public Ecole ecole { get; set; }
        //public List<Enseignant> enseignants = new();
        //public List<Stagiaire> stagiaires = new();
        //public List<Chat> chats = new();

     
    }
}
