using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AccountRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_t_Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_AccountRole",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_AccountRole", x => new { x.NIK, x.RoleId });
                    table.ForeignKey(
                        name: "FK_tb_t_AccountRole_tb_t_Account_NIK",
                        column: x => x.NIK,
                        principalTable: "tb_t_Account",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_t_AccountRole_tb_t_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_t_Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_AccountRole_RoleId",
                table: "tb_t_AccountRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_t_AccountRole");

            migrationBuilder.DropTable(
                name: "tb_t_Role");
        }
    }
}
