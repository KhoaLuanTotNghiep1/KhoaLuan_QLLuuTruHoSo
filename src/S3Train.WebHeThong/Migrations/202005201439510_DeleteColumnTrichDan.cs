namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteColumnTrichDan : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TaiLieuVanBan", "TrichYeu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaiLieuVanBan", "TrichYeu", c => c.String());
        }
    }
}
