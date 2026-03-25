namespace WebQLNhanSu.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class LoaiHopDong
{           
    [Key]   
    public int MaLoaiHopDong { get; set; }

    public string TenLoaiHopDong { get; set; } = null!;

    public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
}