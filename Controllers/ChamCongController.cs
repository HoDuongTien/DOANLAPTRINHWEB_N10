using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize]
    public class ChamCongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChamCongController(ApplicationDbContext context)
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

            ViewBag.CaLamViecs = await _context.CaLamViecs.ToListAsync();

            var danhSachChamCong = await _context.ChamCongs
                .Include(x => x.CaLamViec)
                .Where(x => x.MaNhanVien == nhanVien.MaNhanVien)
                .OrderByDescending(x => x.NgayLam)
                .ToListAsync();

            return View(danhSachChamCong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int maCa)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nhanVien == null)
            {
                TempData["Error"] = "Bạn chưa có hồ sơ nhân viên.";
                return RedirectToAction("Index");
            }

            var homNay = DateTime.Today;

            var daChamCong = await _context.ChamCongs.AnyAsync(x =>
                x.MaNhanVien == nhanVien.MaNhanVien &&
                x.NgayLam.Date == homNay);

            if (daChamCong)
            {
                TempData["Error"] = "Hôm nay bạn đã chấm công rồi.";
                return RedirectToAction("Index");
            }

            var caLam = await _context.CaLamViecs.FirstOrDefaultAsync(x => x.MaCa == maCa);
            if (caLam == null)
            {
                TempData["Error"] = "Ca làm việc không tồn tại.";
                return RedirectToAction("Index");
            }

            var chamCong = new ChamCong
            {
                MaNhanVien = nhanVien.MaNhanVien,
                MaCa = maCa,
                NgayLam = homNay,
                GioVao = DateTime.Now.TimeOfDay,
                GioRa = null,
                SoGioLam = 0,
                GioTangCa = 0
            };

            _context.ChamCongs.Add(chamCong);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Chấm công vào thành công.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nhanVien == null)
            {
                TempData["Error"] = "Bạn chưa có hồ sơ nhân viên.";
                return RedirectToAction("Index");
            }

            var chamCong = await _context.ChamCongs
                .Include(x => x.CaLamViec)
                .FirstOrDefaultAsync(x => x.MaChamCong == id && x.MaNhanVien == nhanVien.MaNhanVien);

            if (chamCong == null)
            {
                TempData["Error"] = "Không tìm thấy bản ghi chấm công.";
                return RedirectToAction("Index");
            }

            if (chamCong.GioRa != null)
            {
                TempData["Error"] = "Bạn đã chấm công ra rồi.";
                return RedirectToAction("Index");
            }

            chamCong.GioRa = DateTime.Now.TimeOfDay;

            if (chamCong.GioVao.HasValue && chamCong.GioRa.HasValue)
            {
                chamCong.SoGioLam = (chamCong.GioRa.Value - chamCong.GioVao.Value).TotalHours;
                if (chamCong.SoGioLam < 0) chamCong.SoGioLam = 0;

                chamCong.GioTangCa = chamCong.SoGioLam > 8 ? chamCong.SoGioLam - 8 : 0;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Chấm công ra thành công.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Manage()
        {
            var list = await _context.ChamCongs
                .Include(x => x.NhanVien)
                .Include(x => x.CaLamViec)
                .OrderByDescending(x => x.NgayLam)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Edit(int id)
        {
            var chamCong = await _context.ChamCongs.FindAsync(id);
            if (chamCong == null) return NotFound();

            ViewBag.CaLamViecs = await _context.CaLamViecs.ToListAsync();
            return View(chamCong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Edit(ChamCong model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CaLamViecs = await _context.CaLamViecs.ToListAsync();
                return View(model);
            }

            if (model.GioVao.HasValue && model.GioRa.HasValue)
            {
                model.SoGioLam = (model.GioRa.Value - model.GioVao.Value).TotalHours;
                if (model.SoGioLam < 0) model.SoGioLam = 0;

                model.GioTangCa = model.SoGioLam > 8 ? model.SoGioLam - 8 : 0;
            }
            else
            {
                model.SoGioLam = 0;
                model.GioTangCa = 0;
            }

            _context.ChamCongs.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật chấm công thành công.";
            return RedirectToAction("Manage");
        }
    }
}