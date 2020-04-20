namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnOnTableUserAndTaiLieuVanBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.TaiLieuVanBan", "Dang", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaiLieuVanBan", "Dang");
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
