using FluentMigrator;

namespace API.Migrations;

[Migration(1)]
public class CreateTables : Migration
{
    public override void Up()
    {
        //Execute sql script
        Execute.Sql(@"
            CREATE TABLE wallets (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                user_id UUID NOT NULL REFERENCES identity.users(id) ON DELETE CASCADE,
                balance BIGINT NOT NULL DEFAULT 0,
                created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE wallet");
    }
}