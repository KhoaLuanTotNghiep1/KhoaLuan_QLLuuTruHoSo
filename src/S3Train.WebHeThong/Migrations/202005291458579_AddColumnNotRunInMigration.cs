namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnNotRunInMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaiLieuVanBan", "HinhAnh", c => c.String());

            AddColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ChiTietMuonTra", "TaiLieuVanBanId");
            AddForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan", "Id");

            DropColumn("dbo.TaiLieuVanBan", "TrichYeu");
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaiLieuVanBan", "HinhAnh");
            DropForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan");
            DropIndex("dbo.ChiTietMuonTra", new[] { "TaiLieuVanBanId" });
            DropColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId");

            DropColumn("dbo.TaiLieuVanBan", "TrichYeu");
        }
    }
}
