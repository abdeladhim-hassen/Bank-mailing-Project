using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgenceId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1001,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 5, 25, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3936), new DateTime(2027, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3938) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1002,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 4, 20, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3946), new DateTime(2028, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3947) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1003,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 7, 29, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3951), new DateTime(2026, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3952) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1004,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 11, 6, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(4047), new DateTime(2027, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(4048) });

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3763));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3768));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3771));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3773));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3776));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 10,
                column: "LastVerificationDate",
                value: new DateTime(2024, 5, 10, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3239));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AgenceId", "DateCreated", "PasswordHash", "PasswordSalt" },
                values: new object[] { 23, new DateTime(2024, 5, 24, 15, 51, 44, 878, DateTimeKind.Local).AddTicks(3635), new byte[] { 149, 224, 181, 24, 49, 89, 201, 81, 16, 217, 204, 141, 40, 45, 43, 48, 119, 49, 143, 33, 3, 195, 208, 45, 157, 229, 137, 110, 186, 8, 117, 245, 205, 124, 11, 191, 115, 86, 36, 73, 204, 207, 180, 222, 234, 47, 186, 52, 224, 171, 243, 67, 123, 84, 21, 131, 122, 25, 219, 239, 233, 174, 31, 20 }, new byte[] { 61, 167, 213, 208, 242, 197, 185, 165, 95, 254, 208, 217, 192, 34, 32, 196, 76, 66, 230, 130, 58, 122, 50, 103, 104, 35, 32, 188, 183, 197, 2, 171, 136, 12, 176, 26, 152, 85, 56, 38, 199, 238, 91, 22, 226, 224, 232, 172, 98, 66, 95, 219, 23, 252, 6, 62, 194, 98, 120, 30, 242, 171, 221, 162, 42, 3, 165, 42, 155, 214, 129, 229, 104, 193, 72, 93, 128, 227, 244, 215, 16, 132, 237, 191, 86, 246, 29, 16, 142, 98, 194, 21, 199, 161, 229, 124, 29, 67, 124, 100, 27, 11, 200, 219, 20, 84, 250, 228, 31, 215, 120, 87, 220, 205, 138, 46, 29, 131, 20, 2, 106, 235, 88, 64, 85, 23, 202, 79 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AgenceId",
                table: "Users",
                column: "AgenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Agences_AgenceId",
                table: "Users",
                column: "AgenceId",
                principalTable: "Agences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Agences_AgenceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AgenceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AgenceId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1001,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 5, 24, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4649), new DateTime(2027, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4651) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1002,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 4, 19, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4659), new DateTime(2028, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4660) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1003,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 7, 28, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4665), new DateTime(2026, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4666) });

            migrationBuilder.UpdateData(
                table: "Cartes",
                keyColumn: "NumeroCarte",
                keyValue: 1004,
                columns: new[] { "DateDelivrance", "DateExpiration" },
                values: new object[] { new DateTime(2023, 11, 5, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4669), new DateTime(2027, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4670) });

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4439));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4443));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4445));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4488));

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateOuvertureCompte",
                value: new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4491));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Credits",
                keyColumn: "Id",
                keyValue: 10,
                column: "LastVerificationDate",
                value: new DateTime(2024, 4, 27, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 5, 23, 15, 25, 20, 518, DateTimeKind.Local).AddTicks(4036), new byte[] { 118, 33, 137, 242, 23, 94, 164, 228, 172, 117, 190, 126, 152, 119, 218, 188, 82, 1, 177, 235, 220, 85, 239, 8, 15, 238, 164, 95, 178, 164, 202, 218, 97, 40, 3, 208, 253, 109, 116, 233, 209, 239, 248, 147, 7, 112, 10, 28, 53, 98, 172, 86, 74, 191, 143, 167, 69, 165, 233, 181, 118, 1, 253, 74 }, new byte[] { 254, 139, 147, 160, 139, 190, 130, 56, 53, 95, 118, 219, 193, 175, 35, 181, 174, 139, 228, 58, 189, 112, 32, 54, 32, 83, 94, 152, 140, 85, 120, 86, 190, 37, 26, 185, 196, 110, 166, 120, 60, 246, 218, 68, 132, 5, 10, 246, 179, 145, 146, 146, 108, 208, 82, 130, 102, 97, 74, 92, 124, 132, 48, 104, 167, 232, 213, 39, 86, 177, 214, 95, 229, 199, 192, 197, 76, 214, 202, 134, 232, 97, 147, 114, 172, 60, 191, 142, 177, 185, 78, 66, 141, 32, 177, 208, 173, 108, 62, 137, 79, 165, 159, 196, 123, 147, 234, 48, 12, 251, 61, 78, 238, 75, 108, 77, 60, 191, 247, 218, 121, 181, 176, 172, 114, 197, 145, 98 } });
        }
    }
}
