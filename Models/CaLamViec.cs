namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class CaLamViec
{
    [Key]
    public int MaCa { get; set; }

    public string TenCa { get; set; } = null!;

    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }

    public ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();
}