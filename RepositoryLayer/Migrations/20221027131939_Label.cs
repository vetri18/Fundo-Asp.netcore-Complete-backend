using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class Label : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lableTable",
                columns: table => new
                {
                    LabelID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelName = table.Column<string>(nullable: true),
                    noteID = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lableTable", x => x.LabelID);
                    table.ForeignKey(
                        name: "FK_lableTable_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_lableTable_Notes_noteID",
                        column: x => x.noteID,
                        principalTable: "Notes",
                        principalColumn: "NoteID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lableTable_UserId",
                table: "lableTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_lableTable_noteID",
                table: "lableTable",
                column: "noteID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lableTable");
        }
    }
}
