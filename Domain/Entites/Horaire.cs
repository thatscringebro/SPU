using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Horaire
    {
        public Guid Id { get; set; }
        public Guid? StagiaireId { get; set; }
       
        public Guid MDSId { get; set; }
    

        public Stagiaire? stagiaire { get; set; }
        public MDS mds { get; set; }
    }


}
