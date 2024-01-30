using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class Horaire
    {
        public Guid Id { get; set; }

   
        public List<PlageHoraire> plageHoraires = new();
    }


}
