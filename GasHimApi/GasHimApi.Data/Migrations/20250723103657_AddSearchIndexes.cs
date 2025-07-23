using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GasHimApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS pg_trgm;");

            migrationBuilder.Sql(@"CREATE INDEX ix_substances_name_lower
                           ON ""Substances"" (LOWER(""Name""));");

            migrationBuilder.Sql(@"CREATE INDEX ix_substances_name_trgm
                           ON ""Substances"" USING gin (LOWER(""Name"") gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX ix_substances_synonyms_trgm
                           ON ""Substances"" USING gin (LOWER(""Synonyms"") gin_trgm_ops);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_substances_synonyms_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_substances_name_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_substances_name_lower;");
            // migrationBuilder.Sql(@"DROP EXTENSION IF EXISTS pg_trgm;"); // опционально
        }
    }
}
