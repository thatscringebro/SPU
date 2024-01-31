using System.Runtime.InteropServices;

namespace SPU.Domain.Entites
{
    public class Adresse
    {
        public Guid Id { get; set; }
        public string NumeroDeRue { get; set; }
        public string NomDeRue { get; set; }
        public string ville { get; set; }
        public string pays { get; set; }
        public string codePostal { get; set; }



    }
}
