using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string message { get; set; }
        public DateTime DateHeure { get; set; }
        public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(idUtilisateur))] virtual
        public Utilisateur utilisateur { get; set; }
        //[ForeignKey(nameof(idChat))] virtual
        public Chat chat { get; set; }
    }
}
