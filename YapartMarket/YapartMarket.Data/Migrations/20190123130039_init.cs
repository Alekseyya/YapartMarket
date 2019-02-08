using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace YapartMarket.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    picture_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_carts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    show = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    shipping_method = table.Column<string>(nullable: true),
                    payment_method = table.Column<string>(nullable: true),
                    client_guid = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    shipped_date = table.Column<DateTime>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    is_sent = table.Column<bool>(nullable: false),
                    is_closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart_lines",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    cart_id = table.Column<int>(nullable: false),
                    article = table.Column<string>(nullable: false),
                    descriptions = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_cart_lines_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    show = table.Column<bool>(nullable: false),
                    sort = table.Column<int>(nullable: false),
                    group_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sections", x => x.id);
                    table.ForeignKey(
                        name: "fk_sections_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    years = table.Column<string>(nullable: true),
                    mark_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_models", x => x.id);
                    table.ForeignKey(
                        name: "fk_models_marks_mark_id",
                        column: x => x.mark_id,
                        principalTable: "marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    order_id = table.Column<int>(nullable: true),
                    articul = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    price_with_discount = table.Column<decimal>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    is_closed = table.Column<bool>(nullable: false),
                    order_status = table.Column<string>(nullable: true),
                    order_status_comment = table.Column<string>(nullable: true),
                    client_guid = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    sort = table.Column<int>(nullable: false),
                    english_name = table.Column<string>(nullable: true),
                    show = table.Column<bool>(nullable: false),
                    section_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_sections_section_id",
                        column: x => x.section_id,
                        principalTable: "sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "modifications",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    model_id = table.Column<int>(nullable: true),
                    years = table.Column<string>(nullable: true),
                    sort = table.Column<int>(nullable: false),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_modifications_models_model_id",
                        column: x => x.model_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    article = table.Column<string>(maxLength: 50, nullable: false),
                    short_article = table.Column<string>(nullable: true),
                    descriptions = table.Column<string>(nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    days_delivery = table.Column<int>(nullable: false),
                    old_price = table.Column<decimal>(nullable: false),
                    popular = table.Column<bool>(nullable: false),
                    characteristic = table.Column<string>(nullable: true),
                    brief = table.Column<string>(nullable: true),
                    show = table.Column<bool>(nullable: false),
                    discount = table.Column<bool>(nullable: false),
                    keywords = table.Column<string>(nullable: true),
                    remove_marketplace = table.Column<bool>(nullable: false),
                    brand_id = table.Column<int>(nullable: true),
                    category_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pictures",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false),
                    path = table.Column<string>(nullable: false),
                    update_timestamp = table.Column<DateTime>(nullable: true),
                    product_id = table.Column<int>(nullable: true),
                    mark_id = table.Column<int>(nullable: true),
                    model_id = table.Column<int>(nullable: true),
                    modification_id = table.Column<int>(nullable: true),
                    brand_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pictures", x => x.id);
                    table.ForeignKey(
                        name: "fk_pictures_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pictures_marks_mark_id",
                        column: x => x.mark_id,
                        principalTable: "marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pictures_models_model_id",
                        column: x => x.model_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pictures_modifications_modification_id",
                        column: x => x.modification_id,
                        principalTable: "modifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pictures_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_modifications",
                columns: table => new
                {
                    product_id = table.Column<int>(nullable: false),
                    modification_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_modifications", x => new { x.product_id, x.modification_id });
                    table.ForeignKey(
                        name: "fk_product_modifications_modifications_modification_id",
                        column: x => x.modification_id,
                        principalTable: "modifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_modifications_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cart_lines_cart_id",
                table: "cart_lines",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_section_id",
                table: "categories",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "ix_models_mark_id",
                table: "models",
                column: "mark_id");

            migrationBuilder.CreateIndex(
                name: "ix_modifications_model_id",
                table: "modifications",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_pictures_brand_id",
                table: "pictures",
                column: "brand_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pictures_mark_id",
                table: "pictures",
                column: "mark_id");

            migrationBuilder.CreateIndex(
                name: "ix_pictures_model_id",
                table: "pictures",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_pictures_modification_id",
                table: "pictures",
                column: "modification_id");

            migrationBuilder.CreateIndex(
                name: "ix_pictures_product_id",
                table: "pictures",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_modifications_modification_id",
                table: "product_modifications",
                column: "modification_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_brand_id",
                table: "products",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_sections_group_id",
                table: "sections",
                column: "group_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_lines");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "pictures");

            migrationBuilder.DropTable(
                name: "product_modifications");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "modifications");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "marks");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "groups");
        }
    }
}
