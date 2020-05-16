namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustToOldVersion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaiLieuVanBan", "DangVanBanTaiLieuId", "dbo.DangTaiLieuVanBan");
            DropIndex("dbo.TaiLieuVanBan", new[] { "DangVanBanTaiLieuId" });
            AddColumn("dbo.TaiLieuVanBan", "Dang", c => c.String());
            DropColumn("dbo.TaiLieuVanBan", "DangVanBanTaiLieuId");
            DropTable("dbo.DangTaiLieuVanBan");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DangTaiLieuVanBan",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 50),
                        MoTa = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaiLieuVanBan", "DangVanBanTaiLieuId", c => c.String(maxLength: 128));
            DropColumn("dbo.TaiLieuVanBan", "Dang");
            CreateIndex("dbo.TaiLieuVanBan", "DangVanBanTaiLieuId");
            AddForeignKey("dbo.TaiLieuVanBan", "DangVanBanTaiLieuId", "dbo.DangTaiLieuVanBan", "Id");
        }
    }
}
