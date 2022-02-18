using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Migrations
{
    public partial class ImproveIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VulnerabilityAlerts_PackageName_ManifestPath",
                table: "VulnerabilityAlerts");

            migrationBuilder.CreateIndex(
                name: "IX_VulnerabilityAlerts_PackageName_ManifestPath_DescriptionLink",
                table: "VulnerabilityAlerts",
                columns: new[] { "PackageName", "ManifestPath", "DescriptionLink" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VulnerabilityAlerts_PackageName_ManifestPath_DescriptionLink",
                table: "VulnerabilityAlerts");

            migrationBuilder.CreateIndex(
                name: "IX_VulnerabilityAlerts_PackageName_ManifestPath",
                table: "VulnerabilityAlerts",
                columns: new[] { "PackageName", "ManifestPath" },
                unique: true);
        }
    }
}
