namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LichSuNguoiDung
{
    [Key]
    public int MaLichSu { get; set; }

    public int MaNhanVien { get; set; }

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }

    public DateTime ThoiGian { get; set; } = DateTime.Now;

    [Required]
    public string HanhDong { get; set; } = null!;

    public string? MoTa { get; set; }
}