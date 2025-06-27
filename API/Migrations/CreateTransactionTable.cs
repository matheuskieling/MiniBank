using FluentMigrator;

namespace API.Migrations;

[Migration(2)]
public class CreateTransactionTable : Migration
{
    public override void Up()
    {
        //Execute sql script
        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS transactions (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                sender_id UUID NOT NULL REFERENCES wallets(id) ON DELETE CASCADE,
                receiver_id UUID NOT NULL REFERENCES wallets(id) ON DELETE CASCADE,
                amount BIGINT NOT NULL,
                created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE transactions");
    }
}