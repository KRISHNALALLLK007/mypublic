using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Demo.DataService.Migrations
{
    public partial class AuditLogImpl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(maxLength: 100, nullable: true),
                    EventType = table.Column<string>(maxLength: 100, nullable: true),
                    PrimaryKeyNames = table.Column<string>(maxLength: 100, nullable: true),
                    PrimaryKeyValues = table.Column<string>(maxLength: 100, nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    OriginalValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(maxLength: 100, nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");
        }
    }
}
