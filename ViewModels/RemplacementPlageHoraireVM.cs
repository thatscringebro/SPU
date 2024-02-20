namespace SPU.ViewModels
{
    public class RemplacementPlageHoraireVM
    {
        public Guid Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? MatriculeRemplacent1 { get; set; }
        public string? MatriculeRemplacent2 { get; set; }

    }
}
