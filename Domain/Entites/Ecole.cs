using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Ecole
    {
        public Guid id { get; set; }    
        public string Nom {  get; set; }
        //public List<Stagiaire> Stagiaires { get; set; }
        //public List<Enseignant> Enseignants { get; set;}
        //public List<Coordonnateur> Coordonateurs { get; set; }
        public string NumDeTel { get; set; }

        public Guid AdresseId { get; set; }
        //[ForeignKey(nameof(idAdresse))] virtual
        public Adresse adresse { get; set; }

    }
}
