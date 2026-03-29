using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class CaLamViecController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaLamViecController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.CaLamViecs
                .OrderBy(x => x.MaCa)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var caLamViec = await _context.CaLamViecs
                .FirstOrDefaultAsync(x => x.MaCa == id);

            if (caLamViec == null) return NotFound();

            return View(caLamViec);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaLamViec caLamViec)
        {
            if (caLamViec.GioKetThuc <= caLamViec.GioBatDau)
            {
                ModelState.AddModelError("GioKetThuc", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            if (!ModelState.IsValid) return View(caLamViec);

            bool exists = await _context.CaLamViecs
                .AnyAsync(x => x.TenCa.Trim().ToLower() == caLamViec.TenCa.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenCa", "Tên ca làm việc đã tồn tại.");
                return View(caLamViec);
            }

            try
            {
                _context.Add(caLamViec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm ca làm việc.");
                return View(caLamViec);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var caLamViec = await _context.CaLamViecs.FindAsync(id);
            if (caLamViec == null) return NotFound();

            return View(caLamViec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaLamViec caLamViec)
        {
            if (id != caLamViec.MaCa) return NotFound();

            if (caLamViec.GioKetThuc <= caLamViec.GioBatDau)
            {
                ModelState.AddModelError("GioKetThuc", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            if (!ModelState.IsValid) return View(caLamViec);

            bool exists = await _context.CaLamViecs
                .AnyAsync(x => x.MaCa != caLamViec.MaCa &&
                               x.TenCa.Trim().ToLower() == caLamViec.TenCa.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenCa", "Tên ca làm việc đã tồn tại.");
                return View(caLamViec);
            }

            var caDb = await _context.CaLamViecs.FindAsync(id);
            if (caDb == null) return NotFound();

            try
            {
                caDb.TenCa = caLamViec.TenCa;
                caDb.GioBatDau = caLamViec.GioBatDau;
                caDb.GioKetThuc = caLamViec.GioKetThuc;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật ca làm việc.");
                return View(caLamViec);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var caLamViec = await _context.CaLamViecs
                .FirstOrDefaultAsync(x => x.MaCa == id);

            if (caLamViec == null) return NotFound();

            return View(caLamViec);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caLamViec = await _context.CaLamViecs.FindAsync(id);
            if (caLamViec == null) return NotFound();

            try
            {
                _context.CaLamViecs.Remove(caLamViec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa vì ca làm việc đang được dùng trong chấm công.");
                return View("Delete", caLamViec);
            }
        }
    }
}