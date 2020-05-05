namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class muontra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MuonTra", "VanThu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MuonTra", "VanThu");
        }
    }
}
