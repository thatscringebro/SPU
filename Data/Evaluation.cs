using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Evaluation
    {
        public Guid id;
        public string lienGoogleForm;

        //Est-ce qu'il y a plusieurs évaluation ? ou un seul lien forme.

        public Guid idStagiaire;
        public Guid idMDS;

        public bool consulter;
        public bool actif;
        public bool estStagiaire; 

        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire stagiaire { get; set; }
        [ForeignKey(nameof(idMDS))]
        public virtual MDS MDS { get; set; }
    }
}
