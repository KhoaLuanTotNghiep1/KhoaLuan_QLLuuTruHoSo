using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace S3Train.Domain
{
    public class ApplicationDbContext : 
        IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Ke> Kes { get; set; }
        public DbSet<ChiTietMuonTra> ChiTietMuonTras { get; set; }
        public DbSet<Hop> Hops { get; set; }
        public DbSet<HoSo> HoSos { get; set; }
        public DbSet<Tu> Tus { get; set; }
        public DbSet<LoaiHoSo> LoaiHoSos { get; set; }
        public DbSet<MuonTra> MuonTras { get; set; }
        public DbSet<PhongBan> phongBans { get; set; }
        public DbSet<TaiLieuVanBan> TaiLieuVanBans { get; set; }
        public DbSet<NoiBanHanh> NoiBanHanhs { get; set; }
        public DbSet<LichSuHoatDong> lichSuHoatDongs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tu>().ToTable("Tu");
            modelBuilder.Entity<Tu>().Property(p => p.Ten).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.ViTri).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.NgươiQuanLy).HasMaxLength(100).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.DonViTinh).HasMaxLength(30).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.SoLuongMax).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.SoLuongHienTai).IsOptional();
            modelBuilder.Entity<Tu>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<Tu>().HasMany(p => p.Kes).WithOptional(prod => prod.Tu);

            modelBuilder.Entity<Ke>().ToTable("Ke");
            modelBuilder.Entity<Ke>().Property(p => p.Ten).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<Ke>().Property(p => p.SoThuTu).IsRequired();
            modelBuilder.Entity<Ke>().Property(p => p.SoHopToiDa).IsRequired();
            modelBuilder.Entity<Ke>().Property(p => p.SoHopHienTai).IsRequired();
            modelBuilder.Entity<Ke>().Property(p => p.NamBatDau).IsOptional();
            modelBuilder.Entity<Ke>().Property(p => p.NamKetThuc).IsOptional();
            modelBuilder.Entity<Ke>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<Ke>().HasMany(p => p.Hops).WithOptional(prod => prod.Ke);

            modelBuilder.Entity<Hop>().ToTable("Hop");
            modelBuilder.Entity<Hop>().Property(p => p.ChuyenDe).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<Hop>().Property(p => p.SoHop).IsRequired();
            modelBuilder.Entity<Hop>().Property(p => p.NgayBatDau).IsOptional();
            modelBuilder.Entity<Hop>().Property(p => p.NgayKetThuc).IsOptional();
            modelBuilder.Entity<Hop>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<Hop>().HasMany(p => p.HoSos).WithOptional(prod => prod.Hop);

            modelBuilder.Entity<PhongBan>().ToTable("PhongBan");
            modelBuilder.Entity<PhongBan>().Property(p => p.Ten).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<PhongBan>().HasMany(p => p.Hops).WithOptional(prod => prod.PhongBan);

            modelBuilder.Entity<LoaiHoSo>().ToTable("LoaiHoSo");
            modelBuilder.Entity<LoaiHoSo>().Property(p => p.Ma).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<LoaiHoSo>().Property(p => p.Ten).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<LoaiHoSo>().HasMany(p => p.HoSos).WithOptional(prod => prod.LoaiHoSo);

            modelBuilder.Entity<HoSo>().ToTable("HoSo");
            modelBuilder.Entity<HoSo>().Property(p => p.PhongLuuTru).IsOptional();
            modelBuilder.Entity<HoSo>().Property(p => p.ThoiGianBaoQuan).IsOptional();
            modelBuilder.Entity<HoSo>().Property(p => p.GhiChu).IsOptional();
            modelBuilder.Entity<HoSo>().Property(p => p.BienMucHoSo).IsOptional();
            modelBuilder.Entity<HoSo>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<HoSo>().HasMany(p => p.TaiLieuVanBans).WithRequired(prod => prod.HoSo);
            modelBuilder.Entity<HoSo>().HasOptional(p => p.TapHoSo).WithMany(c => c.HoSoCons).HasForeignKey(b => b.TapHoSoId);


            modelBuilder.Entity<TaiLieuVanBan>().ToTable("TaiLieuVanBan");
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.Ten).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.Loai).HasMaxLength(300).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.SoKyHieu).HasMaxLength(100).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NoiDung).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NguoiDuyet).HasMaxLength(100).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NguoiKy).HasMaxLength(100).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NguoiGuiHoacNhan).HasMaxLength(100).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NoiNhan).HasMaxLength(200).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.SoTo).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.GhiChu).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.DuongDan).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.NgayBanHanh).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.Dang).HasMaxLength(30).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().Property(p => p.HinhAnh).IsOptional();
            modelBuilder.Entity<TaiLieuVanBan>().HasMany(p => p.ChiTietMuonTras).WithOptional(p => p.TaiLieuVanBan);

            modelBuilder.Entity<MuonTra>().ToTable("MuonTra");
            modelBuilder.Entity<MuonTra>().Property(p => p.NgayMuon).IsOptional();
            modelBuilder.Entity<MuonTra>().Property(p => p.NgayKetThuc).IsOptional();
            modelBuilder.Entity<MuonTra>().Property(p => p.TinhTrang).IsOptional();
            modelBuilder.Entity<MuonTra>().Property(p => p.VanThu).IsOptional();
            modelBuilder.Entity<MuonTra>().Property(p => p.SoLuong).IsOptional();
            modelBuilder.Entity<MuonTra>().HasMany(p => p.ChiTietMuonTras).WithOptional(prod => prod.MuonTra);


            modelBuilder.Entity<ChiTietMuonTra>().ToTable("ChiTietMuonTra");

            modelBuilder.Entity<NoiBanHanh>().ToTable("NoiBanHanh");
            modelBuilder.Entity<NoiBanHanh>().Property(p => p.Ten).IsOptional();
            modelBuilder.Entity<NoiBanHanh>().Property(p => p.MoTa).IsOptional();
            modelBuilder.Entity<NoiBanHanh>().HasMany(p => p.TaiLieuVanBans).WithOptional(prod => prod.NoiBanHanh);

            modelBuilder.Entity<LichSuHoatDong>().ToTable("LichSuHoatDong");
            modelBuilder.Entity<LichSuHoatDong>().Property(p => p.HoatDong).IsOptional();
            modelBuilder.Entity<LichSuHoatDong>().Property(p => p.ChiTietHoatDong).IsOptional();
            
        }
    }
}