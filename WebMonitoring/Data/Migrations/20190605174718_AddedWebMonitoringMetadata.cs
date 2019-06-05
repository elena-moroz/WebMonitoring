using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMonitoring.Data.Migrations
{
    public partial class AddedWebMonitoringMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastMonitoringDate",
                table: "Websites",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastPublishedArticleCount",
                table: "Websites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WebsiteId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_WebsiteId",
                table: "Articles",
                column: "WebsiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Websites_WebsiteId",
                table: "Articles",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Websites_WebsiteId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_WebsiteId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "LastMonitoringDate",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "LastPublishedArticleCount",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "WebsiteId",
                table: "Articles");
        }
    }
}
