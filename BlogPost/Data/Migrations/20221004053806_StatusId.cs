using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPost.Data.Migrations
{
    public partial class StatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_StatusId",
                table: "Posts",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Statuses_StatusId",
                table: "Posts",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Statuses_StatusId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Posts_StatusId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Posts");
        }
    }
}
