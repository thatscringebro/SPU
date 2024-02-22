using SPU.Domain.Entites;

namespace SPU.ViewModels
{
    public class HorairePageVM
    {
        public string Role { get; set; }
        public string nomMds { get; set; }
        public string? nomMds2 { get; set; }
        public string? nomStagiaire { get; set; }
        public DateTime? DateDebutStage { get; set; }
        public DateTime? DateFinStage { get; set; }
        public DateTime DateCreationHoraire { get; set; }
        public DateTime DateExpiration { get; set; }
    }
}
