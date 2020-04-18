namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKhoToTu : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Kho", newName: "Tu");
            RenameColumn(table: "dbo.Ke", name: "KhoId", newName: "Tuid");
            RenameIndex(table: "dbo.Ke", name: "IX_KhoId", newName: "IX_Tuid");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Ke", name: "IX_Tuid", newName: "IX_KhoId");
            RenameColumn(table: "dbo.Ke", name: "Tuid", newName: "KhoId");
            RenameTable(name: "dbo.Tu", newName: "Kho");
        }
    }
}
