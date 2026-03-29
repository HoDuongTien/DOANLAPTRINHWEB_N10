using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebQLNhanSu.Data;
using WebQLNhanSu.Enums;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize]
    public class NghiPhepController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NghiPhepController(ApplicationDbContext context)
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

            var list = await _context.NghiPheps
                .Where(x => x.MaNhanVien == nhanVien.MaNhanVien)
                .OrderByDescending(x => x.TuNgay)
                .ToListAsync();

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nhanVien == null)
            {
                TempData["Error"] = "Bạn chưa có hồ sơ nhân viên.";
                return RedirectToAction("Index");
            }

            var model = new NghiPhep
            {
                MaNhanVien = nhanVien.MaNhanVien,
                TuNgay = DateTime.Today,
                DenNgay = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NghiPhep model)
        {
            if (model.DenNgay < model.TuNgay)
            {
                ModelState.AddModelError("", "Đến ngày phải lớn hơn hoặc bằng từ ngày.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.TrangThaiNghiPhep = TrangThaiNghiPhep.ChoDuyet;

            _context.NghiPheps.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Gửi đơn nghỉ phép thành công.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Manage(string? status)
        {
            var query = _context.NghiPheps
                .Include(x => x.NhanVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) &&
                Enum.TryParse<TrangThaiNghiPhep>(status, out var trangThai))
            {
                query = query.Where(x => x.TrangThaiNghiPhep == trangThai);
            }

            var list = await query
                .OrderByDescending(x => x.TuNgay)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Duyet(int id)
        {
            var nghiPhep = await _context.NghiPheps.FindAsync(id);
            if (nghiPhep == null) return NotFound();

            nghiPhep.TrangThaiNghiPhep = TrangThaiNghiPhep.DaDuyet;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã duyệt đơn nghỉ phép.";
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> TuChoi(int id)
        {
            var nghiPhep = await _context.NghiPheps.FindAsync(id);
            if (nghiPhep == null) return NotFound();

            nghiPhep.TrangThaiNghiPhep = TrangThaiNghiPhep.TuChoi;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã từ chối đơn nghỉ phép.";
            return RedirectToAction("Manage");
        }
    }
}