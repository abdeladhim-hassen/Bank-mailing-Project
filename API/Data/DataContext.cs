using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Data;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<Agence> Agences { get; set; }

    public virtual DbSet<CartEvenement> CartEvenements { get; set; }

    public virtual DbSet<Carte> Cartes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<ClientNotification> ClientNotifications { get; set; }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<CreditEvenement> CreditEvenements { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Carte>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("CartDateExpirationTrigger");
                    tb.HasTrigger("CartStatutTrigger");
                    tb.HasTrigger("CarteInsertTrigger");
                });
        });
 
        modelBuilder.Entity<Agence>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(e => e.Clients)
                .WithOne(c => c.Agence)
                .HasForeignKey(c => c.AgenceId);
        });

        modelBuilder.Entity<Carte>(entity =>
        {
            entity.HasKey(e => e.NumeroCarte);

            entity.HasMany(e => e.CartEvenements)
                .WithOne(e => e.Carte)
                .HasForeignKey(e => e.CarteId);

            entity.HasOne(e => e.Client)
                .WithMany(c => c.Cartes)
                .HasForeignKey(e => e.ClientId);
        });

        modelBuilder.Entity<CartEvenement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Carte)
                .WithMany(c => c.CartEvenements)
                .HasForeignKey(e => e.CarteId);

            entity.HasOne(e => e.Template)
                .WithMany(t => t.CartEvenements)
                .HasForeignKey(e => e.TemplateId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Templates)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Agence)
                .WithMany(a => a.Clients)
                .HasForeignKey(e => e.AgenceId);

            entity.HasMany(e => e.Cartes)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            entity.HasMany(e => e.Comptes)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            entity.HasMany(e => e.ClientNotifications)
                .WithOne(n => n.Client)
                .HasForeignKey(n => n.ClientId);
        });

        modelBuilder.Entity<ClientNotification>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.NotificationId });

            entity.HasOne(e => e.Client)
                .WithMany(c => c.ClientNotifications)
                .HasForeignKey(e => e.ClientId);

            entity.HasOne(e => e.Notification)
                .WithMany(n => n.ClientNotifications)
                .HasForeignKey(e => e.NotificationId);
        });

        modelBuilder.Entity<Compte>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.SoldeCompte).HasColumnType("decimal(18, 3)");

            entity.HasOne(e => e.Client)
                .WithMany(c => c.Comptes)
                .HasForeignKey(e => e.ClientId);

            entity.HasMany(e => e.Credits)
                .WithOne(c => c.Compte)
                .HasForeignKey(c => c.CompteId);
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MontantCredit).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.RestCredit).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.CreditMensuelle).HasColumnType("decimal(18, 3)");

            entity.HasOne(e => e.Compte)
                .WithMany(c => c.Credits)
                .HasForeignKey(e => e.CompteId);

            entity.HasMany(e => e.CreditEvenements)
                .WithOne(e => e.Credit)
                .HasForeignKey(e => e.CreditId);
        });

        modelBuilder.Entity<CreditEvenement>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Credit)
                .WithMany(c => c.CreditEvenements)
                .HasForeignKey(e => e.CreditId);

            entity.HasOne(e => e.Template)
                .WithMany(t => t.CreditEvenements)
                .HasForeignKey(e => e.TemplateId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(e => e.ClientNotifications)
                .WithOne(cn => cn.Notification)
                .HasForeignKey(cn => cn.NotificationId);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(e => e.CartEvenements)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId);

            entity.HasMany(e => e.CreditEvenements)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Templates)
                .HasForeignKey(e => e.CategoryId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(e => e.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);
        });


        // Seeding data for models

        byte[] passwordHash, passwordSalt;
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456789"));
        }

        Random random = new();
        int daysOffset = random.Next(0, 30);
        DateTime randomDate = DateTime.Now.AddDays(-daysOffset);
       

        modelBuilder.Entity<Agence>().HasData(
            new Agence { Id = 1, Nom = "Ariana" },
            new Agence { Id = 2, Nom = "Béja" },
            new Agence { Id = 3, Nom = "Ben Arous" },
            new Agence { Id = 4, Nom = "Bizerte" },
            new Agence { Id = 5, Nom = "Gabès" },
            new Agence { Id = 6, Nom = "Gafsa" },
            new Agence { Id = 7, Nom = "Jendouba" },
            new Agence { Id = 8, Nom = "Kairouan" },
            new Agence { Id = 9, Nom = "Kasserine" },
            new Agence { Id = 10, Nom = "Kébili" },
            new Agence { Id = 11, Nom = "Kef" },
            new Agence { Id = 12, Nom = "Mahdia" },
            new Agence { Id = 13, Nom = "Manouba" },
            new Agence { Id = 14, Nom = "Médenine" },
            new Agence { Id = 15, Nom = "Monastir" },
            new Agence { Id = 16, Nom = "Nabeul" },
            new Agence { Id = 17, Nom = "Sfax" },
            new Agence { Id = 18, Nom = "Sidi Bouzid" },
            new Agence { Id = 19, Nom = "Siliana" },
            new Agence { Id = 20, Nom = "Sousse" },
            new Agence { Id = 21, Nom = "Tataouine" },
            new Agence { Id = 22, Nom = "Tozeur" },
            new Agence { Id = 23, Nom = "Tunis" },
            new Agence { Id = 24, Nom = "Zaghouan" });
        modelBuilder.Entity<User>().HasData(
           new User
           {
               Id = 1,
               AgenceId = 23,
               Login = "Admin",
               Email = "admin@contact.com",
               FirstName = "Admin",
               LastName = "Admin",
               Telephone = "20123123",
               PasswordHash = passwordHash,
               PasswordSalt = passwordSalt,
               DateCreated = DateTime.Now,
               Role = "Admin",
               Etat = true
           });
        modelBuilder.Entity<Client>().HasData(
        new Client
        {
            Id = 1,
            Nom = "John",
            Prenom = "Doe",
            Adresse = "123 Main St",
            NumeroTelephone = "555-1234",
            AdresseEmail = "john.doe@example.com",
            DateNaissance = new DateTime(1990, 1, 1),
            AgenceId = 1,
            CanalPrefere = "WHATSAPP"
        },
        new Client
        {
            Id = 2,
            Nom = "Jane",
            Prenom = "Smith",
            Adresse = "456 Elm St",
            NumeroTelephone = "555-5678",
            AdresseEmail = "jane.smith@example.com",
            DateNaissance = new DateTime(1985, 5, 10),
            AgenceId = 2,
            CanalPrefere = "SMS"
        },
        new Client
        {
            Id = 3,
            Nom = "John",
            Prenom = "Doe",
            Adresse = "123 Main St",
            NumeroTelephone = "555-1234",
            AdresseEmail = "john.doe@example.com",
            DateNaissance = new DateTime(1990, 1, 1),
            AgenceId = 1,
            CanalPrefere = "WHATSAPP"
        },
        new Client
        {
            Id = 4,
            Nom = "Jane",
            Prenom = "Smith",
            Adresse = "456 Elm St",
            NumeroTelephone = "555-5678",
            AdresseEmail = "jane.smith@example.com",
            DateNaissance = new DateTime(1985, 5, 10),
            AgenceId = 2,
            CanalPrefere = "SMS"
        },
        new Client
        {
            Id = 5,
            Nom = "Michael",
            Prenom = "Johnson",
            Adresse = "789 Oak St",
            NumeroTelephone = "555-7890",
            AdresseEmail = "michael.johnson@example.com",
            DateNaissance = new DateTime(1982, 8, 15),
            AgenceId = 3,
            CanalPrefere = "EMAIL"
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Carte" },
            new Category { Id = 2, Name = "Credit" }
        );

        modelBuilder.Entity<Compte>().HasData(
               new Compte
               {
                   Id = 1,
                   NumeroCompte = 1001,
                   ClientId = 1,
                   SoldeCompte = 1000,
                   TypeCompte = "Checking",
                   DateOuvertureCompte = DateTime.Now,
                   StatutCompte = "Active"
               },
               new Compte
               {
                   Id = 2,
                   NumeroCompte = 1002,
                   ClientId = 2,
                   SoldeCompte = 2000,
                   TypeCompte = "Savings",
                   DateOuvertureCompte = DateTime.Now,
                   StatutCompte = "Active"
               },
               new Compte
               {
                   Id = 3,
                   NumeroCompte = 1003,
                   ClientId = 3,
                   SoldeCompte = 1500,
                   TypeCompte = "Investment",
                   DateOuvertureCompte = DateTime.Now,
                   StatutCompte = "Active"
               },
               new Compte
               {
                   Id = 4,
                   NumeroCompte = 1004,
                   ClientId = 1,
                   SoldeCompte = 3000,
                   TypeCompte = "Credit",
                   DateOuvertureCompte = DateTime.Now,
                   StatutCompte = "Active"
               },
               new Compte
               {
                   Id = 5,
                   NumeroCompte = 1005,
                   ClientId = 2,
                   SoldeCompte = 5000,
                   TypeCompte = "Loan",
                   DateOuvertureCompte = DateTime.Now,
                   StatutCompte = "Active"
               }
           );

        modelBuilder.Entity<Credit>().HasData(
            new Credit
            {
                Id = 1,
                CompteId = 1,
                MontantCredit = 500,
                RestCredit = 500,
                CreditMensuelle = 100,
                JourPaiement = 1,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 2,
                CompteId = 2,
                MontantCredit = 1000,
                RestCredit = 800,
                CreditMensuelle = 200,
                JourPaiement = 5,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 3,
                CompteId = 3,
                MontantCredit = 1200,
                RestCredit = 1000,
                CreditMensuelle = 300,
                JourPaiement = 10,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 4,
                CompteId = 4,
                MontantCredit = 800,
                RestCredit = 500,
                CreditMensuelle = 150,
                JourPaiement = 15,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 5,
                CompteId = 5,
                MontantCredit = 600,
                RestCredit = 300,
                CreditMensuelle = 100,
                JourPaiement = 20,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 6,
                CompteId = 1,
                MontantCredit = 1500,
                RestCredit = 1200,
                CreditMensuelle = 250,
                JourPaiement = 25,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 7,
                CompteId = 2,
                MontantCredit = 2000,
                RestCredit = 1500,
                CreditMensuelle = 300,
                JourPaiement = 3,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 8,
                CompteId = 3,
                MontantCredit = 700,
                RestCredit = 400,
                CreditMensuelle = 150,
                JourPaiement = 7,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 9,
                CompteId = 4,
                MontantCredit = 900,
                RestCredit = 700,
                CreditMensuelle = 200,
                JourPaiement = 12,
                LastVerificationDate = randomDate
            },
            new Credit
            {
                Id = 10,
                CompteId = 5,
                MontantCredit = 1500,
                RestCredit = 1200,
                CreditMensuelle = 300,
                JourPaiement = 18,
                LastVerificationDate = randomDate
            }
        );


        modelBuilder.Entity<Template>().HasData(
        new Template
        {
            Id = 1,
            CategoryId = 1,
            Type = "Nouvelle Carte",
            EmailBody = "Votre nouvelle carte est prête à être utilisée.",
            SMSMessage = "Votre nouvelle carte est activée.",
            WhatsMessage = "Votre nouvelle carte est prête à l'emploi."
        },
        new Template
        {
            Id = 2,
            CategoryId = 1,
            Type = "Carte Activé",
            EmailBody = "Votre carte a été activée avec succès.",
            SMSMessage = "Votre carte est maintenant active.",
            WhatsMessage = "Votre carte est activée et prête à l'emploi."
        },
        new Template
        {
            Id = 3,
            CategoryId = 1,
            Type = "Carte Bloqué",
            EmailBody = "Votre carte a été bloquée. Veuillez contacter le support.",
            SMSMessage = "Votre carte est bloquée. Contactez le support.",
            WhatsMessage = "Votre carte est bloquée. Veuillez nous contacter."
        },
        new Template
        {
            Id = 4,
            CategoryId = 1,
            Type = "Carte Expiration",
            EmailBody = "Votre carte expirera bientôt. Veuillez la renouveler.",
            SMSMessage = "Votre carte expirera bientôt.",
            WhatsMessage = "Votre carte arrive à expiration. Pensez à la renouveler."
        },
        new Template
        {
            Id = 5,
            CategoryId = 2,
            Type = "Confirmation de Paiement",
            EmailBody = "Votre paiement a été confirmé avec succès.",
            SMSMessage = "Paiement confirmé. Merci.",
            WhatsMessage = "Votre paiement a été effectué avec succès."
        },
        new Template
        {
            Id = 6,
            CategoryId = 2,
            Type = "Rappel de Paiement",
            EmailBody = "Un rappel pour votre paiement en attente.",
            SMSMessage = "Rappel: paiement en attente.",
            WhatsMessage = "N'oubliez pas de régler votre paiement en attente."
        },
        new Template
        {
            Id = 7,
            CategoryId = 2,
            Type = "Notification de fin de prêt",
            EmailBody = "Votre prêt est terminé. Merci de votre fidélité.",
            SMSMessage = "Prêt terminé. Merci.",
            WhatsMessage = "Votre prêt est terminé. Nous vous remercions pour votre confiance."
        }
    );


        modelBuilder.Entity<Carte>().HasData(
        new Carte
        {
            NumeroCarte = 1001,
            Bin = 123456,
            DateDelivrance = DateTime.Now.AddDays(-365),
            DateExpiration = DateTime.Now.AddYears(3),
            Statut = true,
            PlafondGAB = 500,
            PlafondTPE = 1000,
            ResteGAB = 500,
            ResteTPE = 1000,
            Mobile = 123456789,
            ClientId = 1
        },
        new Carte
        {
            NumeroCarte = 1002,
            Bin = 987654,
            DateDelivrance = DateTime.Now.AddDays(-400),
            DateExpiration = DateTime.Now.AddYears(4),
            Statut = true,
            PlafondGAB = 1000,
            PlafondTPE = 2000,
            ResteGAB = 800,
            ResteTPE = 1800,
            Mobile = 987654321,
            ClientId = 2
        },
        new Carte
        {
            NumeroCarte = 1003,
            Bin = 654321,
            DateDelivrance = DateTime.Now.AddDays(-300),
            DateExpiration = DateTime.Now.AddYears(2),
            Statut = true,
            PlafondGAB = 800,
            PlafondTPE = 1500,
            ResteGAB = 600,
            ResteTPE = 1200,
            Mobile = 111222333,
            ClientId = 1
        },
        new Carte
        {
            NumeroCarte = 1004,
            Bin = 987123,
            DateDelivrance = DateTime.Now.AddDays(-200),
            DateExpiration = DateTime.Now.AddYears(3),
            PlafondGAB = 1200,
            PlafondTPE = 2500,
            ResteGAB = 1000,
            ResteTPE = 2200,
            Mobile = 444555666,
            ClientId = 3
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
