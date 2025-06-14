using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Quiz_QuizModelid",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuizModelid",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "PointSourceId",
                table: "RewardTransactions");

            migrationBuilder.DropColumn(
                name: "QuizModelid",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "kWValue",
                table: "RecommendRecords",
                newName: "KWValue");

            migrationBuilder.RenameColumn(
                name: "kWValue",
                table: "ConsumptionRecords",
                newName: "KWValue");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "KWValue",
                table: "RecommendRecords",
                newName: "kWValue");

            migrationBuilder.RenameColumn(
                name: "KWValue",
                table: "ConsumptionRecords",
                newName: "kWValue");

            migrationBuilder.AddColumn<int>(
                name: "PointSourceId",
                table: "RewardTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuizModelid",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuizModelid",
                table: "Question",
                column: "QuizModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Quiz_QuizModelid",
                table: "Question",
                column: "QuizModelid",
                principalTable: "Quiz",
                principalColumn: "id");
        }
    }
}
