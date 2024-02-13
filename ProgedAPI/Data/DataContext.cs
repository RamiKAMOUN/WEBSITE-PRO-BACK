using Microsoft.EntityFrameworkCore;
using ProgedAPI.Entities;

namespace ProgedAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options)
        {
            
        }
       
        public DbSet<Histoire> Histoires { get; set; }
        public DbSet<Temoignage> Temoignages { get; set; }
        public DbSet<Cervice> Cervices { get; set; }

        public DbSet<NotreHistoire> NotreHistoires { get; set; }
        public DbSet<Famille> Familles { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Dirigeant> Dirigeants { get; set; }
        public DbSet<Poste> Postes { get; set; }
                
        public DbSet<Message> Messages { get; set; }

        public DbSet<NosTechnolg> NosTechnolgs { get; set; }

       public DbSet<QueCherzV> QueCherzVs { get; set; }

        public DbSet<Client> Clients { get; set; }

    }
}
