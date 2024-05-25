using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class CreditMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMSMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationCodeGeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Etat = table.Column<bool>(type: "bit", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresseEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgenceId = table.Column<int>(type: "int", nullable: false),
                    CanalPrefere = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Agences_AgenceId",
                        column: x => x.AgenceId,
                        principalTable: "Agences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMSMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cartes",
                columns: table => new
                {
                    NumeroCarte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bin = table.Column<int>(type: "int", nullable: false),
                    DateDelivrance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<bool>(type: "bit", nullable: false),
                    PlafondGAB = table.Column<int>(type: "int", nullable: false),
                    PlafondTPE = table.Column<int>(type: "int", nullable: false),
                    ResteGAB = table.Column<int>(type: "int", nullable: false),
                    ResteTPE = table.Column<int>(type: "int", nullable: false),
                    Mobile = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartes", x => x.NumeroCarte);
                    table.ForeignKey(
                        name: "FK_Cartes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientNotifications",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNotifications", x => new { x.ClientId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_ClientNotifications_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comptes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCompte = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoldeCompte = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TypeCompte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOuvertureCompte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatutCompte = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comptes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartEvenements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Canal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeureEnvoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    CarteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartEvenements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartEvenements_Cartes_CarteId",
                        column: x => x.CarteId,
                        principalTable: "Cartes",
                        principalColumn: "NumeroCarte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartEvenements_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontantCredit = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    RestCredit = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    CreditMensuelle = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    LastVerificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JourPaiement = table.Column<int>(type: "int", nullable: false),
                    CompteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credits_Comptes_CompteId",
                        column: x => x.CompteId,
                        principalTable: "Comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditEvenements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Canal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeureEnvoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    CreditId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditEvenements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditEvenements_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditEvenements_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agences",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "Ariana" },
                    { 2, "Béja" },
                    { 3, "Ben Arous" },
                    { 4, "Bizerte" },
                    { 5, "Gabès" },
                    { 6, "Gafsa" },
                    { 7, "Jendouba" },
                    { 8, "Kairouan" },
                    { 9, "Kasserine" },
                    { 10, "Kébili" },
                    { 11, "Kef" },
                    { 12, "Mahdia" },
                    { 13, "Manouba" },
                    { 14, "Médenine" },
                    { 15, "Monastir" },
                    { 16, "Nabeul" },
                    { 17, "Sfax" },
                    { 18, "Sidi Bouzid" },
                    { 19, "Siliana" },
                    { 20, "Sousse" },
                    { 21, "Tataouine" },
                    { 22, "Tozeur" },
                    { 23, "Tunis" },
                    { 24, "Zaghouan" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Carte" },
                    { 2, "Credit" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "DateCreated", "Email", "Etat", "FirstName", "LastName", "Login", "PasswordHash", "PasswordSalt", "Role", "Telephone", "VerificationCode", "VerificationCodeGeneratedAt" },
                values: new object[] { 1, "", new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7116), "admin@contact.com", true, "Admin", "Admin", "Admin", new byte[] { 113, 50, 116, 5, 115, 212, 43, 132, 18, 71, 244, 156, 48, 174, 214, 238, 180, 96, 0, 99, 244, 116, 143, 106, 15, 95, 225, 16, 16, 16, 202, 215, 71, 151, 182, 70, 167, 24, 78, 166, 202, 40, 125, 32, 224, 239, 79, 231, 182, 161, 24, 180, 121, 254, 64, 192, 191, 111, 195, 23, 251, 242, 71, 116 }, new byte[] { 115, 52, 105, 25, 80, 198, 248, 195, 154, 139, 191, 72, 70, 142, 231, 72, 90, 133, 253, 95, 224, 87, 13, 51, 120, 54, 157, 50, 69, 46, 211, 31, 249, 192, 45, 6, 128, 196, 86, 124, 143, 87, 132, 117, 240, 211, 164, 204, 251, 168, 232, 243, 98, 32, 241, 233, 178, 188, 169, 183, 96, 135, 210, 26, 115, 232, 162, 247, 156, 113, 248, 200, 91, 69, 126, 82, 196, 220, 100, 215, 40, 204, 244, 213, 162, 69, 137, 116, 202, 235, 248, 227, 123, 88, 207, 144, 112, 44, 150, 161, 24, 148, 0, 221, 231, 86, 219, 226, 208, 245, 244, 69, 160, 24, 145, 210, 198, 222, 124, 237, 255, 250, 243, 109, 92, 201, 103, 113 }, "Admin", "20123123", "", null });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Adresse", "AdresseEmail", "AgenceId", "CanalPrefere", "DateNaissance", "Nom", "NumeroTelephone", "Prenom" },
                values: new object[,]
                {
                    { 1, "123 Main St", "john.doe@example.com", 1, "WHATSAPP", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "555-1234", "Doe" },
                    { 2, "456 Elm St", "jane.smith@example.com", 2, "SMS", new DateTime(1985, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane", "555-5678", "Smith" },
                    { 3, "123 Main St", "john.doe@example.com", 1, "WHATSAPP", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "555-1234", "Doe" },
                    { 4, "456 Elm St", "jane.smith@example.com", 2, "SMS", new DateTime(1985, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane", "555-5678", "Smith" },
                    { 5, "789 Oak St", "michael.johnson@example.com", 3, "EMAIL", new DateTime(1982, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michael", "555-7890", "Johnson" }
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "CategoryId", "EmailBody", "SMSMessage", "Type", "WhatsMessage" },
                values: new object[,]
                {
                    { 1, 1, "Votre nouvelle carte est prête à être utilisée.", "Votre nouvelle carte est activée.", "Nouvelle Carte", "Votre nouvelle carte est prête à l'emploi." },
                    { 2, 1, "Votre carte a été activée avec succès.", "Votre carte est maintenant active.", "Carte Activé", "Votre carte est activée et prête à l'emploi." },
                    { 3, 1, "Votre carte a été bloquée. Veuillez contacter le support.", "Votre carte est bloquée. Contactez le support.", "Carte Bloqué", "Votre carte est bloquée. Veuillez nous contacter." },
                    { 4, 1, "Votre carte expirera bientôt. Veuillez la renouveler.", "Votre carte expirera bientôt.", "Carte Expiration", "Votre carte arrive à expiration. Pensez à la renouveler." },
                    { 5, 2, "Votre paiement a été confirmé avec succès.", "Paiement confirmé. Merci.", "Confirmation de Paiement", "Votre paiement a été effectué avec succès." },
                    { 6, 2, "Un rappel pour votre paiement en attente.", "Rappel: paiement en attente.", "Rappel de Paiement", "N'oubliez pas de régler votre paiement en attente." },
                    { 7, 2, "Votre prêt est terminé. Merci de votre fidélité.", "Prêt terminé. Merci.", "Notification de fin de prêt", "Votre prêt est terminé. Nous vous remercions pour votre confiance." }
                });

            migrationBuilder.InsertData(
                table: "Cartes",
                columns: new[] { "NumeroCarte", "Bin", "ClientId", "DateDelivrance", "DateExpiration", "Mobile", "PlafondGAB", "PlafondTPE", "ResteGAB", "ResteTPE", "Statut" },
                values: new object[,]
                {
                    { 1001, 123456, 1, new DateTime(2023, 5, 24, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7710), new DateTime(2027, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7712), 123456789, 500, 1000, 500, 1000, true },
                    { 1002, 987654, 2, new DateTime(2023, 4, 19, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7720), new DateTime(2028, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7721), 987654321, 1000, 2000, 800, 1800, true },
                    { 1003, 654321, 1, new DateTime(2023, 7, 28, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7725), new DateTime(2026, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7726), 111222333, 800, 1500, 600, 1200, true },
                    { 1004, 987123, 3, new DateTime(2023, 11, 5, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7729), new DateTime(2027, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7730), 444555666, 1200, 2500, 1000, 2200, false }
                });

            migrationBuilder.InsertData(
                table: "Comptes",
                columns: new[] { "Id", "ClientId", "DateOuvertureCompte", "NumeroCompte", "SoldeCompte", "StatutCompte", "TypeCompte" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7550), 1001, 1000m, "Active", "Checking" },
                    { 2, 2, new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7554), 1002, 2000m, "Active", "Savings" },
                    { 3, 3, new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7557), 1003, 1500m, "Active", "Investment" },
                    { 4, 1, new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7560), 1004, 3000m, "Active", "Credit" },
                    { 5, 2, new DateTime(2024, 5, 23, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7564), 1005, 5000m, "Active", "Loan" }
                });

            migrationBuilder.InsertData(
                table: "Credits",
                columns: new[] { "Id", "CompteId", "CreditMensuelle", "JourPaiement", "LastVerificationDate", "MontantCredit", "RestCredit" },
                values: new object[,]
                {
                    { 1, 1, 100m, 1, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 500m, 500m },
                    { 2, 2, 200m, 5, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 1000m, 800m },
                    { 3, 3, 300m, 10, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 1200m, 1000m },
                    { 4, 4, 150m, 15, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 800m, 500m },
                    { 5, 5, 100m, 20, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 600m, 300m },
                    { 6, 1, 250m, 25, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 1500m, 1200m },
                    { 7, 2, 300m, 3, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 2000m, 1500m },
                    { 8, 3, 150m, 7, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 700m, 400m },
                    { 9, 4, 200m, 12, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 900m, 700m },
                    { 10, 5, 300m, 18, new DateTime(2024, 4, 27, 15, 24, 57, 836, DateTimeKind.Local).AddTicks(7019), 1500m, 1200m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cartes_ClientId",
                table: "Cartes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CartEvenements_CarteId",
                table: "CartEvenements",
                column: "CarteId");

            migrationBuilder.CreateIndex(
                name: "IX_CartEvenements_TemplateId",
                table: "CartEvenements",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientNotifications_NotificationId",
                table: "ClientNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AgenceId",
                table: "Clients",
                column: "AgenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comptes_ClientId",
                table: "Comptes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditEvenements_CreditId",
                table: "CreditEvenements",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditEvenements_TemplateId",
                table: "CreditEvenements",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_CompteId",
                table: "Credits",
                column: "CompteId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CategoryId",
                table: "Templates",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartEvenements");

            migrationBuilder.DropTable(
                name: "ClientNotifications");

            migrationBuilder.DropTable(
                name: "CreditEvenements");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Cartes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Comptes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Agences");
        }
    }
}
