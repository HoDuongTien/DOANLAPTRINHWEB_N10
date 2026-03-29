using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class PhongBanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhongBanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.PhongBans
                .OrderBy(x => x.MaPhongBan)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var phongBan = await _context.PhongBans
                .FirstOrDefaultAsync(x => x.MaPhongBan == id);

            if (phongBan == null) return NotFound();

            return View(phongBan);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhongBan phongBan)
        {
            if (!ModelState.IsValid) return View(phongBan);

            bool exists = await _context.PhongBans
                .AnyAsync(x => x.TenPhongBan.Trim().ToLower() == phongBan.TenPhongBan.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenPhongBan", "Tên phòng ban đã tồn tại.");
                return View(phongBan);
            }

            try
            {
                _context.Add(phongBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm phòng ban.");
                return View(phongBan);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var phongBan = await _context.PhongBans.FindAsync(id);
            if (phongBan == null) return NotFound();

            return View(phongBan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhongBan phongBan)
        {
            if (id != phongBan.MaPhongBan) return NotFound();
            if (!ModelState.IsValid) return View(phongBan);

            bool exists = await _context.PhongBans
                .AnyAsync(x => x.MaPhongBan != phongBan.MaPhongBan &&
                               x.TenPhongBan.Trim().ToLower() == phongBan.TenPhongBan.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenPhongBan", "Tên phòng ban đã tồn tại.");
                return View(phongBan);
            }

            var pbDb = await _context.PhongBans.FindAsync(id);
            if (pbDb == null) return NotFound();

            try
            {
                pbDb.TenPhongBan = phongBan.TenPhongBan;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật phòng ban.");
                return View(phongBan);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var phongBan = await _context.PhongBans
                .FirstOrDefaultAsync(x => x.MaPhongBan == id);

            if (phongBan == null) return NotFound();

            return View(phongBan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phongBan = await _context.PhongBans.FindAsync(id);
            if (phongBan == null) return NotFound();

            try
            {
                _context.PhongBans.Remove(phongBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa vì phòng ban đang được nhân viên sử dụng.");
                return View("Delete", phongBan);
            }
        }
    }
}