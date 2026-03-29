#nullable enable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Models;
using WebQLNhanSu.Enums;

namespace WebQLNhanSu.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PhongBan> PhongBans => Set<PhongBan>();
        public DbSet<ChucVu> ChucVus => Set<ChucVu>();
        public DbSet<CaLamViec> CaLamViecs => Set<CaLamViec>();
        public DbSet<LoaiHopDong> LoaiHopDongs => Set<LoaiHopDong>();
        public DbSet<TrinhDo> TrinhDos => Set<TrinhDo>();
        public DbSet<NhanVien> NhanViens => Set<NhanVien>();
        public DbSet<HopDong> HopDongs => Set<HopDong>();
        public DbSet<ChamCong> ChamCongs => Set<ChamCong>();
        public DbSet<NghiPhep> NghiPheps => Set<NghiPhep>();
        public DbSet<BangLuong> BangLuongs => Set<BangLuong>();
        public DbSet<CauHinhLuong> CauHinhLuongs => Set<CauHinhLuong>();
        public DbSet<LichSuNguoiDung> LichSuNguoiDungs => Set<LichSuNguoiDung>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhongBan>().HasIndex(x => x.TenPhongBan).IsUnique();
            modelBuilder.Entity<ChucVu>().HasIndex(x => x.TenChucVu).IsUnique();
            modelBuilder.Entity<LoaiHopDong>().HasIndex(x => x.TenLoaiHopDong).IsUnique();
            modelBuilder.Entity<TrinhDo>().HasIndex(x => x.TenTrinhDo).IsUnique();

            modelBuilder.Entity<NhanVien>()
                .HasIndex(x => x.SoDienThoai)
                .IsUnique()
                .HasFilter("[SoDienThoai] IS NOT NULL");

            modelBuilder.Entity<BangLuong>()
                .HasIndex(x => new { x.MaNhanVien, x.Thang, x.Nam })
                .IsUnique();

            modelBuilder.Entity<NhanVien>()
                .HasOne(x => x.PhongBan)
                .WithMany(x => x.NhanViens)
                .HasForeignKey(x => x.MaPhongBan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NhanVien>()
                .HasOne(x => x.ChucVu)
                .WithMany(x => x.NhanViens)
                .HasForeignKey(x => x.MaChucVu)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NhanVien>()
                .HasOne(x => x.TrinhDo)
                .WithMany(x => x.NhanViens)
                .HasForeignKey(x => x.MaTrinhDo)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<HopDong>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.HopDongs)
                .HasForeignKey(x => x.MaNhanVien);

            modelBuilder.Entity<HopDong>()
                .HasOne(x => x.LoaiHopDong)
                .WithMany(x => x.HopDongs)
                .HasForeignKey(x => x.MaLoaiHopDong);

            modelBuilder.Entity<ChamCong>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.ChamCongs)
                .HasForeignKey(x => x.MaNhanVien);

            modelBuilder.Entity<ChamCong>()
                .HasOne(x => x.CaLamViec)
                .WithMany(x => x.ChamCongs)
                .HasForeignKey(x => x.MaCa);

            modelBuilder.Entity<NghiPhep>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.NghiPheps)
                .HasForeignKey(x => x.MaNhanVien);

            modelBuilder.Entity<BangLuong>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.BangLuongs)
                .HasForeignKey(x => x.MaNhanVien);

            modelBuilder.Entity<LichSuNguoiDung>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.LichSuNguoiDungs)
                .HasForeignKey(x => x.MaNhanVien)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaLamViec>()
                .ToTable(t => t.HasCheckConstraint("CK_CaLamViec_Gio", "[GioKetThuc] > [GioBatDau]"));

            modelBuilder.Entity<HopDong>()
                .ToTable(t => t.HasCheckConstraint("CK_HopDong_Ngay", "[NgayKetThuc] IS NULL OR [NgayKetThuc] >= [NgayBatDau]"));

            modelBuilder.Entity<NghiPhep>()
                .ToTable(t => t.HasCheckConstraint("CK_NghiPhep_Ngay", "[DenNgay] >= [TuNgay]"));

            modelBuilder.Entity<ChamCong>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_ChamCong_Gio", "[SoGioLam] >= 0");
                    t.HasCheckConstraint("CK_ChamCong_OT", "[GioTangCa] >= 0");
                });

            modelBuilder.Entity<ChucVu>()
                .ToTable(t => t.HasCheckConstraint("CK_ChucVu_Luong", "[LuongCoBan] >= 0"));

            modelBuilder.Entity<BangLuong>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_BL_Thang", "[Thang] BETWEEN 1 AND 12");
                    t.HasCheckConstraint("CK_BL_Nam", "[Nam] >= 2000");
                    t.HasCheckConstraint("CK_BL_Tien", "[LuongCoBan] >= 0 AND [Thuong] >= 0 AND [KhauTru] >= 0");
                });

            modelBuilder.Entity<NhanVien>()
                .ToTable(t => t.HasCheckConstraint("CK_NV_GioiTinh", "[GioiTinh] IN (N'Nam', N'Nữ', N'Khác')"));

            modelBuilder.Entity<BangLuong>()
                .Property(x => x.TongLuong)
                .HasComputedColumnSql("[LuongCoBan] + [Thuong] - [KhauTru]");

            modelBuilder.Entity<NghiPhep>()
                .Property(x => x.TrangThaiNghiPhep)
                .HasDefaultValue(TrangThaiNghiPhep.ChoDuyet);

            modelBuilder.Entity<HopDong>()
                .Property(x => x.TrangThai)
                .HasDefaultValue("Đang hiệu lực");

            modelBuilder.Entity<LichSuNguoiDung>()
                .Property(x => x.ThoiGian)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<PhongBan>().HasData(
                new PhongBan { MaPhongBan = 1, TenPhongBan = "Phòng Nhân Sự" },
                new PhongBan { MaPhongBan = 2, TenPhongBan = "Phòng Kỹ Thuật" },
                new PhongBan { MaPhongBan = 3, TenPhongBan = "Phòng Kinh Doanh" },
                new PhongBan { MaPhongBan = 4, TenPhongBan = "Phòng Tài Chính" },
                new PhongBan { MaPhongBan = 5, TenPhongBan = "Phòng Marketing" },
                new PhongBan { MaPhongBan = 6, TenPhongBan = "Phòng Hành Chính" }
            );

            modelBuilder.Entity<ChucVu>().HasData(
                new ChucVu { MaChucVu = 1, TenChucVu = "Nhân Viên", LuongCoBan = 5000000 },
                new ChucVu { MaChucVu = 2, TenChucVu = "Trưởng Phòng", LuongCoBan = 8000000 },
                new ChucVu { MaChucVu = 3, TenChucVu = "Giám Đốc", LuongCoBan = 15000000 },
                new ChucVu { MaChucVu = 4, TenChucVu = "Thực Tập Sinh", LuongCoBan = 3000000 },
                new ChucVu { MaChucVu = 5, TenChucVu = "Phó Giám Đốc", LuongCoBan = 12000000 }
            );

            modelBuilder.Entity<LoaiHopDong>().HasData(
                new LoaiHopDong { MaLoaiHopDong = 1, TenLoaiHopDong = "Hợp Đồng Thử Việc" },
                new LoaiHopDong { MaLoaiHopDong = 2, TenLoaiHopDong = "Hợp Đồng Chính Thức" },
                new LoaiHopDong { MaLoaiHopDong = 3, TenLoaiHopDong = "Hợp Đồng Thời Vụ" },
                new LoaiHopDong { MaLoaiHopDong = 4, TenLoaiHopDong = "Hợp Đồng Cộng Tác Viên" },
                new LoaiHopDong { MaLoaiHopDong = 5, TenLoaiHopDong = "Hợp Đồng Thực Tập" }
            );

            modelBuilder.Entity<TrinhDo>().HasData(
                new TrinhDo { MaTrinhDo = 1, TenTrinhDo = "Cao Đẳng" },
                new TrinhDo { MaTrinhDo = 2, TenTrinhDo = "Đại Học" },
                new TrinhDo { MaTrinhDo = 3, TenTrinhDo = "Thạc Sĩ" },
                new TrinhDo { MaTrinhDo = 4, TenTrinhDo = "Tiến Sĩ" },
                new TrinhDo { MaTrinhDo = 5, TenTrinhDo = "Không Có" }
            );

            modelBuilder.Entity<CauHinhLuong>().HasData(
                new CauHinhLuong { MaCauHinh = 1, LuongCoBanMacDinh = 100000, HeSoTangCa = 2.2, PhuCap = 50000 },
                new CauHinhLuong { MaCauHinh = 2, LuongCoBanMacDinh = 200000, HeSoTangCa = 1.3, PhuCap = 100000 },
                new CauHinhLuong { MaCauHinh = 3, LuongCoBanMacDinh = 300000, HeSoTangCa = 1.4, PhuCap = 150000 },
                new CauHinhLuong { MaCauHinh = 4, LuongCoBanMacDinh = 400000, HeSoTangCa = 2.5, PhuCap = 200000 }
            );

            modelBuilder.Entity<CaLamViec>().HasData(
                new CaLamViec { MaCa = 1, TenCa = "Ca Sáng", GioBatDau = TimeSpan.Parse("08:00"), GioKetThuc = TimeSpan.Parse("16:00") },
                new CaLamViec { MaCa = 2, TenCa = "Ca Chiều", GioBatDau = TimeSpan.Parse("16:00"), GioKetThuc = TimeSpan.Parse("21:00") },
                new CaLamViec { MaCa = 3, TenCa = "Ca Đêm", GioBatDau = TimeSpan.Parse("00:00"), GioKetThuc = TimeSpan.Parse("08:00") },
                new CaLamViec { MaCa = 4, TenCa = "Ca Hành Chính", GioBatDau = TimeSpan.Parse("08:00"), GioKetThuc = TimeSpan.Parse("17:00") }
            );
        }
    }
}