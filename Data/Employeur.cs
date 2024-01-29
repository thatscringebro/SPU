using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Data
{
    public class Employeur
    {
        public int id;
        public int idAdresse;
        //public Utilisateur utilisateur

        [ForeignKey(nameof(idAdresse))]
        public virtual Adresse adresse { get; set; }
      


    }
}
