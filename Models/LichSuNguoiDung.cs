namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class LichSuNguoiDung
{   [Key]   
    public int MaLichSu { get; set; }

    public int MaTaiKhoan { get; set; }
    public TaiKhoan? TaiKhoan { get; set; }

    public DateTime ThoiGian { get; set; } = DateTime.Now;

    public string HanhDong { get; set; } = null!;
    public string? MoTa { get; set; }
}