namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteColumnHoSoChaIdOnTableHoSo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.HoSo", name: "HoSoChaId", newName: "TapHoSoId");
            RenameIndex(table: "dbo.HoSo", name: "IX_HoSoChaId", newName: "IX_TapHoSoId");
            DropColumn("dbo.HoSo", "TapHoSo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoSo", "TapHoSo", c => c.String(nullable: false, maxLength: 300));
            RenameIndex(table: "dbo.HoSo", name: "IX_TapHoSoId", newName: "IX_HoSoChaId");
            RenameColumn(table: "dbo.HoSo", name: "TapHoSoId", newName: "HoSoChaId");
        }
    }
}
