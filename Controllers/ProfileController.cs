using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

[Authorize]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private void LoadDropdown(NhanVien? nv = null)
    {
        ViewBag.PhongBan = new SelectList(_context.PhongBans, "MaPhongBan", "TenPhongBan", nv?.MaPhongBan);
        ViewBag.ChucVu = new SelectList(_context.ChucVus, "MaChucVu", "TenChucVu", nv?.MaChucVu);
        ViewBag.TrinhDo = new SelectList(_context.TrinhDos, "MaTrinhDo", "TenTrinhDo", nv?.MaTrinhDo);

        var gioiTinhs = new List<string> { "Nam", "Nữ", "Khác" };
        ViewBag.GioiTinh = new SelectList(gioiTinhs, nv?.GioiTinh);
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        var nv = await _context.NhanViens
            .Include(x => x.PhongBan)
            .Include(x => x.ChucVu)
            .Include(x => x.TrinhDo)
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (nv == null)
        {
            return RedirectToAction(nameof(Edit));
        }

        return View(nv);
    }

    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        var nv = await _context.NhanViens
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        LoadDropdown(nv);

        return View(nv ?? new NhanVien());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(NhanVien model)
    {
        ModelState.Remove("UserId");
        ModelState.Remove("User");
        ModelState.Remove("PhongBan");
        ModelState.Remove("ChucVu");
        ModelState.Remove("TrinhDo");
        ModelState.Remove("HopDongs");
        ModelState.Remove("ChamCongs");
        ModelState.Remove("NghiPheps");
        ModelState.Remove("BangLuongs");
        ModelState.Remove("LichSuNguoiDungs");

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        if (!ModelState.IsValid)
        {
            LoadDropdown(model);
            return View(model);
        }

        var nv = await _context.NhanViens
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (nv == null)
        {
            model.UserId = user.Id;
            model.User = null;
            _context.NhanViens.Add(model);
        }
        else
        {
            nv.TenNhanVien = model.TenNhanVien;
            nv.GioiTinh = model.GioiTinh;
            nv.NgaySinh = model.NgaySinh;
            nv.SoDienThoai = model.SoDienThoai;
            nv.DiaChi = model.DiaChi;
            nv.MaPhongBan = model.MaPhongBan;
            nv.MaChucVu = model.MaChucVu;
            nv.MaTrinhDo = model.MaTrinhDo;
        }

        await _context.SaveChangesAsync();

        TempData["msg"] = "Cập nhật hồ sơ thành công";
        return RedirectToAction(nameof(Index));
    }
}