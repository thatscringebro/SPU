using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Chat
    {
        public Guid id;

        public Guid idStagiaire;
        public Guid idEnseignant;


        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire stagiaire { get; set; }
        [ForeignKey(nameof(idEnseignant))]
        public virtual Enseignant enseignant { get; set; }


        public List<MDS> mds;
        public List<Message> message;  
    }
}
