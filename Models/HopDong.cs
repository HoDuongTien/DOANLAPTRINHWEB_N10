using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebQLNhanSu.Models;

public class HopDong
{
    [Key]
    public int MaHopDong { get; set; }

    public int MaNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }

    public int MaLoaiHopDong { get; set; }
    public LoaiHopDong? LoaiHopDong { get; set; }

    public DateTime NgayBatDau { get; set; }
    public DateTime? NgayKetThuc { get; set; }

    public decimal Luong { get; set; }

    public string TrangThai { get; set; } = "Đang hiệu lực";
}