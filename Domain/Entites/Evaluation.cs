using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class Evaluation
    {
        public Guid Id { get; set; }
        public string lienGoogleForm { get; set; }

        //Est-ce qu'il y a plusieurs évaluation ? ou un seul lien forme.

        public Guid idStagiaire { get; set; }
        public Guid idMDS { get; set; }

        public bool consulter { get; set; }
        public bool actif { get; set; }
        public bool estStagiaire { get; set; }

        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire stagiaire { get; set; }
        [ForeignKey(nameof(idMDS))]
        public virtual MDS MDS { get; set; }
    }
}
