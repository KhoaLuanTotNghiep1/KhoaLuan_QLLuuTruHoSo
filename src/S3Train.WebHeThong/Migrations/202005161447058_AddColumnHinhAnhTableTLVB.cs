namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnHinhAnhTableTLVB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaiLieuVanBan", "HinhAnh", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaiLieuVanBan", "HinhAnh");
        }
    }
}
