using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Employeur
    {


        public Guid Id { get; set; }
        public Guid AdresseId { get; set; }
        public Guid UtilisateurId { get; set; }

        //[ForeignKey(nameof(UtilisateurId))] virtual
        public Utilisateur utilisateur { get; set; }

        //[ForeignKey(nameof(idAdresse))] virtual
        public Adresse adresse { get; set; }

        //public List<MDS> mDs { get; set; }

    }
}
