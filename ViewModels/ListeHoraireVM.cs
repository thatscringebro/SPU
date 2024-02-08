using SPU.Domain.Entites;

namespace SPU.ViewModels
{
    public class ListeHoraireVM
    {
        public int idHoraire { get; set; }
        public string DateDebutFinStageNom { get; set; }
        public List<Horaire> listeHoraireMds { get; set; }
    }
}
