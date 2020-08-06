using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SK.Domain.Enums;

namespace SK.Persistence.Migrations
{
    public partial class AddedAdditionalUserContentToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Kinks",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SexualPreference",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserGender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Voivoideship",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AdditionalUserContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(nullable: true),
                    UserGender = table.Column<int>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Voivoideship = table.Column<int>(nullable: false),
                    SexualPreference = table.Column<int>(nullable: false),
                    Kinks = table.Column<List<Kink>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalUserContents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalUserContents");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<Kink>>(
                name: "Kinks",
                table: "AspNetUsers",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SexualPreference",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserGender",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Voivoideship",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
