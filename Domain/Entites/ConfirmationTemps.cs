using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class ConfirmationTemps
    {
        public Guid Id { get; set; }
        public bool confirmationPresence { get; set; }
        public string commentaires { get; set; }
        //Est pour afficher les commentaires pour les absences des stagiaires
        public bool estStagiaire { get; set; }

        public Guid idPlageHoraire { get; set; }

        [ForeignKey(nameof(idPlageHoraire))]
        public virtual PlageHoraire PlageHoraire { get; set; }

    }
}
