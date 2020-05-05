namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CTMuonTra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietMuonTra", "SoLuong", c => c.Int());
            DropColumn("dbo.MuonTra", "SoLuong");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MuonTra", "SoLuong", c => c.Int());
            DropColumn("dbo.ChiTietMuonTra", "SoLuong");
        }
    }
}
