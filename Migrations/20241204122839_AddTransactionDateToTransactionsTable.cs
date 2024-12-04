﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expensify.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionDateToTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "TransactionDate",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Transactions");
        }
    }
}
