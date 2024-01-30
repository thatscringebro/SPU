using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domaine.Entites
{
    public class Employeur
    {
        public Guid Id { get; set; }
        public Guid idAdresse { get; set; }
        //public Utilisateur utilisateur

        [ForeignKey(nameof(idAdresse))]
        public virtual Adresse adresse { get; set; }



    }
}
