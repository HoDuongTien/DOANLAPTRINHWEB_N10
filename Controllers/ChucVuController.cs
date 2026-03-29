using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class ChucVuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChucVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.ChucVus
                .OrderBy(x => x.MaChucVu)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(x => x.MaChucVu == id);

            if (chucVu == null) return NotFound();

            return View(chucVu);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChucVu chucVu)
        {
            if (chucVu.LuongCoBan < 0)
            {
                ModelState.AddModelError("LuongCoBan", "Lương cơ bản phải lớn hơn hoặc bằng 0.");
            }

            if (!ModelState.IsValid) return View(chucVu);

            bool exists = await _context.ChucVus
                .AnyAsync(x => x.TenChucVu.Trim().ToLower() == chucVu.TenChucVu.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenChucVu", "Tên chức vụ đã tồn tại.");
                return View(chucVu);
            }

            try
            {
                _context.Add(chucVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm chức vụ.");
                return View(chucVu);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null) return NotFound();

            return View(chucVu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChucVu chucVu)
        {
            if (id != chucVu.MaChucVu) return NotFound();

            if (chucVu.LuongCoBan < 0)
            {
                ModelState.AddModelError("LuongCoBan", "Lương cơ bản phải lớn hơn hoặc bằng 0.");
            }

            if (!ModelState.IsValid) return View(chucVu);

            bool exists = await _context.ChucVus
                .AnyAsync(x => x.MaChucVu != chucVu.MaChucVu &&
                               x.TenChucVu.Trim().ToLower() == chucVu.TenChucVu.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("TenChucVu", "Tên chức vụ đã tồn tại.");
                return View(chucVu);
            }

            var cvDb = await _context.ChucVus.FindAsync(id);
            if (cvDb == null) return NotFound();

            try
            {
                cvDb.TenChucVu = chucVu.TenChucVu;
                cvDb.LuongCoBan = chucVu.LuongCoBan;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật chức vụ.");
                return View(chucVu);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(x => x.MaChucVu == id);

            if (chucVu == null) return NotFound();

            return View(chucVu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null) return NotFound();

            try
            {
                _context.ChucVus.Remove(chucVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa vì chức vụ đang được nhân viên sử dụng.");
                return View("Delete", chucVu);
            }
        }
    }
}