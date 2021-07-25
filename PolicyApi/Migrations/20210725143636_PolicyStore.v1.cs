using Microsoft.EntityFrameworkCore.Migrations;

namespace PolicyApi.Migrations
{
    public partial class PolicyStorev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PERMISSION_CATEGORIES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISPLAY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POSITION = table.Column<int>(type: "int", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSION_CATEGORIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POSITION = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSIONS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISPLAY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERMISSION_CATEDGORY_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    POSITION = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERMISSIONS_PERMISSION_CATEGORIES_PERMISSION_CATEDGORY_ID",
                        column: x => x.PERMISSION_CATEDGORY_ID,
                        principalTable: "PERMISSION_CATEGORIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROFILE_ROLES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROFILE_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFILE_ROLES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PROFILE_ROLES_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROFILE_ROLE_PERMISSIONS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROFILE_ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PERMISSION_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFILE_ROLE_PERMISSIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PROFILE_ROLE_PERMISSIONS_PERMISSIONS_PERMISSION_ID",
                        column: x => x.PERMISSION_ID,
                        principalTable: "PERMISSIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROFILE_ROLE_PERMISSIONS_PROFILE_ROLES_PROFILE_ROLE_ID",
                        column: x => x.PROFILE_ROLE_ID,
                        principalTable: "PROFILE_ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSIONS_PERMISSION_CATEDGORY_ID",
                table: "PERMISSIONS",
                column: "PERMISSION_CATEDGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROFILE_ROLE_PERMISSIONS_PERMISSION_ID",
                table: "PROFILE_ROLE_PERMISSIONS",
                column: "PERMISSION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROFILE_ROLE_PERMISSIONS_PROFILE_ROLE_ID",
                table: "PROFILE_ROLE_PERMISSIONS",
                column: "PROFILE_ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROFILE_ROLES_ROLE_ID",
                table: "PROFILE_ROLES",
                column: "ROLE_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROFILE_ROLE_PERMISSIONS");

            migrationBuilder.DropTable(
                name: "PERMISSIONS");

            migrationBuilder.DropTable(
                name: "PROFILE_ROLES");

            migrationBuilder.DropTable(
                name: "PERMISSION_CATEGORIES");

            migrationBuilder.DropTable(
                name: "ROLES");
        }
    }
}
