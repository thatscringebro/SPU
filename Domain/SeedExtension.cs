using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPU.Domain.Entites;
using SPU.Enum;
using System.Net;

namespace SPU.Domain
{
    public static class SeedExtension
    {
		public static readonly PasswordHasher<Utilisateur> PASSWORD_HASHER = new();

		public static void Seed(this ModelBuilder builder)
		{
            var adresse = AddAdress(builder, "j2om0t", "125", "LaBelle", "St-Boire", "Canada", "Ici");
            var adresse1 = AddAdress(builder, "h0h2h2", "1258", "Boeuf", "St-LaVoire", "Canada", "Ici");

			var ecole = AddEcole(builder, "LaFonte", "1255647894", adresse1);

            var user0 = AddUser(builder, "user0", "Qwerty123!", "Liam", "O'Brien", "user0@gmail.com", "1234567899");
			var user1 = AddUser(builder, "enseignant0", "Password123!", "Emma", "Dupont", "enseignant0@gmail.com", "1234567891");
			var user2 = AddUser(builder, "employeur0", "Passw0rd!", "Lucas", "Martin", "employeur0@gmail.com", "1234567892");
			var user3 = AddUser(builder, "coordo0", "Coord1234!", "Chloe", "Leroy", "coordo0@gmail.com", "1234567893");
			var user4 = AddUser(builder, "stagiaire0", "Stagiaire123!", "Julien", "Moreau", "stagiaire0@gmail.com", "1234567894");
			var user5 = AddUser(builder, "mds0", "Mds2024!", "Sarah", "Bernard", "mds0@gmail.com", "1234567895");
			
			var enseignant0 = AddEnseignant(builder, user1, ecole);

			var employeur0 = AddEmployeur(builder, user2, adresse);

			var coordo0 = AddCoordo(builder, user3, ecole);

            var chat = AddChat(builder, user0, enseignant0, coordo0);

   //         var stagiaire0 = AddStagiaire(builder, user4, enseignant0, horaire, chat, employeur0, ecole);

			//var mds0 = AddMds(builder, user5, "SPU123456", Status.Accepté, Civilite.M, TypeEmployeur.CIUSSS, true, 
			//	"0987654321", "AccreditationX", "Commentaire exemple", "Commentaire CIUSS",
			//	"EmployeurX", stagiaire0, employeur0, horaire, chat);***

   //         var horaire = AddHoraire(builder, stagiaire0, mds0, ecole);

        }

        private static Ecole AddEcole(ModelBuilder builder, string nom, string numTel, Adresse adresse)
        {
            var newEcole = new Ecole()
            {
                id = Guid.NewGuid(),
                Nom = nom,
                NumDeTel = numTel,
            };
            newEcole.AdresseId = adresse.Id;
            builder.Entity<Ecole>().HasData(newEcole);

            return newEcole;
        }

        private static Horaire AddHoraire(ModelBuilder builder, Stagiaire stagiaire, MDS mds, Ecole ecole)
        {
            var newHoraire = new Horaire()
            {
                Id = Guid.NewGuid(),
				StagiaireId = stagiaire.Id,
				MDSId = mds.Id,
            };
            builder.Entity<Horaire>().HasData(newHoraire);

            return newHoraire;
        }

        private static Utilisateur AddUser(ModelBuilder builder, string userName,
			string password, string prenom, string nom, string email,
			string phoneNumber)
		{
			var newUser = new Utilisateur(userName)
			{
				Id = Guid.NewGuid(),
				UserName = userName,
				NormalizedUserName = userName.ToUpper(),
				Prenom = prenom,
				Nom = nom,
				PhoneNumber = phoneNumber,
				Email = email,
				NormalizedEmail = email,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
			builder.Entity<Utilisateur>().HasData(newUser);

			return newUser;
		}

		private static Enseignant AddEnseignant(ModelBuilder builder, Utilisateur user, Ecole ecole)
		{
			var newUser = new Enseignant()
			{
				Id = Guid.NewGuid(), 
				UtilisateurId = user.Id,
			};
			newUser.EcoleId = ecole.id;
			builder.Entity<Enseignant>().HasData(newUser);

			return newUser;
		}

		private static Employeur AddEmployeur(ModelBuilder builder, Utilisateur user, Adresse adresse)
		{
			var newUser = new Employeur()
            {
                Id = Guid.NewGuid(),
                UtilisateurId = user.Id,
			};
			newUser.AdresseId = adresse.Id;
			builder.Entity<Employeur>().HasData(newUser);
			
			return newUser;
        }

        private static Adresse AddAdress(ModelBuilder builder, string codeP, 
			string numRue, string nomRue, string ville, string pays, string province)
        {
            var newAddress = new Adresse()
            {
                Id = Guid.NewGuid(),
                NoCivique = numRue,
                Rue = nomRue,
                CodePostal = codeP,
                Pays = pays,
                Province = province,
                Ville = ville
            };
            builder.Entity<Adresse>().HasData(newAddress);

            return newAddress;
        }

        private static Coordonateur AddCoordo(ModelBuilder builder, Utilisateur user, Ecole ecole)
		{
			var newUser = new Coordonateur()
            {
                Id = Guid.NewGuid(),
				UtilisateurId=user.Id,
				EcoleId = ecole.id,
            };
			builder.Entity<Coordonateur>().HasData(newUser);

			return newUser;
		}

		private static Stagiaire AddStagiaire(ModelBuilder builder, Utilisateur user,
			Enseignant enseignant, Guid idHoraire, Chat chat, Employeur employeur, Ecole ecole)
		{
			var newUser = new Stagiaire()
			{
				Id = Guid.NewGuid(),
				EnseignantId = enseignant.Id,
				ChatId = chat.Id,
				UtilisateurId = user.Id,
				EmployeurId = employeur.Id,
				EcoleId = ecole.id,
			};
			//newUser.EcoleId = ecole.id;
			//newUser.ChatId = chat.Id;
			builder.Entity<Stagiaire>().HasData(newUser);

			return newUser;
		}

		private static MDS AddMds(ModelBuilder builder, Utilisateur user, 
			string idMatricule, Status status, Civilite civilite, TypeEmployeur 
			typeEmployeur, bool actif, string telMaison, string accredication, 
			string commentaire, string commentaireCIUSS, string nomEmployeur, 
			Stagiaire stagiaire, Employeur employeur, Guid idHoraire, Guid idChat)
		{
			var newUser = new MDS()
			{
				Id = Guid.NewGuid(),
				UtilisateurId=user.Id,
				MatriculeId = idMatricule,
				status = status,
				civilite = civilite,
				typeEmployeur = typeEmployeur,
				actif = actif,
				telMaison = telMaison,	
				accreditation = accredication,
				commentaire = commentaire,
				commentaireCIUSS = commentaireCIUSS,
				NomEmployeur = nomEmployeur,
				ChatId = idChat,
			};
			newUser.StagiaireId = stagiaire.Id;
			newUser.EmployeurId = employeur.Id;

			builder.Entity<MDS>().HasData(newUser);

			return newUser;
        }

        private static Chat AddChat(ModelBuilder builder, Utilisateur user, Enseignant enseignant, Coordonateur coordonateur)
        {
            var newChat = new Chat()
            {
                Id = Guid.NewGuid(),
				EnseignantId = enseignant.Id,
				CoordonateurId = coordonateur.Id
            };
            builder.Entity<Chat>().HasData(newChat);

            return newChat;
        }
    }
}


