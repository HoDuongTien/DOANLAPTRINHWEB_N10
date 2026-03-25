namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class NghiPhep
{               
    [Key]   
    public int MaNghiPhep { get; set; }

    public int MaNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }

    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }

    public string? LyDo { get; set; }

    public string TrangThai { get; set; } = "Chờ duyệt";
}