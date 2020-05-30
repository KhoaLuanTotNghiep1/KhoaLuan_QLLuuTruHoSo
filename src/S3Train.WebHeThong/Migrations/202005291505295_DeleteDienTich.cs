namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDienTich : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tu", "DienTich");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tu", "DienTich", c => c.String(maxLength: 100));
        }
    }
}
