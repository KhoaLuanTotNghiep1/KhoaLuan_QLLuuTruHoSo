namespace S3Train.WebHeThong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietMuonTra",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ThuMuon = c.String(),
                        MuonTraID = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MuonTra", t => t.MuonTraID)
                .Index(t => t.MuonTraID);
            
            CreateTable(
                "dbo.MuonTra",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NgayMuon = c.DateTime(),
                        NgayKetThuc = c.DateTime(),
                        SoLuong = c.Int(),
                        TinhTrang = c.String(),
                        UserId = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hop",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        ChuyenDe = c.String(maxLength: 300),
                        SoHop = c.Int(nullable: false),
                        TinhTrang = c.String(),
                        NgayBatDau = c.DateTime(),
                        NgayKetThuc = c.DateTime(),
                        KeId = c.String(maxLength: 128),
                        PhongBanId = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ke", t => t.KeId)
                .ForeignKey("dbo.PhongBan", t => t.PhongBanId)
                .Index(t => t.KeId)
                .Index(t => t.PhongBanId);
            
            CreateTable(
                "dbo.HoSo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TapHoSo = c.String(nullable: true, maxLength: 300),
                        PhongLuuTru = c.String(),
                        TinhTrang = c.String(maxLength: 150),
                        ThoiGianBaoQuan = c.Int(),
                        GhiChu = c.String(),
                        BienMucHoSo = c.String(),
                        HoSoChaId = c.String(maxLength: 128),
                        UserId = c.String(),
                        HopId = c.String(maxLength: 128),
                        LoaiHoSoId = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HoSo", t => t.HoSoChaId)
                .ForeignKey("dbo.LoaiHoSo", t => t.LoaiHoSoId)
                .ForeignKey("dbo.Hop", t => t.HopId)
                .Index(t => t.HoSoChaId)
                .Index(t => t.HopId)
                .Index(t => t.LoaiHoSoId);
            
            CreateTable(
                "dbo.LoaiHoSo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 300),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaiLieuVanBan",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 300),
                        Loai = c.String(maxLength: 300),
                        So = c.String(maxLength: 300),
                        KhoGiay = c.String(maxLength: 300),
                        SoTo = c.Int(),
                        TacGia = c.String(maxLength: 300),
                        TinhTrang = c.String(maxLength: 150),
                        DuongDan = c.String(),
                        GhiChu = c.String(),
                        HoSoId = c.String(nullable: false, maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HoSo", t => t.HoSoId, cascadeDelete: true)
                .Index(t => t.HoSoId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Avatar = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Ke",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 300),
                        UserId = c.String(),
                        SoThuTu = c.Int(nullable: false),
                        SoHopToiDa = c.Int(nullable: false),
                        SoHopHienTai = c.Int(nullable: false),
                        NamBatDau = c.DateTime(),
                        NamKetThuc = c.DateTime(),
                        TinhTrang = c.String(),
                        KhoId = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kho", t => t.KhoId)
                .Index(t => t.KhoId);
            
            CreateTable(
                "dbo.Kho",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 300),
                        ViTri = c.String(maxLength: 300),
                        DienTich = c.String(maxLength: 100),
                        NgươiQuanLy = c.String(maxLength: 100),
                        DonViTinh = c.String(maxLength: 30),
                        SoLuongHienTai = c.Int(),
                        SoLuongMax = c.Int(),
                        TinhTrang = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhongBan",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(maxLength: 300),
                        NgayTao = c.DateTime(nullable: false),
                        NgayCapNhat = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserTaiLieuVanBans",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        TaiLieuVanBan_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.TaiLieuVanBan_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaiLieuVanBan", t => t.TaiLieuVanBan_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.TaiLieuVanBan_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Hop", "PhongBanId", "dbo.PhongBan");
            DropForeignKey("dbo.Ke", "KhoId", "dbo.Kho");
            DropForeignKey("dbo.Hop", "KeId", "dbo.Ke");
            DropForeignKey("dbo.HoSo", "HopId", "dbo.Hop");
            DropForeignKey("dbo.TaiLieuVanBan", "HoSoId", "dbo.HoSo");
            DropForeignKey("dbo.ApplicationUserTaiLieuVanBans", "TaiLieuVanBan_Id", "dbo.TaiLieuVanBan");
            DropForeignKey("dbo.ApplicationUserTaiLieuVanBans", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HoSo", "LoaiHoSoId", "dbo.LoaiHoSo");
            DropForeignKey("dbo.HoSo", "HoSoChaId", "dbo.HoSo");
            DropForeignKey("dbo.ChiTietMuonTra", "MuonTraID", "dbo.MuonTra");
            DropIndex("dbo.ApplicationUserTaiLieuVanBans", new[] { "TaiLieuVanBan_Id" });
            DropIndex("dbo.ApplicationUserTaiLieuVanBans", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ke", new[] { "KhoId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.TaiLieuVanBan", new[] { "HoSoId" });
            DropIndex("dbo.HoSo", new[] { "LoaiHoSoId" });
            DropIndex("dbo.HoSo", new[] { "HopId" });
            DropIndex("dbo.HoSo", new[] { "HoSoChaId" });
            DropIndex("dbo.Hop", new[] { "PhongBanId" });
            DropIndex("dbo.Hop", new[] { "KeId" });
            DropIndex("dbo.ChiTietMuonTra", new[] { "MuonTraID" });
            DropTable("dbo.ApplicationUserTaiLieuVanBans");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PhongBan");
            DropTable("dbo.Kho");
            DropTable("dbo.Ke");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TaiLieuVanBan");
            DropTable("dbo.LoaiHoSo");
            DropTable("dbo.HoSo");
            DropTable("dbo.Hop");
            DropTable("dbo.MuonTra");
            DropTable("dbo.ChiTietMuonTra");
        }
    }
}
