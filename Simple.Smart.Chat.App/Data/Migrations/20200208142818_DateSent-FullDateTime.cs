using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple.Smart.Chat.App.Data.Migrations
{
    public partial class DateSentFullDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSent",
                table: "ChatMessages",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CONVERT(date, GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSent",
                table: "ChatMessages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CONVERT(date, GETDATE())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
