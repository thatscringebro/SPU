using SPU.Domain.Entites;

namespace SPU.ViewModels
{
    public class HoraireIndexVM
    {
        public List<Horaire?> listeMds { get; set; }
        public List<Stagiaire?> listeStagiaire { get; set; }
        public List<AssociationStagiaireEnseignantVM> associationStagEns { get; set; }
        public string role { get; set; }
    }
}
