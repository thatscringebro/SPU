using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Horaire
    {
        public Guid Id { get; set; }
        public Guid? StagiaireId { get; set; }
       
        public Guid MDSId1 { get; set; }
        public Guid? MDSId2 { get; set; }

        public Stagiaire? stagiaire { get; set; }
        public MDS mds1 { get; set; }
        public MDS? mds2 { get; set;}
    }


}
