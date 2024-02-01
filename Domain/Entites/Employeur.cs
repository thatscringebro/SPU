using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Employeur : Utilisateur
    {
        public Employeur(string userName) : base(userName)
        {
        }

        //public Guid Id { get; set; }
        public Guid idAdresse { get; set; }
        //public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))]
        //public virtual Utilisateur utilisateur { get; set; }

        [ForeignKey(nameof(idAdresse))]
        public virtual Adresse adresse { get; set; }



    }
}
