using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntitys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_tenants_users_CreateUserId",
            //    table: "tenants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_tenants_users_UpdateUserId",
            //    table: "tenants");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_users_CreateUserId",
            //    table: "users");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_users_users_UpdateUserId",
            //    table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_UpdateUserId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_tenants_UpdateUserId",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "tenants");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "users",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreateUserId",
                table: "users",
                newName: "LastModifiedId");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_users_CreateUserId",
                table: "users",
                newName: "IX_users_LastModifiedId");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "tenants",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreateUserId",
                table: "tenants",
                newName: "LastModifiedId");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "tenants",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_tenants_CreateUserId",
                table: "tenants",
                newName: "IX_tenants_LastModifiedId");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedId",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedId",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedId",
                table: "tenants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tenants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedId",
                table: "tenants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_users_CreatedId",
                table: "users",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_users_DeletedId",
                table: "users",
                column: "DeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_tenants_CreatedId",
                table: "tenants",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_tenants_DeletedId",
                table: "tenants",
                column: "DeletedId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tenants_users_CreatedId",
            //    table: "tenants",
            //    column: "CreatedId",
            //    principalTable: "users",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tenants_users_DeletedId",
            //    table: "tenants",
            //    column: "DeletedId",
            //    principalTable: "users",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tenants_users_LastModifiedId",
            //    table: "tenants",
            //    column: "LastModifiedId",
            //    principalTable: "users",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_users_CreatedId",
            //    table: "users",
            //    column: "CreatedId",
            //    principalTable: "users",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_users_DeletedId",
            //    table: "users",
            //    column: "DeletedId",
            //    principalTable: "users",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_users_users_LastModifiedId",
            //    table: "users",
            //    column: "LastModifiedId",
            //    principalTable: "users",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tenants_users_CreatedId",
                table: "tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_tenants_users_DeletedId",
                table: "tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_tenants_users_LastModifiedId",
                table: "tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_CreatedId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_DeletedId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_LastModifiedId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_CreatedId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_DeletedId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_tenants_CreatedId",
                table: "tenants");

            migrationBuilder.DropIndex(
                name: "IX_tenants_DeletedId",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "CreatedId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CreatedId",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "tenants");

            migrationBuilder.RenameColumn(
                name: "LastModifiedId",
                table: "users",
                newName: "CreateUserId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "users",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "CreateAt");

            migrationBuilder.RenameIndex(
                name: "IX_users_LastModifiedId",
                table: "users",
                newName: "IX_users_CreateUserId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedId",
                table: "tenants",
                newName: "CreateUserId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "tenants",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tenants",
                newName: "CreateAt");

            migrationBuilder.RenameIndex(
                name: "IX_tenants_LastModifiedId",
                table: "tenants",
                newName: "IX_tenants_CreateUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateUserId",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateUserId",
                table: "tenants",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_UpdateUserId",
                table: "users",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tenants_UpdateUserId",
                table: "tenants",
                column: "UpdateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tenants_users_CreateUserId",
                table: "tenants",
                column: "CreateUserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tenants_users_UpdateUserId",
                table: "tenants",
                column: "UpdateUserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_CreateUserId",
                table: "users",
                column: "CreateUserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_UpdateUserId",
                table: "users",
                column: "UpdateUserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
