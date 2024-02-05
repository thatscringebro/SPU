using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Evaluation
    {
        public Guid Id { get; set; }
        public string lienGoogleForm { get; set; }

        //Est-ce qu'il y a plusieurs évaluation ? ou un seul lien forme.

        public Guid StagiaireId { get; set; }
        public Guid MDSId { get; set; }

        public bool consulter { get; set; }
        public bool actif { get; set; }
        public bool estStagiaire { get; set; }

        //[ForeignKey(nameof(idStagiaire))] virtual
        public Stagiaire stagiaire { get; set; }
        //[ForeignKey(nameof(idMDS))] virtual
        public MDS mds { get; set; }
    }
}
