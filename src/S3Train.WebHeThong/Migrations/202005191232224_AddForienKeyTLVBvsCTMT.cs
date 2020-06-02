namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForienKeyTLVBvsCTMT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ChiTietMuonTra", "TaiLieuVanBanId");
            AddForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan");
            DropIndex("dbo.ChiTietMuonTra", new[] { "TaiLieuVanBanId" });
            DropColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId");
        }
    }
}
