namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableNoiBanHanhAndLichSuHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoiBanHanh",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(),
                        MoTa = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LichSuHoatDong",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        HoatDong = c.String(),
                        ChiTietHoatDong = c.String(),
                        UserId = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.TaiLieuVanBan", "NoiBanHanhId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TaiLieuVanBan", "NoiBanHanhId");
            AddForeignKey("dbo.TaiLieuVanBan", "NoiBanHanhId", "dbo.NoiBanHanh", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LichSuHoatDong", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaiLieuVanBan", "NoiBanHanhId", "dbo.NoiBanHanh");
            DropIndex("dbo.LichSuHoatDong", new[] { "UserId" });
            DropIndex("dbo.TaiLieuVanBan", new[] { "NoiBanHanhId" });
            DropColumn("dbo.TaiLieuVanBan", "NoiBanHanhId");
            DropTable("dbo.LichSuHoatDong");
            DropTable("dbo.NoiBanHanh");
        }
    }
}
