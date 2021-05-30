using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SK.Persistence.Migrations
{
    public partial class AddedAdditionalInfoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InfoName = table.Column<string>(nullable: true),
                    InfoType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalInfos");
        }
    }
}
