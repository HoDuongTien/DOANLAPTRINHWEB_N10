
namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class ChamCong
{
    [Key]   
    public int MaChamCong { get; set; }

    public int MaNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }

    public int MaCa { get; set; }
    public CaLamViec? CaLamViec { get; set; }

    public DateTime NgayLam { get; set; }

    public TimeSpan? GioVao { get; set; }
    public TimeSpan? GioRa { get; set; }

    public double SoGioLam { get; set; }
    public double GioTangCa { get; set; }
}