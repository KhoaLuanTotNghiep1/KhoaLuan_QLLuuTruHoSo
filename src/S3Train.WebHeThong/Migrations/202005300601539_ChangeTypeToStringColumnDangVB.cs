namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeToStringColumnDangVB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaiLieuVanBan", "Dang", c => c.Int());
        }
    }
}
