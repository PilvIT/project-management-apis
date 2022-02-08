using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Migrations
{
    public partial class ImproveProjectNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "GitRepositories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GitRepositories_ProjectId",
                table: "GitRepositories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories");

            migrationBuilder.DropIndex(
                name: "IX_GitRepositories_ProjectId",
                table: "GitRepositories");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "GitRepositories");
        }
    }
}
