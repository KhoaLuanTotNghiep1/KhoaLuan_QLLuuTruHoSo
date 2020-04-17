namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adjustdatabse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserTaiLieuVanBans", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserTaiLieuVanBans", "TaiLieuVanBan_Id", "dbo.TaiLieuVanBan");
            DropIndex("dbo.ApplicationUserTaiLieuVanBans", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserTaiLieuVanBans", new[] { "TaiLieuVanBan_Id" });
            AddColumn("dbo.TaiLieuVanBan", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.MuonTra", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Hop", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.HoSo", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ke", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MuonTra", "UserId");
            CreateIndex("dbo.Hop", "UserId");
            CreateIndex("dbo.HoSo", "UserId");
            CreateIndex("dbo.TaiLieuVanBan", "UserId");
            CreateIndex("dbo.Ke", "UserId");
            AddForeignKey("dbo.TaiLieuVanBan", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.HoSo", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Ke", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Hop", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MuonTra", "UserId", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ApplicationUserTaiLieuVanBans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserTaiLieuVanBans",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        TaiLieuVanBan_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.TaiLieuVanBan_Id });
            
            DropForeignKey("dbo.MuonTra", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Hop", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ke", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HoSo", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaiLieuVanBan", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Ke", new[] { "UserId" });
            DropIndex("dbo.TaiLieuVanBan", new[] { "UserId" });
            DropIndex("dbo.HoSo", new[] { "UserId" });
            DropIndex("dbo.Hop", new[] { "UserId" });
            DropIndex("dbo.MuonTra", new[] { "UserId" });
            AlterColumn("dbo.Ke", "UserId", c => c.String());
            AlterColumn("dbo.HoSo", "UserId", c => c.String());
            AlterColumn("dbo.Hop", "UserId", c => c.String());
            AlterColumn("dbo.MuonTra", "UserId", c => c.String());
            DropColumn("dbo.TaiLieuVanBan", "UserId");
            CreateIndex("dbo.ApplicationUserTaiLieuVanBans", "TaiLieuVanBan_Id");
            CreateIndex("dbo.ApplicationUserTaiLieuVanBans", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserTaiLieuVanBans", "TaiLieuVanBan_Id", "dbo.TaiLieuVanBan", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserTaiLieuVanBans", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
