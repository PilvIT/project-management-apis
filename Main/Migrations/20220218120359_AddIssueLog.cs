using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Migrations
{
    public partial class AddIssueLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GitRepositories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "GitRepositories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IssueLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DetailLink = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CvssScore = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueLogs_GitRepositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "GitRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VulnerabilityAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PackageName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Ecosystem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescriptionLink = table.Column<string>(type: "text", nullable: false),
                    VulnerableVersions = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PatchedVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ManifestPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CvssScore = table.Column<double>(type: "double precision", nullable: false),
                    IsTransitive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IssueLogId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VulnerabilityAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VulnerabilityAlerts_GitRepositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "GitRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueLogs_RepositoryId",
                table: "IssueLogs",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VulnerabilityAlerts_PackageName_ManifestPath",
                table: "VulnerabilityAlerts",
                columns: new[] { "PackageName", "ManifestPath" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VulnerabilityAlerts_RepositoryId",
                table: "VulnerabilityAlerts",
                column: "RepositoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueLogs");

            migrationBuilder.DropTable(
                name: "VulnerabilityAlerts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GitRepositories");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "GitRepositories");
        }
    }
}
