using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class Stagiaire
    {
        public Guid Id { get; set; }
        public Guid? EnseignantId { get; set; }
        //public Guid HoraireId { get; set; }
        public Guid? ChatId { get; set; }
        public Guid UtilisateurId { get; set; }
        public Guid? EmployeurId { get; set; }
        public Guid EcoleId { get; set; }


        //[ForeignKey(nameof(UtilisateurId))] virtual
        public Utilisateur utilisateur { get; set; }

        //[ForeignKey(nameof(idChat))] virtual
        public Chat? chat { get; set; }
        //[ForeignKey(nameof(idHoraire))] virtual
        //public Horaire horaire { get; set;}

        //[ForeignKey(nameof(idEnseignant))] virtual
        public Enseignant? enseignant { get; set; }
        public Employeur? employeur { get; set; }    
        public Ecole ecole { get; set; }

        //public List<MDS> mds = new();

    
    }
}
