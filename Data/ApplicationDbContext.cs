#nullable enable

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
        public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
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

            modelBuilder.Entity<TaiKhoan>()
                .HasIndex(x => x.TenDangNhap).IsUnique();

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

            modelBuilder.Entity<TaiKhoan>()
                .HasOne(x => x.NhanVien)
                .WithMany(x => x.TaiKhoans)
                .HasForeignKey(x => x.MaNhanVien)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LichSuNguoiDung>()
                .HasOne(x => x.TaiKhoan)
                .WithMany(x => x.LichSuNguoiDungs)
                .HasForeignKey(x => x.MaTaiKhoan);

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

            modelBuilder.Entity<TaiKhoan>()
                .ToTable(t => t.HasCheckConstraint("CK_TK_VaiTro", "[VaiTro] IN ('Admin','HR','Employee')"));

            modelBuilder.Entity<BangLuong>()
                .Property(x => x.TongLuong)
                .HasComputedColumnSql("[LuongCoBan] + [Thuong] - [KhauTru]");

            modelBuilder.Entity<NghiPhep>()
                .Property(x => x.TrangThai)
                .HasDefaultValue("Chờ duyệt");

            modelBuilder.Entity<HopDong>()
                .Property(x => x.TrangThai)
                .HasDefaultValue("Đang hiệu lực");

            modelBuilder.Entity<TaiKhoan>()
                .Property(x => x.TrangThai)
                .HasDefaultValue(true);

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
                new CaLamViec { MaCa = 4, TenCa = "Ca Hành Chính", GioBatDau = TimeSpan.Parse("08:00"), GioKetThuc = TimeSpan.Parse("17:00") },
                new CaLamViec { MaCa = 5, TenCa = "Ca Linh Hoạt", GioBatDau = TimeSpan.Parse("07:00"), GioKetThuc = TimeSpan.Parse("19:00") }
            );
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan { MaTaiKhoan = 1, TenDangNhap = "admin", MatKhau = "admin123", VaiTro = "Admin", TrangThai = true },
                new TaiKhoan { MaTaiKhoan = 2, TenDangNhap = "hr", MatKhau = "hr123", VaiTro = "HR", TrangThai = true },
                new TaiKhoan { MaTaiKhoan = 3, TenDangNhap = "employee", MatKhau = "employee123", VaiTro = "Employee", TrangThai = true }           
            );
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien { MaNhanVien = 1, TenNhanVien = "Nguyễn Văn A", NgaySinh = new DateTime(1990, 1, 1), GioiTinh = "Nam", SoDienThoai = "0123456789", DiaChi = "Hà Nội", MaPhongBan = 1, MaChucVu = 1, MaTrinhDo = 2 },
                new NhanVien { MaNhanVien = 2, TenNhanVien = "Trần Thị B", NgaySinh = new DateTime(1992, 5, 15), GioiTinh = "Nữ", SoDienThoai = "0987654321", DiaChi = "Hồ Chí Minh", MaPhongBan = 2, MaChucVu = 2, MaTrinhDo = 3 },
                new NhanVien { MaNhanVien = 3, TenNhanVien = "Lê Văn C", NgaySinh = new DateTime(1985, 10, 20), GioiTinh = "Nam", SoDienThoai = "0112233445", DiaChi = "Đà Nẵng", MaPhongBan = 3, MaChucVu = 3, MaTrinhDo = 4 },
                new NhanVien { MaNhanVien = 4, TenNhanVien = "Phạm Thị D", NgaySinh = new DateTime(1995, 3, 10), GioiTinh = "Nữ", SoDienThoai = "0223344556", DiaChi = "Hải Phòng", MaPhongBan = 4, MaChucVu = 4, MaTrinhDo = 1 }
            );
            modelBuilder.Entity<HopDong>().HasData(
                new HopDong { MaHopDong = 1, MaNhanVien = 1, MaLoaiHopDong = 2, NgayBatDau = new DateTime(2020, 1, 1), NgayKetThuc = null, TrangThai = "Đang hiệu lực" },
                new HopDong { MaHopDong = 2, MaNhanVien = 2, MaLoaiHopDong = 2, NgayBatDau = new DateTime(2021, 6, 1), NgayKetThuc = null, TrangThai = "Đang hiệu lực" },
                new HopDong { MaHopDong = 3, MaNhanVien = 3, MaLoaiHopDong = 2, NgayBatDau = new DateTime(2019, 3, 1), NgayKetThuc = null, TrangThai = "Đang hiệu lực" },
                new HopDong { MaHopDong = 4, MaNhanVien = 4, MaLoaiHopDong = 1, NgayBatDau = new DateTime(2024, 1, 1), NgayKetThuc = new DateTime(2024, 6, 30), TrangThai = "Đang hiệu lực" }
             );
             modelBuilder.Entity<ChamCong>().HasData(
                new ChamCong { MaChamCong = 1, MaNhanVien = 1, NgayLam = new DateTime(2024, 6, 1), MaCa = 1, SoGioLam = 8, GioTangCa = 2 },
                new ChamCong { MaChamCong = 2, MaNhanVien = 2, NgayLam = new DateTime(2024, 6, 1), MaCa = 2, SoGioLam = 8, GioTangCa = 0 },
                new ChamCong { MaChamCong = 3, MaNhanVien = 3, NgayLam = new DateTime(2024, 6, 1), MaCa = 3, SoGioLam = 8, GioTangCa = 1 },
                new ChamCong { MaChamCong = 4, MaNhanVien = 4, NgayLam = new DateTime(2024, 6, 1), MaCa = 4, SoGioLam = 8, GioTangCa = 0 }
             );
             modelBuilder.Entity<NghiPhep>().HasData(
                new NghiPhep { MaNghiPhep = 1, MaNhanVien = 1, TuNgay = new DateTime(2024, 7, 1), DenNgay = new DateTime(2024, 7, 5), LyDo = "Đi du lịch", TrangThai = "Chờ duyệt" },
                new NghiPhep { MaNghiPhep = 2, MaNhanVien = 2, TuNgay = new DateTime(2024, 7, 10), DenNgay = new DateTime(2024, 7, 12), LyDo = "Thăm gia đình", TrangThai = "Chờ duyệt" },
                new NghiPhep { MaNghiPhep = 3, MaNhanVien = 3, TuNgay = new DateTime(2024, 7, 15), DenNgay = new DateTime(2024, 7, 20), LyDo = "Đi công tác", TrangThai = "Chờ duyệt" },
                new NghiPhep { MaNghiPhep = 4, MaNhanVien = 4, TuNgay = new DateTime(2024, 7, 5), DenNgay = new DateTime(2024, 7, 7), LyDo = "Đi học", TrangThai = "Chờ duyệt" }    
             );
             modelBuilder.Entity<BangLuong>().HasData(
                new BangLuong { MaBangLuong = 1, MaNhanVien = 1, Thang = 6, Nam = 2024, LuongCoBan = 5000000, Thuong = 1000000, KhauTru = 500000 },
                new BangLuong { MaBangLuong = 2, MaNhanVien = 2, Thang = 6, Nam = 2024, LuongCoBan = 8000000, Thuong = 1500000, KhauTru = 800000 },
                new BangLuong { MaBangLuong = 3, MaNhanVien = 3, Thang = 6, Nam = 2024, LuongCoBan = 15000000, Thuong = 3000000, KhauTru = 2000000 },
                new BangLuong { MaBangLuong = 4, MaNhanVien = 4, Thang = 6, Nam = 2024, LuongCoBan = 3000000, Thuong = 500000, KhauTru = 200000 },
                new BangLuong { MaBangLuong = 5, MaNhanVien = 1, Thang = 7, Nam = 2024, LuongCoBan = 5000000, Thuong = 1200000, KhauTru = 600000 }
             );

             modelBuilder.Entity<LichSuNguoiDung>().HasData(
                new LichSuNguoiDung { MaLichSu = 1, MaTaiKhoan = 3, HanhDong = "Đăng nhập", ThoiGian = new DateTime(2024, 6, 1) },
                new LichSuNguoiDung { MaLichSu = 2, MaTaiKhoan = 3, HanhDong = "Đăng nhập", ThoiGian = new DateTime(2024, 6, 7) },
                new LichSuNguoiDung { MaLichSu = 3, MaTaiKhoan = 3, HanhDong = "Đăng nhập", ThoiGian = new DateTime(2024, 6, 15) },
                new LichSuNguoiDung { MaLichSu = 4, MaTaiKhoan = 1, HanhDong = "Tạo hợp đồng cho Nguyễn Văn A", ThoiGian = new DateTime(2024, 6, 1) },
                new LichSuNguoiDung { MaLichSu = 5, MaTaiKhoan = 2, HanhDong = "Duyệt nghỉ phép cho Trần Thị B", ThoiGian = new DateTime(2024, 6, 7) },
                new LichSuNguoiDung { MaLichSu = 6, MaTaiKhoan = 2, HanhDong = "Cập nhật chấm công cho Lê Văn C", ThoiGian = new DateTime(2024, 6, 15) }
             );      
        }
           
    }
}