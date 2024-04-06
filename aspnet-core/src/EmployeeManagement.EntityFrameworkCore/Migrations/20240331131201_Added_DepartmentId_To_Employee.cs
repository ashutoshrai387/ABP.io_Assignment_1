﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Added_DepartmentId_To_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "AppEmployees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployees_DepartmentId",
                table: "AppEmployees",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployees_AppDepartments_DepartmentId",
                table: "AppEmployees",
                column: "DepartmentId",
                principalTable: "AppDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployees_AppDepartments_DepartmentId",
                table: "AppEmployees");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployees_DepartmentId",
                table: "AppEmployees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AppEmployees");
        }
    }
}
