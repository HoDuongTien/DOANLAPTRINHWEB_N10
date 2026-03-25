namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class TaiKhoan
{
    [Key]
    public int MaTaiKhoan { get; set; }

    public string TenDangNhap { get; set; } = null!;
    public string MatKhau { get; set; } = null!;
    public string VaiTro { get; set; } = null!;

    public bool TrangThai { get; set; } = true;

    public int? MaNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }

    public ICollection<LichSuNguoiDung> LichSuNguoiDungs { get; set; } = new List<LichSuNguoiDung>();
}