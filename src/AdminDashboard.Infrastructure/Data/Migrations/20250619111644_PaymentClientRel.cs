using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdminDashboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentClientRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"), new Guid("60646b56-eac8-4297-8b88-949a6f5f684c") });

            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("57dc4274-2d41-4e65-b11a-4a2c623bb812"), new Guid("1483cc99-29ad-42f4-9528-1df013131c65") });

            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"), new Guid("1483cc99-29ad-42f4-9528-1df013131c65") });

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: new Guid("ad087526-b475-4a42-86c0-be2766964078"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("49d13a3b-6192-43ca-b28c-61b5d7394062"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("60f8806a-b080-4895-a587-3505112e8993"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("9a68f8ef-faae-4d12-b05a-e297721682c9"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("b28090be-0fc5-4c96-80c6-fe182eb2ca79"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("ec3b9d7f-03c9-406d-bb74-bfd24702bd4d"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f8a1d99f-5b86-4aad-9eae-8ca0244682ff"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("3b30c2c0-294b-4d96-8026-5cdc0403e139"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("57dc4274-2d41-4e65-b11a-4a2c623bb812"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("a987db56-f99f-47f5-93a8-1ba71889df80"));

            migrationBuilder.DeleteData(
                table: "tags",
                keyColumn: "id",
                keyValue: new Guid("1483cc99-29ad-42f4-9528-1df013131c65"));

            migrationBuilder.DeleteData(
                table: "tags",
                keyColumn: "id",
                keyValue: new Guid("60646b56-eac8-4297-8b88-949a6f5f684c"));

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id", "balance_t", "email", "name" },
                values: new object[,]
                {
                    { new Guid("63ead4d6-88f1-48ea-b0cb-2843fabf87b4"), 200m, "jane@example.com", "Jane Smith" },
                    { new Guid("95a88551-7a97-47c3-8baa-d97623823869"), 300m, "bob@example.com", "Bob Johnson" },
                    { new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"), 100m, "john@example.com", "John Doe" }
                });

            migrationBuilder.InsertData(
                table: "exchange_rates",
                columns: new[] { "id", "last_updated", "rate" },
                values: new object[] { new Guid("05acc9b9-4e0a-4795-ba92-f4621a11639e"), new DateTime(2025, 6, 19, 11, 16, 43, 351, DateTimeKind.Utc).AddTicks(235), 10.0m });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "color", "name" },
                values: new object[,]
                {
                    { new Guid("0631783e-41a9-40eb-bf33-43e78acd9632"), "#00ff00", "Regular" },
                    { new Guid("c8c90070-486b-482b-9e1d-89639f0d8533"), "#ff0000", "VIP" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password_hash" },
                values: new object[] { new Guid("6732a5fd-4a21-4d0b-9d52-9480887277c4"), "admin@mirra.dev", "$2a$11$OxMVjuCygRNxm7FFMpzSEe6/6yQkA2aJaM9Zfg32yhm8RE/limjYy" });

            migrationBuilder.InsertData(
                table: "client_tag",
                columns: new[] { "clients_id", "tags_id" },
                values: new object[,]
                {
                    { new Guid("63ead4d6-88f1-48ea-b0cb-2843fabf87b4"), new Guid("c8c90070-486b-482b-9e1d-89639f0d8533") },
                    { new Guid("95a88551-7a97-47c3-8baa-d97623823869"), new Guid("0631783e-41a9-40eb-bf33-43e78acd9632") },
                    { new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"), new Guid("c8c90070-486b-482b-9e1d-89639f0d8533") }
                });

            migrationBuilder.InsertData(
                table: "payments",
                columns: new[] { "id", "amount", "client_id", "date", "description" },
                values: new object[,]
                {
                    { new Guid("225af31e-46bd-4dfe-b5fb-12f31095e960"), 50m, new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"), new DateTime(2025, 6, 19, 11, 16, 43, 566, DateTimeKind.Utc).AddTicks(5568), "Payment 1" },
                    { new Guid("53fe1933-b4b3-41c7-9d25-51b0fcc755cf"), 100m, new Guid("63ead4d6-88f1-48ea-b0cb-2843fabf87b4"), new DateTime(2025, 6, 19, 11, 16, 43, 566, DateTimeKind.Utc).AddTicks(5812), "Payment 3" },
                    { new Guid("7389534e-2ae9-4e78-8ae1-034c3eebe944"), 150m, new Guid("95a88551-7a97-47c3-8baa-d97623823869"), new DateTime(2025, 6, 19, 11, 16, 43, 566, DateTimeKind.Utc).AddTicks(5822), "Payment 5" },
                    { new Guid("dd4684dc-7ab1-4cfa-b61e-9108f57ce7be"), 25m, new Guid("95a88551-7a97-47c3-8baa-d97623823869"), new DateTime(2025, 6, 19, 11, 16, 43, 566, DateTimeKind.Utc).AddTicks(5817), "Payment 4" },
                    { new Guid("f34b52e9-502c-4d5a-ae49-982eb309044a"), 75m, new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"), new DateTime(2025, 6, 19, 11, 16, 43, 566, DateTimeKind.Utc).AddTicks(5807), "Payment 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("63ead4d6-88f1-48ea-b0cb-2843fabf87b4"), new Guid("c8c90070-486b-482b-9e1d-89639f0d8533") });

            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("95a88551-7a97-47c3-8baa-d97623823869"), new Guid("0631783e-41a9-40eb-bf33-43e78acd9632") });

            migrationBuilder.DeleteData(
                table: "client_tag",
                keyColumns: new[] { "clients_id", "tags_id" },
                keyValues: new object[] { new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"), new Guid("c8c90070-486b-482b-9e1d-89639f0d8533") });

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: new Guid("05acc9b9-4e0a-4795-ba92-f4621a11639e"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("225af31e-46bd-4dfe-b5fb-12f31095e960"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("53fe1933-b4b3-41c7-9d25-51b0fcc755cf"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("7389534e-2ae9-4e78-8ae1-034c3eebe944"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("dd4684dc-7ab1-4cfa-b61e-9108f57ce7be"));

            migrationBuilder.DeleteData(
                table: "payments",
                keyColumn: "id",
                keyValue: new Guid("f34b52e9-502c-4d5a-ae49-982eb309044a"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("6732a5fd-4a21-4d0b-9d52-9480887277c4"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("63ead4d6-88f1-48ea-b0cb-2843fabf87b4"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("95a88551-7a97-47c3-8baa-d97623823869"));

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "id",
                keyValue: new Guid("ffc8296f-886c-4d18-a245-4ab0cf6c2b28"));

            migrationBuilder.DeleteData(
                table: "tags",
                keyColumn: "id",
                keyValue: new Guid("0631783e-41a9-40eb-bf33-43e78acd9632"));

            migrationBuilder.DeleteData(
                table: "tags",
                keyColumn: "id",
                keyValue: new Guid("c8c90070-486b-482b-9e1d-89639f0d8533"));

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
        }
    }
}
