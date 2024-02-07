
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
        //public DbSet<ConfirmationTemps> ConfirmationTemps { get; set; }
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
