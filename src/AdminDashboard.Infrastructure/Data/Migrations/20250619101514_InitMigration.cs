using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdminDashboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    balance_t = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exchange_rates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exchange_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_tag",
                columns: table => new
                {
                    clients_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_tag", x => new { x.clients_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_client_tag_clients_clients_id",
                        column: x => x.clients_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_tag_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id", "balance_t", "email", "name" },
                values: new object[,]
                {
                    { new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"), 300m, "bob@example.com", "Bob Johnson" },
                    { new Guid("57dc4274-2d41-4e65-b11a-4a2c623bb812"), 200m, "jane@example.com", "Jane Smith" },
                    { new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"), 100m, "john@example.com", "John Doe" }
                });

            migrationBuilder.InsertData(
                table: "exchange_rates",
                columns: new[] { "id", "last_updated", "rate" },
                values: new object[] { new Guid("ad087526-b475-4a42-86c0-be2766964078"), new DateTime(2025, 6, 19, 10, 15, 14, 44, DateTimeKind.Utc).AddTicks(5620), 10.0m });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "color", "name" },
                values: new object[,]
                {
                    { new Guid("1483cc99-29ad-42f4-9528-1df013131c65"), "#ff0000", "VIP" },
                    { new Guid("60646b56-eac8-4297-8b88-949a6f5f684c"), "#00ff00", "Regular" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password_hash" },
                values: new object[] { new Guid("f8a1d99f-5b86-4aad-9eae-8ca0244682ff"), "admin@mirra.dev", "$2a$11$S/dVHGgcEwp9ChhFaxbjgOc15VKnLsxa/Qzbm40oOfSimHINBI9aC" });

            migrationBuilder.InsertData(
                table: "client_tag",
                columns: new[] { "clients_id", "tags_id" },
                values: new object[,]
                {
                    { new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"), new Guid("60646b56-eac8-4297-8b88-949a6f5f684c") },
                    { new Guid("57dc4274-2d41-4e65-b11a-4a2c623bb812"), new Guid("1483cc99-29ad-42f4-9528-1df013131c65") },
                    { new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"), new Guid("1483cc99-29ad-42f4-9528-1df013131c65") }
                });

            migrationBuilder.InsertData(
                table: "payments",
                columns: new[] { "id", "amount", "client_id", "date", "description" },
                values: new object[,]
                {
                    { new Guid("49d13a3b-6192-43ca-b28c-61b5d7394062"), 25m, new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"), new DateTime(2025, 6, 19, 10, 15, 14, 311, DateTimeKind.Utc).AddTicks(5114), "Payment 4" },
                    { new Guid("60f8806a-b080-4895-a587-3505112e8993"), 75m, new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"), new DateTime(2025, 6, 19, 10, 15, 14, 311, DateTimeKind.Utc).AddTicks(5105), "Payment 2" },
                    { new Guid("9a68f8ef-faae-4d12-b05a-e297721682c9"), 100m, new Guid("57dc4274-2d41-4e65-b11a-4a2c623bb812"), new DateTime(2025, 6, 19, 10, 15, 14, 311, DateTimeKind.Utc).AddTicks(5110), "Payment 3" },
                    { new Guid("b28090be-0fc5-4c96-80c6-fe182eb2ca79"), 50m, new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"), new DateTime(2025, 6, 19, 10, 15, 14, 311, DateTimeKind.Utc).AddTicks(4865), "Payment 1" },
                    { new Guid("ec3b9d7f-03c9-406d-bb74-bfd24702bd4d"), 150m, new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"), new DateTime(2025, 6, 19, 10, 15, 14, 311, DateTimeKind.Utc).AddTicks(5119), "Payment 5" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_client_tag_tags_id",
                table: "client_tag",
                column: "tags_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_email",
                table: "clients",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payments_client_id",
                table: "payments",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_tag");

            migrationBuilder.DropTable(
                name: "exchange_rates");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
