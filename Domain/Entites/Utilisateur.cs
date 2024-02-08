using Microsoft.AspNetCore.Identity;

namespace SPU.Domain.Entites
{
    public class Utilisateur : IdentityUser<Guid>
    {
        public Utilisateur(string userName) :base(userName) { }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string NomComplet => $"{Prenom} {Nom}";
        public override string Email { get => base.Email; set => base.Email = value; }
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

    }
}
