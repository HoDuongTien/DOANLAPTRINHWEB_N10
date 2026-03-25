namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class NhanVien
{                   
    [Key]
    public int MaNhanVien { get; set; }

    public string TenNhanVien { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public string GioiTinh { get; set; } = null!;

    public string? SoDienThoai { get; set; }
    public string? DiaChi { get; set; }

    public int MaPhongBan { get; set; }
    public PhongBan? PhongBan { get; set; }

    public int MaChucVu { get; set; }
    public ChucVu? ChucVu { get; set; }

    public int? MaTrinhDo { get; set; }
    public TrinhDo? TrinhDo { get; set; }

    public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
    public ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();
    public ICollection<NghiPhep> NghiPheps { get; set; } = new List<NghiPhep>();
    public ICollection<BangLuong> BangLuongs { get; set; } = new List<BangLuong>();
    public ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}