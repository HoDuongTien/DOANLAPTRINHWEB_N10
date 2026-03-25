using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebQLNhanSu.Models;


public class BangLuong
{
    [Key]
    public int MaBangLuong { get; set; }

    public int MaNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }

    public int Thang { get; set; }
    public int Nam { get; set; }

    public decimal LuongCoBan { get; set; }
    public decimal Thuong { get; set; }
    public decimal KhauTru { get; set; }

    public decimal TongLuong { get; set; }
}