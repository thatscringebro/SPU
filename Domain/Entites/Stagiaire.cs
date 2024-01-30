using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class Stagiaire
    {
        public Guid Id { get; set; }
        //public Utilisateur utilisateur;
        public Guid idEnseignant { get; set; }
        public Guid idHoraire { get; set; }

        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire horaire { get; set;}

        [ForeignKey(nameof(idEnseignant))]
        public virtual Enseignant enseignant { get; set; }

        public List<MDS> mds = new();
    }
}
