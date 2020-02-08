using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple.Smart.Chat.App.Data.Migrations
{
    public partial class DateSentMessageDefaultSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSent",
                table: "ChatMessages",
                nullable: false,
                defaultValueSql: "CONVERT(date, GETDATE())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSent",
                table: "ChatMessages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "CONVERT(date, GETDATE())");
        }
    }
}
