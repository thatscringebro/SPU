using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Horaire
    {
        public int id;

        public int idMDS;
        public int idStagiaire;

        [ForeignKey(nameof(idStagiaire))]
        public virtual Stagiaire Stagiaire { get; set; }
        [ForeignKey(nameof(idMDS))]
        public virtual MDS Mds { get; set; }

        public List<PlageHoraire> plageHoraires = new();
    }


}
