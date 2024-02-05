using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Enseignant 
    {
        public Guid Id { get; set; }
        public Guid UtilisateurId { get; set; }
        public Guid EcoleId { get; set; }


        //virtual
        //[ForeignKey(nameof(UtilisateurId))]
        public  Utilisateur utilisateur { get; set; }
        public Ecole ecole { get; set; }
        //public List<Chat> chats = new List<Chat>();
        //public List<Stagiaire> stagiaires = new();

       
    }
}
