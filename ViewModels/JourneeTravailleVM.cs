namespace SPU.ViewModels
{
    public class JourneeTravailleVM
    {
        public Guid Id { get; set; }
        public DateTime DateDebutQuart { get; set; }
        public DateTime DateFinQuart { get; set; }
        public bool StagiairePresent { get; set; } = true;
        public Guid? MdsAbsent1 { get; set; }
        public Guid? MdsAbsent2 { get; set; }
    }
}
