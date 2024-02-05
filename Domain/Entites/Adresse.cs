using System.Runtime.InteropServices;

namespace SPU.Domain.Entites
{
    public class Adresse
    {
        public Guid Id { get; set; }
        public string NoCivique { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string Pays { get; set; }
        public string Province { get; set; }
        public string CodePostal { get; set; }



    }
}
