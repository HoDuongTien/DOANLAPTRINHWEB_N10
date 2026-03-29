using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize]
    public class LuongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LuongController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nhanVien == null)
            {
                TempData["Error"] = "Bạn chưa có hồ sơ nhân viên.";
                return RedirectToAction("Index", "Home");
            }

            var list = await _context.BangLuongs
                .Where(x => x.MaNhanVien == nhanVien.MaNhanVien)
                .OrderByDescending(x => x.Nam)
                .ThenByDescending(x => x.Thang)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Manage()
        {
            var list = await _context.BangLuongs
                .Include(x => x.NhanVien)
                .OrderByDescending(x => x.Nam)
                .ThenByDescending(x => x.Thang)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Admin,HR")]
        public IActionResult Create()
        {
            ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create(BangLuong model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien", model.MaNhanVien);
                return View(model);
            }

            var daTonTai = await _context.BangLuongs.AnyAsync(x =>
                x.MaNhanVien == model.MaNhanVien &&
                x.Thang == model.Thang &&
                x.Nam == model.Nam);

            if (daTonTai)
            {
                ModelState.AddModelError("", "Bảng lương tháng này của nhân viên đã tồn tại.");
                ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien", model.MaNhanVien);
                return View(model);
            }

            _context.BangLuongs.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tạo bảng lương thành công.";
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Edit(int id)
        {
            var bangLuong = await _context.BangLuongs.FindAsync(id);
            if (bangLuong == null) return NotFound();

            ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien", bangLuong.MaNhanVien);
            return View(bangLuong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Edit(BangLuong model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien", model.MaNhanVien);
                return View(model);
            }

            var bangLuongKhac = await _context.BangLuongs.AnyAsync(x =>
                x.MaBangLuong != model.MaBangLuong &&
                x.MaNhanVien == model.MaNhanVien &&
                x.Thang == model.Thang &&
                x.Nam == model.Nam);

            if (bangLuongKhac)
            {
                ModelState.AddModelError("", "Đã tồn tại bảng lương khác cùng tháng/năm cho nhân viên này.");
                ViewBag.NhanVien = new SelectList(_context.NhanViens.ToList(), "MaNhanVien", "TenNhanVien", model.MaNhanVien);
                return View(model);
            }

            _context.BangLuongs.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật bảng lương thành công.";
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int id)
        {
            var bangLuong = await _context.BangLuongs
                .Include(x => x.NhanVien)
                .FirstOrDefaultAsync(x => x.MaBangLuong == id);

            if (bangLuong == null) return NotFound();

            return View(bangLuong);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bangLuong = await _context.BangLuongs.FindAsync(id);
            if (bangLuong == null) return NotFound();

            _context.BangLuongs.Remove(bangLuong);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa bảng lương thành công.";
            return RedirectToAction("Manage");
        }
    }
}