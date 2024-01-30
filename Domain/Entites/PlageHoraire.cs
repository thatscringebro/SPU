using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class PlageHoraire
    {
        public Guid Id { get; set; }

        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }

        public bool confirmationPresence { get; set; }

        public Guid idHoraire { get; set; }

        [ForeignKey(nameof(idHoraire))]
        public virtual Horaire Horaire { get; set; }
    }
}
