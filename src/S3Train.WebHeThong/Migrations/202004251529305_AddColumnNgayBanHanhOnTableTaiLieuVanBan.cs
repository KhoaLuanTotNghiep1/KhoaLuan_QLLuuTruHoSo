namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnNgayBanHanhOnTableTaiLieuVanBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaiLieuVanBan", "NgayBanHanh", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaiLieuVanBan", "NgayBanHanh");
        }
    }
}
