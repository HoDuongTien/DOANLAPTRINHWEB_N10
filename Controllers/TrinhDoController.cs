using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class TrinhDoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrinhDoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.TrinhDos
                .OrderBy(x => x.MaTrinhDo)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trinhDo = await _context.TrinhDos
                .FirstOrDefaultAsync(x => x.MaTrinhDo == id);

            if (trinhDo == null) return NotFound();

            return View(trinhDo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrinhDo trinhDo)
        {
            if (!ModelState.IsValid) return View(trinhDo);

            bool exists = await _context.TrinhDos
                .AnyAsync(x => x.TenTrinhDo.Trim().ToLower() == trinhDo.TenTrinhDo.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenTrinhDo", "Tên trình độ đã tồn tại.");
                return View(trinhDo);
            }

            try
            {
                _context.Add(trinhDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm trình độ.");
                return View(trinhDo);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trinhDo = await _context.TrinhDos.FindAsync(id);
            if (trinhDo == null) return NotFound();

            return View(trinhDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrinhDo trinhDo)
        {
            if (id != trinhDo.MaTrinhDo) return NotFound();
            if (!ModelState.IsValid) return View(trinhDo);

            bool exists = await _context.TrinhDos
                .AnyAsync(x => x.MaTrinhDo != trinhDo.MaTrinhDo &&
                               x.TenTrinhDo.Trim().ToLower() == trinhDo.TenTrinhDo.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenTrinhDo", "Tên trình độ đã tồn tại.");
                return View(trinhDo);
            }

            var tdDb = await _context.TrinhDos.FindAsync(id);
            if (tdDb == null) return NotFound();

            try
            {
                tdDb.TenTrinhDo = trinhDo.TenTrinhDo;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật trình độ.");
                return View(trinhDo);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trinhDo = await _context.TrinhDos
                .FirstOrDefaultAsync(x => x.MaTrinhDo == id);

            if (trinhDo == null) return NotFound();

            return View(trinhDo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trinhDo = await _context.TrinhDos.FindAsync(id);
            if (trinhDo == null) return NotFound();

            try
            {
                _context.TrinhDos.Remove(trinhDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa vì trình độ đang được nhân viên sử dụng.");
                return View("Delete", trinhDo);
            }
        }
    }
}