namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustTableTaiLieuVanBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoaiHoSo", "Ma", c => c.String(maxLength: 50));
            AddColumn("dbo.TaiLieuVanBan", "SoKyHieu", c => c.String(maxLength: 100));
            AddColumn("dbo.TaiLieuVanBan", "NoiDung", c => c.String());
            AddColumn("dbo.TaiLieuVanBan", "TrichYeu", c => c.String());
            AddColumn("dbo.TaiLieuVanBan", "NoiNhan", c => c.String(maxLength: 200));
            AddColumn("dbo.TaiLieuVanBan", "NguoiGuiHoacNhan", c => c.String(maxLength: 100));
            AddColumn("dbo.TaiLieuVanBan", "NguoiKy", c => c.String(maxLength: 100));
            AddColumn("dbo.TaiLieuVanBan", "NguoiDuyet", c => c.String(maxLength: 100));
            DropColumn("dbo.TaiLieuVanBan", "So");
            DropColumn("dbo.TaiLieuVanBan", "KhoGiay");
            DropColumn("dbo.TaiLieuVanBan", "TacGia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaiLieuVanBan", "TacGia", c => c.String(maxLength: 300));
            AddColumn("dbo.TaiLieuVanBan", "KhoGiay", c => c.String(maxLength: 300));
            AddColumn("dbo.TaiLieuVanBan", "So", c => c.String(maxLength: 300));
            DropColumn("dbo.TaiLieuVanBan", "NguoiDuyet");
            DropColumn("dbo.TaiLieuVanBan", "NguoiKy");
            DropColumn("dbo.TaiLieuVanBan", "NguoiGuiHoacNhan");
            DropColumn("dbo.TaiLieuVanBan", "NoiNhan");
            DropColumn("dbo.TaiLieuVanBan", "TrichYeu");
            DropColumn("dbo.TaiLieuVanBan", "NoiDung");
            DropColumn("dbo.TaiLieuVanBan", "SoKyHieu");
            DropColumn("dbo.LoaiHoSo", "Ma");
        }
    }
}
