namespace SPU.Domaine.Entites
{
    public class Coordonateur
    {

        public Guid Id { get; set; }

        public List<Enseignant> enseignants = new();
        public List<Stagiaire> stagiaires = new();
        public List<Chat> chats = new();


    }
}
