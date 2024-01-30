using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Stagiaire
    {
        public Guid id;
        //public Utilisateur utilisateur;
        public Guid idEnseignant;
        

        [ForeignKey(nameof(idEnseignant))]
        public virtual Enseignant enseignant { get; set; }

        public List<MDS> mds;
    }
}
