
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPU.Domain.Entites;



namespace SPU.Domain
{
    public class SpuContext : IdentityDbContext<Utilisateur, IdentityRole<Guid>, Guid>
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<Chat> Chats { get; set; }

        //public DbSet<Contrat> Contrats { get; set; }
        public DbSet<Coordonateur> Coordonateurs { get; set; }

        public DbSet<Employeur> Employeurs { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Horaire> Horaires { get; set;}
        public DbSet<MDS> MDS { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<PlageHoraire> PlageHoraires { get; set; }
        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<Ecole> Ecole { get; set; }

        public SpuContext(DbContextOptions<SpuContext> options) : base(options) { }

        public SpuContext() : base(GetOptions()) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Stagiaire>().HasOne(s => s.MDS1).WithMany().HasForeignKey(s => s.MDSId1).OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<Stagiaire>().HasOne(s => s.MDS2).WithMany().HasForeignKey(s => s.MDSId2).OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<MDS>().HasOne(m => m.stagiaire).WithMany().HasForeignKey(m => m.StagiaireId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<MDS>().HasOne(m => m.stagiaire).WithMany().HasForeignKey(m => m.StagiaireId).OnDelete(DeleteBehavior.SetNull); 
            modelBuilder.Entity<MDS>().HasOne(m => m.employeur).WithMany().HasForeignKey(m => m.EmployeurId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<MDS>().HasOne(m => m.chat).WithMany().HasForeignKey(m => m.ChatId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Stagiaire>().HasOne(m => m.enseignant).WithMany().HasForeignKey(c => c.EnseignantId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Stagiaire>().HasOne(m => m.ecole).WithMany().HasForeignKey(c => c.EcoleId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Stagiaire>().HasOne(c => c.chat).WithMany().HasForeignKey(c => c.ChatId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Stagiaire>().HasOne(c => c.employeur).WithMany().HasForeignKey(c => c.EmployeurId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Coordonateur>().HasOne(c => c.ecole).WithMany().HasForeignKey(c => c.EcoleId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Enseignant>().HasOne(c => c.ecole).WithMany().HasForeignKey(c => c.EcoleId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employeur>().HasOne(c => c.adresse).WithMany().HasForeignKey(c => c.AdresseId).OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Seed();
        }

        private static DbContextOptions<SpuContext> GetOptions()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json")
                                                    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<SpuContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }
    }
      
}
