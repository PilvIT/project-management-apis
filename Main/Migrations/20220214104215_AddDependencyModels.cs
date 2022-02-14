using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Migrations
{
    public partial class AddDependencyModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "GitRepositories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Content = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    GitRepositoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dependencies_GitRepositories_GitRepositoryId",
                        column: x => x.GitRepositoryId,
                        principalTable: "GitRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_GitRepositoryId",
                table: "Dependencies",
                column: "GitRepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_Path_GitRepositoryId",
                table: "Dependencies",
                columns: new[] { "Path", "GitRepositoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories");

            migrationBuilder.DropTable(
                name: "Dependencies");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "GitRepositories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GitRepositories_Projects_ProjectId",
                table: "GitRepositories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
