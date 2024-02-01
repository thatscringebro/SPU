namespace SPU.ViewModels
{
    public class JourneeTravailleVM
    {
        public Guid Id { get; set; }
        public DateTime DateDebutQuart { get; set; }
        public DateTime DateFinQuart { get; set; }
        public bool Present { get; set; } = true;
    }
}
