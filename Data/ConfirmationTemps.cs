using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class ConfirmationTemps
    {
        public Guid id;
        public bool confirmationPresence;
        public string commentaires;
        //Est pour afficher les commentaires pour les absences des stagiaires
        public bool estStagiaire;

        public Guid idPlageHoraire;

        [ForeignKey(nameof(idPlageHoraire))]
        public virtual PlageHoraire PlageHoraire { get; set; }

    }
}
