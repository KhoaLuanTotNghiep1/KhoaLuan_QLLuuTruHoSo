namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId", c => c.String(maxLength: 128));
            AddColumn("dbo.ChiTietMuonTra", "SoLuong", c => c.Int());
            AddColumn("dbo.MuonTra", "VanThu", c => c.String());
            CreateIndex("dbo.ChiTietMuonTra", "TaiLieuVanBanId");
            AddForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan", "Id");
            //DropColumn("dbo.ChiTietMuonTra", "ThuMuon");
            //DropColumn("dbo.MuonTra", "SoLuong");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MuonTra", "SoLuong", c => c.Int());
            AddColumn("dbo.ChiTietMuonTra", "ThuMuon", c => c.String());
            DropForeignKey("dbo.ChiTietMuonTra", "TaiLieuVanBanId", "dbo.TaiLieuVanBan");
            DropIndex("dbo.ChiTietMuonTra", new[] { "TaiLieuVanBanId" });
            DropColumn("dbo.MuonTra", "VanThu");
            DropColumn("dbo.ChiTietMuonTra", "SoLuong");
            DropColumn("dbo.ChiTietMuonTra", "TaiLieuVanBanId");
        }
    }
}
