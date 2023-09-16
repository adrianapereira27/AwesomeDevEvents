using AwesomeDevEvents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Persistence
{
    public class DevEventsDbContext : DbContext  // herança
    {
        public DbSet<DevEvent> DevEvents { get; set; } // tabela DevEvents
        public DbSet<DevEventSpeaker> DevEventSpeakers { get; set; } // tabela DevEventSpeaker

        // construtor da classe (EntityFrameworkCore vai usar para add a configuração na classe Program.cs)
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(e => e.Id);  // define a chave da tabela DevEvents

                e.Property(de => de.Title).IsRequired(false); // define que o titulo não é obrigatório (pode ser nulo)
                
                e.Property(de => de.Description)
                    .HasMaxLength(200)              // tamanho máximo do campo Description
                    .HasColumnType("varchar(200)"); // tipo de coluna do campo Description no banco de dados

                e.Property(de => de.StartDate)
                    .HasColumnName("Start_Date");   // o nome do campo no banco de dados

                e.Property(de => de.EndDate)
                    .HasColumnName("End_Date");   // o nome da coluna no banco de dados

                e.HasMany(de => de.Speakers)  // um devEvent tem muitos Speakers
                    .WithOne()        // o speaker só tem um evento (se ele for speaker em outro evento terá que fazer um novo cadastro) 
                    .HasForeignKey(s => s.DevEventId);  // chave estrangeira na tabela speakers

            });
            builder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(e => e.Id);  // define a chave da tabela DevEventSpeaker
            });
        }
    }
}
