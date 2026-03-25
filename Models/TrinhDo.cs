namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class TrinhDo
{       
    [Key]
    public int MaTrinhDo { get; set; }

    public string TenTrinhDo { get; set; } = null!;

    public ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}