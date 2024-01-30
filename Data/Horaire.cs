using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Horaire
    {
        public Guid id;

        public Guid idMDS;
        public Guid idStagiaire;

        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire Stagiaire { get; set; }
        [ForeignKey(nameof(idMDS))]
        public virtual MDS Mds { get; set; }

        public List<PlageHoraire> plageHoraires = new();
    }


}
