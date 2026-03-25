namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class ChucVu
{
    [Key]   
    public int MaChucVu { get; set; }

    public string TenChucVu { get; set; } = null!;

    public decimal LuongCoBan { get; set; }

    public ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}