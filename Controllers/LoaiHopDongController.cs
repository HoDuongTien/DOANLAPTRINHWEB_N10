using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class LoaiHopDongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiHopDongController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.LoaiHopDongs
                .OrderBy(x => x.MaLoaiHopDong)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var loaiHopDong = await _context.LoaiHopDongs
                .FirstOrDefaultAsync(x => x.MaLoaiHopDong == id);

            if (loaiHopDong == null) return NotFound();

            return View(loaiHopDong);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiHopDong loaiHopDong)
        {
            if (!ModelState.IsValid) return View(loaiHopDong);

            bool exists = await _context.LoaiHopDongs
                .AnyAsync(x => x.TenLoaiHopDong.Trim().ToLower() == loaiHopDong.TenLoaiHopDong.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenLoaiHopDong", "Tên loại hợp đồng đã tồn tại.");
                return View(loaiHopDong);
            }

            try
            {
                _context.Add(loaiHopDong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm loại hợp đồng.");
                return View(loaiHopDong);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var loaiHopDong = await _context.LoaiHopDongs.FindAsync(id);
            if (loaiHopDong == null) return NotFound();

            return View(loaiHopDong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoaiHopDong loaiHopDong)
        {
            if (id != loaiHopDong.MaLoaiHopDong) return NotFound();
            if (!ModelState.IsValid) return View(loaiHopDong);

            bool exists = await _context.LoaiHopDongs
                .AnyAsync(x => x.MaLoaiHopDong != loaiHopDong.MaLoaiHopDong &&
                               x.TenLoaiHopDong.Trim().ToLower() == loaiHopDong.TenLoaiHopDong.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenLoaiHopDong", "Tên loại hợp đồng đã tồn tại.");
                return View(loaiHopDong);
            }

            var lhdDb = await _context.LoaiHopDongs.FindAsync(id);
            if (lhdDb == null) return NotFound();

            try
            {
                lhdDb.TenLoaiHopDong = loaiHopDong.TenLoaiHopDong;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật loại hợp đồng.");
                return View(loaiHopDong);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var loaiHopDong = await _context.LoaiHopDongs
                .FirstOrDefaultAsync(x => x.MaLoaiHopDong == id);

            if (loaiHopDong == null) return NotFound();

            return View(loaiHopDong);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiHopDong = await _context.LoaiHopDongs.FindAsync(id);
            if (loaiHopDong == null) return NotFound();

            try
            {
                _context.LoaiHopDongs.Remove(loaiHopDong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa vì loại hợp đồng đang được sử dụng trong hợp đồng.");
                return View("Delete", loaiHopDong);
            }
        }
    }
}