using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Trigger for CartStatut
                CREATE TRIGGER CartStatutTrigger
                ON Cartes
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF UPDATE(Statut)
                    BEGIN
                        INSERT INTO CartEvenements (Canal, HeureEnvoi, TemplateId, CarteId)
                        SELECT 
                            ISNULL(c.CanalPrefere, 'SMS'), 
                            DATEADD(HOUR, 1, GETDATE()), -- Changed DAY to HOUR
                            CASE WHEN i.Statut = 1 THEN 2 ELSE 3 END, 
                            i.NumeroCarte
                        FROM inserted i
                        LEFT JOIN Clients c ON i.ClientId = c.Id;
                    END
                END;
            ");

            migrationBuilder.Sql(@"
                -- Trigger for CartDateExpiration
                CREATE TRIGGER CartDateExpirationTrigger
                ON Cartes
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF UPDATE(DateExpiration)
                    BEGIN
                        INSERT INTO CartEvenements (Canal, HeureEnvoi, TemplateId, CarteId)
                        SELECT 
                            ISNULL(c.CanalPrefere, 'SMS'), 
                            DATEADD(HOUR, 1, GETDATE()), -- Changed DAY to HOUR
                            4, 
                            i.NumeroCarte
                        FROM inserted i
                        LEFT JOIN Clients c ON i.ClientId = c.Id
                        WHERE i.DateExpiration < DATEADD(DAY, 15, GETDATE());
                    END
                END;
            ");

            migrationBuilder.Sql(@"
                -- Trigger for CarteInsert
                CREATE TRIGGER CarteInsertTrigger
                ON Cartes
                AFTER INSERT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO CartEvenements (Canal, HeureEnvoi, TemplateId, CarteId)
                    SELECT 
                        ISNULL(c.CanalPrefere, 'SMS'), 
                        DATEADD(HOUR, 1, GETDATE()), -- Changed DAY to HOUR
                        1, 
                        i.NumeroCarte
                    FROM inserted i
                    LEFT JOIN Clients c ON i.ClientId = c.Id;
                END;
            ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS CartStatutTrigger;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS CartDateExpirationTrigger;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS CarteInsertTrigger;");
        }
    }
}
