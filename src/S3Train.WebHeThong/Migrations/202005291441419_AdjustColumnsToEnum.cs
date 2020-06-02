namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustColumnsToEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MuonTra", "TinhTrang", c => c.Int());
            AlterColumn("dbo.Hop", "TinhTrang", c => c.Int());
            AlterColumn("dbo.HoSo", "TinhTrang", c => c.Int());
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.Int());
            AlterColumn("dbo.TaiLieuVanBan", "TinhTrang", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaiLieuVanBan", "TinhTrang", c => c.String(maxLength: 150));
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.String(maxLength: 50));
            AlterColumn("dbo.HoSo", "TinhTrang", c => c.String(maxLength: 150));
            AlterColumn("dbo.Hop", "TinhTrang", c => c.String());
            AlterColumn("dbo.MuonTra", "TinhTrang", c => c.String());
        }
    }
}
