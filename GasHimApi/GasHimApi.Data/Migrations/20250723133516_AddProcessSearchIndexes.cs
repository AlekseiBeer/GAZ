using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GasHimApi.Data.Migrations
{
    public partial class AddProcessSearchIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Расширение можно вызывать повторно — IF NOT EXISTS
            migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS pg_trgm;");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_name_lower
                                   ON ""Processes"" (LOWER(""Name""));");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_name_trgm
                                   ON ""Processes"" USING gin (LOWER(""Name"") gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_maininputs_trgm
                                   ON ""Processes"" USING gin (LOWER(""MainInputs"") gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_addinputs_trgm
                                   ON ""Processes"" USING gin (LOWER(""AdditionalInputs"") gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_mainoutputs_trgm
                                   ON ""Processes"" USING gin (LOWER(""MainOutputs"") gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX ix_processes_addoutputs_trgm
                                   ON ""Processes"" USING gin (LOWER(""AdditionalOutputs"") gin_trgm_ops);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_addoutputs_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_mainoutputs_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_addinputs_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_maininputs_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_name_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_processes_name_lower;");
            // EXTENSION обычно не трогаем
            // migrationBuilder.Sql(@"DROP EXTENSION IF EXISTS pg_trgm;");
        }
    }
}