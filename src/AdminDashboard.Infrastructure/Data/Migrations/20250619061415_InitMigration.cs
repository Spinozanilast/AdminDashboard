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
                    { new Guid("03a68bb0-57f3-49c8-885e-aac2ddf74738"), 100m, "john@example.com", "John Doe" },
                    { new Guid("11f651f4-fd29-4418-bb65-e18438b067c7"), 300m, "bob@example.com", "Bob Johnson" },
                    { new Guid("1b433c18-f11a-46fd-a731-52ed46a89916"), 200m, "jane@example.com", "Jane Smith" }
                });

            migrationBuilder.InsertData(
                table: "exchange_rates",
                columns: new[] { "id", "last_updated", "rate" },
                values: new object[] { new Guid("260b90ff-c556-426b-8567-9276bc3e339d"), new DateTime(2025, 6, 19, 6, 14, 14, 287, DateTimeKind.Utc).AddTicks(9403), 10.0m });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "color", "name" },
                values: new object[,]
                {
                    { new Guid("4df49d3e-e801-43d2-933a-f90d79ff51bb"), "#ff0000", "VIP" },
                    { new Guid("b63ecaf9-da5a-43dd-990a-2da858e8704d"), "#00ff00", "Regular" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password_hash" },
                values: new object[] { new Guid("cf1f5dac-5edf-4fa0-bdc9-7e923e3c45c6"), "admin@mirra.dev", "$2a$11$.syTzSU2WWH46KGG.7.trOg631Vq.bAotNfuKfo8B/kgmyffOBfFi" });

            migrationBuilder.InsertData(
                table: "client_tag",
                columns: new[] { "clients_id", "tags_id" },
                values: new object[,]
                {
                    { new Guid("03a68bb0-57f3-49c8-885e-aac2ddf74738"), new Guid("4df49d3e-e801-43d2-933a-f90d79ff51bb") },
                    { new Guid("11f651f4-fd29-4418-bb65-e18438b067c7"), new Guid("b63ecaf9-da5a-43dd-990a-2da858e8704d") },
                    { new Guid("1b433c18-f11a-46fd-a731-52ed46a89916"), new Guid("4df49d3e-e801-43d2-933a-f90d79ff51bb") }
                });

            migrationBuilder.InsertData(
                table: "payments",
                columns: new[] { "id", "amount", "client_id", "date", "description" },
                values: new object[,]
                {
                    { new Guid("88e94f46-3472-4965-90ee-d55d9a5c20f1"), 100m, new Guid("1b433c18-f11a-46fd-a731-52ed46a89916"), new DateTime(2025, 6, 19, 6, 14, 14, 622, DateTimeKind.Utc).AddTicks(2476), "Payment 3" },
                    { new Guid("c0df2376-f828-4cd4-af3d-1f0fb0dcadf1"), 75m, new Guid("03a68bb0-57f3-49c8-885e-aac2ddf74738"), new DateTime(2025, 6, 19, 6, 14, 14, 622, DateTimeKind.Utc).AddTicks(2471), "Payment 2" },
                    { new Guid("d680ccfc-3135-44b7-ad50-4b438f5f4c41"), 150m, new Guid("11f651f4-fd29-4418-bb65-e18438b067c7"), new DateTime(2025, 6, 19, 6, 14, 14, 622, DateTimeKind.Utc).AddTicks(2486), "Payment 5" },
                    { new Guid("e91bfe8e-7d77-4af9-8c14-7567e1bf08f6"), 50m, new Guid("03a68bb0-57f3-49c8-885e-aac2ddf74738"), new DateTime(2025, 6, 19, 6, 14, 14, 622, DateTimeKind.Utc).AddTicks(2227), "Payment 1" },
                    { new Guid("eb11a863-31a4-425d-8501-69c18c791e4c"), 25m, new Guid("11f651f4-fd29-4418-bb65-e18438b067c7"), new DateTime(2025, 6, 19, 6, 14, 14, 622, DateTimeKind.Utc).AddTicks(2481), "Payment 4" }
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
