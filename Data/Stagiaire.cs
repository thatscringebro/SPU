using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Stagiaire
    {
        public int id;

        //public Utilisateur utilisateur;
        public int idEnseignant;
        public int idMDS;

        [ForeignKey(nameof(idEnseignant))]
        public virtual Enseignant enseignant { get; set; }
        [ForeignKey(nameof(idMDS))]   
        public virtual MDS Mds { get; set; }
    }
}
