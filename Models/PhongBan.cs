namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class PhongBan
{   
    [Key]
    public int MaPhongBan { get; set; }

    public string TenPhongBan { get; set; } = null!;

    public ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
