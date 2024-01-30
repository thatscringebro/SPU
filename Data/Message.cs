using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Message
    {
        public Guid id;
        public Guid idChat;
        public string message;
        public DateTime dateHeure;


        //Futur foreign key avec identity
        public int idUtilisateur;

        [ForeignKey(nameof(idChat))]
        public virtual Chat Chat { get; set; }
    }
}
