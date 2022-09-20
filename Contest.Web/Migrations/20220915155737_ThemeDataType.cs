using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contest.Web.Migrations
{
    public partial class ThemeDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "569e325b-05a9-438f-a1e9-d3e4ef5bc1a7");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6f3bed06-d76b-4761-b715-c345cd5cc580");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bcaba235-ff8e-48eb-85f4-55d5dd4c6c22");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "84d975fd-1a36-4c19-a44b-6deeac520b21", "ad674d6e-9a7d-4611-919e-7105e95d1196", "Participant", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5e76e56-73e5-4ad6-aea8-c384057f6a74", "a49832bf-0954-4ca4-92f9-28c2f052ddeb", "Admin", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb2441ed-8517-49ed-ad2e-d8c4df135591", "7a5ff694-a2b1-4e16-aaf6-4279d833f557", "SuperAdmin", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "84d975fd-1a36-4c19-a44b-6deeac520b21");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e5e76e56-73e5-4ad6-aea8-c384057f6a74");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "fb2441ed-8517-49ed-ad2e-d8c4df135591");

            migrationBuilder.AlterColumn<int>(
                name: "Theme",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "569e325b-05a9-438f-a1e9-d3e4ef5bc1a7", "7449215f-0e89-44c0-bb4a-4d66d7772aa5", "Participant", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6f3bed06-d76b-4761-b715-c345cd5cc580", "8743d87b-5831-4bac-8ad7-270d1f11c034", "Admin", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bcaba235-ff8e-48eb-85f4-55d5dd4c6c22", "5fe82bd5-02da-4868-896a-f22c08d04c54", "SuperAdmin", null });
        }
    }
}
