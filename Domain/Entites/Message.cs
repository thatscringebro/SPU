using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid idChat { get; set; }
        public string message { get; set; }
        public DateTime dateHeure { get; set; }


        //Futur foreign key avec identity
        //public int idUtilisateur;

        [ForeignKey(nameof(idChat))]
        public virtual Chat Chat { get; set; }
    }
}
