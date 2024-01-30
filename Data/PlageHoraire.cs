using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class PlageHoraire
    {
        public Guid id;

        public DateTime dateDebut;
        public DateTime dateFin; 

        public bool confirmationPresence;

        public Guid idHoraire;

        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire Horaire { get; set; }
    }
}
