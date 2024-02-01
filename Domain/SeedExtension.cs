using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPU.Domain.Entites;

namespace SPU.Domain
{
    public static class SeedExtension
    {

		public static readonly PasswordHasher<Utilisateur> PASSWORD_HASHER = new();

		public static void Seed(this ModelBuilder builder)
		{
			var Coordinateur = AddRole(builder, "Coordinateur");
			var Enseignant = AddRole(builder, "Enseignant");
			var MaitreDeStage = AddRole(builder, "MaitreDeStage");
			var Stagiaire = AddRole(builder, "Stagiaire");
			var Employeur = AddRole(builder, "Employeur");

			var user0 = AddUser(builder, "user0", "user0@gmail.com", "Qwerty123!", "Liam", "O'Brien", "1234567899", new DateTime(1989, 10, 10));
			AddUserToRole(builder, user0, Stagiaire);
			var user1 = AddUser(builder, "user1", "user1@gmail.com", "password1", "John", "Doe", "1234567890", new DateTime(1980, 1, 1));
			AddUserToRole(builder, user1, Stagiaire);
			var user2 = AddUser(builder, "user2", "user2@gmail.com", "password2", "Jane", "Smith", "1234567891", new DateTime(1981, 2, 2));
			AddUserToRole(builder, user2 , Stagiaire);

			var user3 = AddUser(builder, "user3", "user3@gmail.com", "Pass123!", "Aarav", "Patel", "1234567892", new DateTime(1982, 3, 3));
			AddUserToRole (builder, user3, Employeur);

			var user4 = AddUser(builder, "user4", "user4@gmail.com", "W0rld4me", "Ying", "Li", "1234567893", new DateTime(1983, 4, 4));
			AddUserToRole(builder, user4 , Coordinateur);
			var user5 = AddUser(builder, "user5", "user5@gmail.com", "S3cur1ty$", "Olivia", "Garcia", "1234567894", new DateTime(1984, 5, 5));
			AddUserToRole(builder, user5 , Coordinateur);

			var user6 = AddUser(builder, "user6", "user6@gmail.com", "Chang3m3", "Mohamed", "Hassan", "1234567895", new DateTime(1985, 6, 6));
			AddUserToRole(builder , user6 , Enseignant);
			var user7 = AddUser(builder, "user7", "user7@gmail.com", "Passw0rd7", "Ivan", "Ivanov", "1234567896", new DateTime(1986, 7, 7));
			AddUserToRole(builder, user7 , Enseignant);

			var user8 = AddUser(builder, "user8", "user8@gmail.com", "8LuckyNum", "Chloe", "Kim", "1234567897", new DateTime(1987, 8, 8));
			AddUserToRole(builder, user8 , MaitreDeStage);
			var user9 = AddUser(builder, "user9", "user9@gmail.com", "Nin3Lives", "Amelia", "Santos", "1234567898", new DateTime(1988, 9, 9));
			AddUserToRole(builder, user9, MaitreDeStage);

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

		private static void AddUserToRole(ModelBuilder builder, Utilisateur user, IdentityRole<Guid> adminRole)
		{
			builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
			{
				UserId = user.Id,
				RoleId = adminRole.Id,
			});
		}

		private static Utilisateur AddUser(ModelBuilder builder, string userName,
			string email, string password, string prenom, string nom,
			string phoneNumber, DateTime dateOfBirth)
		{
			var newUser = new Utilisateur(userName)
			{
				Id = Guid.NewGuid(),
				Email = email,
				NormalizedEmail = email.ToUpper(),
				UserName = userName,
				NormalizedUserName = userName.ToUpper(),
				Prenom = prenom,
				Nom = nom,
				PhoneNumber = phoneNumber,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
			builder.Entity<Utilisateur>().HasData(newUser);

			return newUser;
		}
	}
}


