﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPU.Domain.Entites;
using SPU.Enum;
using System;
using System.Net;
using System.Security.Cryptography;

namespace SPU.Domain
{/// <summary>
/// A revoir, suite au changement dans le code, le seed n'est plus bon.
/// </summary>
	public static class SeedExtension
	{
		public static readonly PasswordHasher<Utilisateur> PASSWORD_HASHER = new();

		public static void Seed(this ModelBuilder builder)
		{
			var coordonnateur = AddRole(builder, "Coordonnateur");
            var employeur = AddRole(builder, "Employeur");
            var enseignant = AddRole(builder, "Enseignant");
            var mds = AddRole(builder, "MDS");
            var stagiaire = AddRole(builder, "Stagiaire");

            var adresse = AddAdresse(builder, "j2om0t", "125", "LaBelle", "St-Boire", "Canada", "Ici");
            var adresse1 = AddAdresse(builder, "h0h2h2", "1258", "Boeuf", "St-LaVoire", "Canada", "Ici");

            var ecole1 = AddEcole(builder, "Forest", "1255647894", adresse);
            var ecole = AddEcole(builder, "LaFonte", "1255647894", adresse1);

            var user0 = AddUser(builder, "user0", "Qwerty123!", "Liam", "O'Brien", "user0@gmail.com", "1234567899");
			var user2 = AddUser(builder, "employeur0", "Qwerty123!", "Lucas", "Martin", "employeur0@gmail.com", "1234567892");
			var user3 = AddUser(builder, "coordo0", "Qwerty123!", "Chloe", "Leroy", "coordo0@gmail.com", "1234567893");
			

			AddUserToRole(builder, user2, employeur);
			AddUserToRole(builder, user3, coordonnateur);
			

			var employeur0 = AddEmployeur(builder, user2, adresse);

			var coordo0 = AddCoordo(builder, user3, ecole);


            // ----- Enseignants
			var user1 = AddUser(builder, "enseignant0", "Qwerty123!", "Emma", "Dupont", "enseignant0@gmail.com", "1234567891");
            var user6 = AddUser(builder, "enseignant1", "Qwerty123!", "Noah", "Roux", "enseignant1@gmail.com", "1234567896");
            var user7 = AddUser(builder, "enseignant2", "Qwerty123!", "Alice", "Petit", "enseignant2@gmail.com", "1234567897");

			AddUserToRole(builder, user1, enseignant);
            AddUserToRole(builder, user6, enseignant);
            AddUserToRole(builder, user7, enseignant);

			var enseignant0 = AddEnseignant(builder, user1, ecole);
            var enseignant1 = AddEnseignant(builder, user6, ecole);
            var enseignant2 = AddEnseignant(builder, user7, ecole1);





            var chat = AddChat(builder, user0, enseignant0, coordo0);


            // Employeuryeurs
            var user8 = AddUser(builder, "employeur1", "Qwerty123!", "Ethan", "Lefebvre", "employeur1@gmail.com", "1234567898");
            var user9 = AddUser(builder, "employeur2", "Qwerty123!", "Léa", "Girard", "employeur2@gmail.com", "1234567899");
            AddUserToRole(builder, user8, employeur);
            AddUserToRole(builder, user9, employeur);
            var employeur1 = AddEmployeur(builder, user8, adresse);
            var employeur2 = AddEmployeur(builder, user9, adresse1);

            //// Coordinateurs
            //var user10 = AddUser(builder, "coordo1", "Coordo123!", "Gabriel", "David", "coordo1@gmail.com", "1234567800");
            //var user11 = AddUser(builder, "coordo2", "Coordo123!", "Jade", "Simon", "coordo2@gmail.com", "1234567801");
            //AddUserToRole(builder, user10, coordonateur);
            //AddUserToRole(builder, user11, coordonateur);
            //var coordo1 = AddCoordo(builder, user10, ecole1);
            //var coordo2 = AddCoordo(builder, user11, ecole);


            // ----- Stagiaires

			var user4 = AddUser(builder, "stagiaire0", "Qwerty123!", "Julien", "Moreau", "stagiaire0@gmail.com", "1234567894");
            var user12 = AddUser(builder, "stagiaire1", "Qwerty123!", "Maxime", "Chevalier", "stagiaire1@gmail.com", "1234567802");
            var user13 = AddUser(builder, "stagiaire2", "Qwerty123!", "Zoé", "Blanc", "stagiaire2@gmail.com", "1234567803");
            var user20 = AddUser(builder, "stagiaire3", "Qwerty123!", "Sophie", "Lévesque", "stagiaire3@gmail.com", "1234567810");
            var user21 = AddUser(builder, "stagiaire4", "Qwerty123!", "Louis", "Gagnon", "stagiaire4@gmail.com", "1234567811");

            // Attribution du rôle
            // Ajout à l'entité Stagiaire

            var chat1 = AddChat(builder, user12, enseignant1, coordo0);
            var chat2 = AddChat(builder, user13, enseignant2, coordo0);

            AddUserToRole(builder, user20, stagiaire);
            AddUserToRole(builder, user21, stagiaire);
			AddUserToRole(builder, user4, stagiaire);
            AddUserToRole(builder, user12, stagiaire);
            AddUserToRole(builder, user13, stagiaire);

			var stagiaire0 = AddStagiaire(builder, user4, enseignant0, chat, employeur0, ecole);
            var stagiaire1 = AddStagiaire(builder, user12, enseignant0, chat1, employeur1, ecole);
            var stagiaire2 = AddStagiaire(builder, user13, enseignant0, chat2, employeur2, ecole1);
            //var stagiaire3 = AddStagiaire(builder, user20, enseignant0, chat3, employeur2, ecole);
            //var stagiaire4 = AddStagiaire(builder, user21, enseignant0, chat4, employeur2, ecole);



            // ----- MDS

            var user5 = AddUser(builder, "mds0", "Qwerty123!", "Sarah", "Bernard", "mds0@gmail.com", "1234567895");
            var user14 = AddUser(builder, "mds1", "Qwerty123!", "Thomas", "Richard", "mds1@gmail.com", "1234567804");
            var user15 = AddUser(builder, "mds2", "Qwerty123!", "Anna", "Dufour", "mds2@gmail.com", "1234567805");
            var user16 = AddUser(builder, "mds3", "Qwerty123!", "Jean", "Dupont", "mds3@gmail.com", "1234567806");
            var user17 = AddUser(builder, "mds4", "Qwerty123!", "Marie", "Lavoie", "mds4@gmail.com", "1234567807");
            var user18 = AddUser(builder, "mds5", "Qwerty123!", "Émilie", "Tremblay", "mds5@gmail.com", "1234567808");
            var user19 = AddUser(builder, "mds6", "Qwerty123!", "Gabriel", "Lemieux", "mds6@gmail.com", "1234567809");


			AddUserToRole(builder, user5, mds);
            AddUserToRole(builder, user14, mds);
            AddUserToRole(builder, user15, mds);
            AddUserToRole(builder, user16, mds);
            AddUserToRole(builder, user17, mds);
            AddUserToRole(builder, user18, mds);
            AddUserToRole(builder, user19, mds);


            //Liaison entre mds0, mds1, stagiaire1, horaireX
            var mds0 = AddMds(builder, user5, "SPU123456", Status.Accepté, Civilite.M, TypeEmployeur.CIUSSS, true,
                "0987654321", "AccreditationX", "Commentaire exemple", "Commentaire CIUSS",
                "EmployeurX", null, employeur0, chat, new DateTime(2024,02,25).ToUniversalTime(), new DateTime(2026,02,25).ToUniversalTime());

            var mds1 = AddMds(builder, user14, "SPU123457", Status.Incomplet, Civilite.M, TypeEmployeur.CIUSSS, false,
                "0987654322", "AccreditationY", "Commentaire mds1", "Commentaire CIUSS mds1",
                "EmployeurY", null, employeur1, chat1, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            var mds2 = AddMds(builder, user15, "SPU123458", Status.Accepté, Civilite.Mme, TypeEmployeur.CISSS, true,
                "0987654323", "AccreditationZ", "Commentaire mds2", "Commentaire CIUSS mds2",
                "EmployeurZ", null, employeur2, chat2, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            var mds3 = AddMds(builder, user16, "SPU123459", Status.Accepté, Civilite.M, TypeEmployeur.CIUSSS, false,
                "0987654324", "AccreditationX", "Commentaire mds3", "Commentaire CIUSS mds3",
                "EmployeurX", null, employeur0, null, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            var mds4 = AddMds(builder, user17, "SPU123460", Status.Incomplet, Civilite.Mme, TypeEmployeur.CISSS, true,
                "0987654325", "AccreditationW", "Commentaire mds4", "Commentaire CIUSS mds4",
                "EmployeurW", null, employeur1, null, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            var mds5 = AddMds(builder, user18, "SPU123461", Status.Accepté, Civilite.Mme, TypeEmployeur.CIUSSS, false,
                    "0987654326", "AccreditationV", "Commentaire mds5", "Commentaire CIUSS mds5", 
                    "EmployeurV", null, employeur2, null, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            var mds6 = AddMds(builder, user19, "SPU123462", Status.Accepté, Civilite.M, TypeEmployeur.CISSS, true,
                "0987654327", "AccreditationU", "Commentaire mds6", "Commentaire CIUSS mds6",
                "EmployeurU", null, employeur0, null, new DateTime(2024, 02, 25).ToUniversalTime(), new DateTime(2026, 02, 25).ToUniversalTime());

            //Ajout nouvel horaire
            //AddHoraire(builder, mds0);
            //AddHoraire(builder, mds1);
            //AddHoraire(builder, mds2);
            //AddHoraire(builder, mds3);
            //AddHoraire(builder, mds4);
            //AddHoraire(builder, mds5);
            //AddHoraire(builder, mds6);

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

        private static void AddHoraire(ModelBuilder builder, MDS mds)
        {
            var newHoraire = new Horaire()
            {
                Id = Guid.NewGuid(),
                MDSId1 = mds.Id,
                mds1 = mds,
            };
            builder.Entity<Horaire>().HasData(newHoraire);
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
                EcoleId = ecole.id
            };
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

        private static Adresse AddAdresse(ModelBuilder builder, string codeP, 
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

        private static Coordonnateur AddCoordo(ModelBuilder builder, Utilisateur user, Ecole ecole)
		{
			var newUser = new Coordonnateur()
            {
                Id = Guid.NewGuid(),
				UtilisateurId=user.Id,
				EcoleId = ecole.id,
            };
			builder.Entity<Coordonnateur>().HasData(newUser);
			return newUser;
		}

		private static Stagiaire AddStagiaire(ModelBuilder builder, Utilisateur user,
			Enseignant enseignant, Chat chat, Employeur employeur, Ecole ecole)
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
			builder.Entity<Stagiaire>().HasData(newUser);

			return newUser;
		}

		private static MDS AddMds(ModelBuilder builder, Utilisateur user, 
			string idMatricule, Status status, Civilite civilite, TypeEmployeur 
			typeEmployeur, bool actif, string telMaison, string accredication, 
			string commentaire, string commentaireCIUSS, string nomEmployeur, 
			Stagiaire? stagiaire, Employeur employeur, Chat? chat, DateTime dateCreation, DateTime dateExpiration)
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
                StagiaireId = stagiaire?.Id,
				EmployeurId = employeur.Id,
				ChatId = chat?.Id,
                DateCreationHoraire = dateCreation,
                DateExpiration = dateExpiration,
                
			};
			builder.Entity<MDS>().HasData(newUser);

            return newUser;
        }

        private static Chat AddChat(ModelBuilder builder, Utilisateur user, Enseignant enseignant, Coordonnateur coordonateur)
        {
            var newChat = new Chat()
            {
                Id = Guid.NewGuid(),
				EnseignantId = enseignant.Id,
				//CoordonateurId = coordonateur.Id
            };
            builder.Entity<Chat>().HasData(newChat);

            return newChat;
        }

        private static void AddUserToRole(ModelBuilder builder, Utilisateur user, IdentityRole<Guid> role)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                UserId = user.Id,
                RoleId = role.Id,
            });
        }

        private static IdentityRole<Guid> AddRole(ModelBuilder builder, string name)
        {
            var newRole = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToUpper()
            };
            builder.Entity<IdentityRole<Guid>>().HasData(newRole);

            return newRole;
        }
    }
}


