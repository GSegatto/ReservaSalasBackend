using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaSalas.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDatasAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "sala",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataModificacao",
                table: "sala",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "sala");

            migrationBuilder.DropColumn(
                name: "DataModificacao",
                table: "sala");
        }
    }
}
