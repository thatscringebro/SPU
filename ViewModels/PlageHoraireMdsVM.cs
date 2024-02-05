namespace SPU.ViewModels
{
    public class PlageHoraireMdsVM
    {
        public Guid id { get; set; }
        public int ChoixSemaine { get; set; }
        public int ChoixJournee { get; set; } // 1 = Lundi 2 = Mardi ..
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }
        public DateTime DateDebutStage { get; set; }
        public DateTime DateFinStage { get; set; }
    }
}
