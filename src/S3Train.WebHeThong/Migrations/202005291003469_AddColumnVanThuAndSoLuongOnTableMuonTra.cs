namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnVanThuAndSoLuongOnTableMuonTra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MuonTra", "VanThu", c => c.String());
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.String());
            DropColumn("dbo.MuonTra", "VanThu");
        }
    }
}
