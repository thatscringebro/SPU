//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using SPU.Domain.Entites;
//using SPU.Enum;

//namespace SPU.Domain
//{
//    public static class SeedExtension
//    {
//		public static readonly PasswordHasher<Utilisateur> PASSWORD_HASHER = new();

//		public static void Seed(this ModelBuilder builder)
//		{
//			var user0 = AddUser(builder, "user0", "Qwerty123!", "Liam", "O'Brien", "user0@gmail.com", "1234567899");
//			var user1 = AddUser(builder, "enseignant0", "Password123!", "Emma", "Dupont", "enseignant0@gmail.com", "1234567891");

//			Guid ecoleId = Guid.NewGuid();
//			var enseignant0 = AddEnseignant(builder, user1/*, ecoleId*/);

//			var user2 = AddUser(builder, "employeur0", "Passw0rd!", "Lucas", "Martin", "employeur0@gmail.com", "1234567892");
//			var employeur0 = AddEmployeur(builder, user2, "j2om0t", "125", "LaBelle", "St-Boire", "Ici");

//			var user3 = AddUser(builder, "coordo0", "Coord1234!", "Chloe", "Leroy", "coordo0@gmail.com", "1234567893");
//			var coordo0 = AddCoordo(builder, user3);

//			// Remarque : Les Guid pour idEnseignant, idHoraire, et idChat doivent être définis.
//			Guid idEnseignant = Guid.NewGuid(); // Exemple d'ID
//			Guid idHoraire = Guid.NewGuid();     // Exemple d'ID
//			Guid idChat = Guid.NewGuid();        // Exemple d'ID

//			var user4 = AddUser(builder, "stagiaire0", "Stagiaire123!", "Julien", "Moreau", "stagiaire0@gmail.com", "1234567894");
//			var stagiaire0 = AddIntern(builder, user4, idEnseignant, idHoraire, idChat);

//			// Remarque : Les Guid pour idStagiaire, idEmployeur, idHoraire, et idChat, ainsi que les autres champs spécifiques doivent être définis.
//			Guid idStagiaire = Guid.NewGuid(); // Exemple d'ID
//			Guid idEmployeur = Guid.NewGuid(); // Exemple d'ID
//			Guid id_Horaire = Guid.NewGuid();   // Exemple d'ID
//			Guid id_Chat = Guid.NewGuid();      // Exemple d'ID
//			Status status = Status.Accepté;     // Exemple de statut
//			Civilite civilite = Civilite.M;    // Exemple de civilité
//			TypeEmployeur typeEmployeur = TypeEmployeur.CIUSSS; // Exemple de type d'employeur

//			var user5 = AddUser(builder, "mds0", "Mds2024!", "Sarah", "Bernard", "mds0@gmail.com", "1234567895");
//			var mds0 = AddMds(builder, user5, "SPU123456",status, civilite, typeEmployeur, true, 
//				"0987654321", "AccreditationX", "Commentaire exemple", "Commentaire CIUSS",
//				"EmployeurX", idStagiaire, idEmployeur, id_Horaire, id_Chat);

//		}

//		private static Utilisateur AddUser(ModelBuilder builder, string userName,
//			string password, string prenom, string nom, string email,
//			string phoneNumber)
//		{
//			var newUser = new Utilisateur(userName)
//			{
//				Id = Guid.NewGuid(),
//				UserName = userName,
//				NormalizedUserName = userName.ToUpper(),
//				Prenom = prenom,
//				Nom = nom,
//				PhoneNumber = phoneNumber,
//				Email = email,
//				NormalizedEmail = email,
//				SecurityStamp = Guid.NewGuid().ToString()
//			};
//			newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<Utilisateur>().HasData(newUser);

//			return newUser;
//		}

//		private static Enseignant AddEnseignant(ModelBuilder builder, Utilisateur user/*, Guid ecoleId*/)
//		{
//			var newUser = new Enseignant(user.UserName)
//			{
//				Id = user.Id,
//				//EcoleId = ecoleId,
//			};
//			//newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<Enseignant>().HasData(newUser);

//			return newUser;
//		}

//		private static Employeur AddEmployeur(ModelBuilder builder, Utilisateur user, 
//			string codeP, string numRue, string nomRue, string ville, string pays)
//		{
//			var newUser = new Employeur(user.UserName)
//			{
//				Id = user.Id,
//			};
			
//			newUser.adresse = new Adresse()
//			{
//				Id = Guid.NewGuid(),
//				NumeroDeRue = numRue,
//				NomDeRue = nomRue,
//				codePostal = codeP,
//				ville = ville,
//				pays = pays
//			};
//			//newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<Employeur>().HasData(newUser);
//			//builder.Entity<Adresse>().HasData(newUser.adresse);
			
//			return newUser;
//		}

//		private static Coordonateur AddCoordo(ModelBuilder builder, Utilisateur user)
//		{
//			var newUser = new Coordonateur(user.UserName)
//			{
//				Id = user.Id,
//			};
//			//newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<Coordonateur>().HasData(newUser);

//			return newUser;
//		}

//		private static Stagiaire AddIntern(ModelBuilder builder, Utilisateur user,
//			Guid idEnseignant, Guid idHoraire, Guid idChat)
//		{
//			var newUser = new Stagiaire(user.UserName)
//			{
//				Id = user.Id,
//				idEnseignant = idEnseignant,
//				idHoraire = idHoraire,
//				idChat = idChat,
//			};
//			//newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<Stagiaire>().HasData(newUser);

//			return newUser;
//		}

//		private static MDS AddMds(ModelBuilder builder, Utilisateur user, 
//			string idMatricule, Status status,
//			Civilite civilite, TypeEmployeur typeEmployeur, bool actif,
//			string telMaison, string accredication, string commentaire, 
//			string commentaireCIUSS, string nomEmployeur, Guid idStagiaire,
//			Guid idEmployeur, Guid idHoraire, Guid idChat)
//		{
//			var newUser = new MDS(user.UserName)
//			{
//				Id = user.Id,
//				idMatricule = idMatricule,
//				status = status,
//				civilite = civilite,
//				typeEmployeur = typeEmployeur,
//				actif = actif,
//				telMaison = telMaison,	
//				accreditation = accredication,
//				commentaire = commentaire,
//				commentaireCIUSS = commentaireCIUSS,
//				NomEmployeur = nomEmployeur,
//				idStagiaire = idStagiaire,
//				idEmployeur = idEmployeur,
//				idHoraire = idHoraire,
//				idChat = idChat,
//			};
//			//newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
//			builder.Entity<MDS>().HasData(newUser);

//			return newUser;
//		}
//	}
//}


