using FluentMigrator;

namespace Identity.Migrations;

[Migration(1)]
public class CreateUserTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE users (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                username TEXT NOT NULL UNIQUE,
                hash TEXT NOT NULL,
                salt TEXT NOT NULL,
                created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
               
            )
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE users");
    }
}