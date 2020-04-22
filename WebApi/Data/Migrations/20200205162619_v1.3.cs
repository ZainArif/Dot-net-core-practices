using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_RoleId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleId",
                table: "User");

            //migrationBuilder.DropColumn(
            //    name: "RolesRoleId",
            //    table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolesRoleId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RolesRoleId",
                table: "User",
                column: "RolesRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_RolesRoleId",
                table: "User",
                column: "RolesRoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
