
namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CauHinhLuong
{
    [Key]
    public int MaCauHinh { get; set; }

    public decimal LuongCoBanMacDinh { get; set; }
    public double HeSoTangCa { get; set; }
    public decimal PhuCap { get; set; }
}