using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CTA.Migrations
{
    public partial class AddedImagefieldtoUsermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Lots",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Lots",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Lots");
        }
    }
}
