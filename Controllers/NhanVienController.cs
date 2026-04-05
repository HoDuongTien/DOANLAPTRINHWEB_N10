using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;
using WebQLNhanSu.Models;

namespace WebQLNhanSu.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*public async Task<IActionResult> Index()
        {
            var data = await _context.NhanViens
                .Include(x => x.PhongBan)
                .Include(x => x.ChucVu)
                .Include(x => x.TrinhDo)
                .ToListAsync();

            return View(data);
        } */
        public async Task<IActionResult> Index(string search)
       {
            var query = _context.NhanViens
               .Include(x => x.PhongBan)
               .Include(x => x.ChucVu)
               .Include(x => x.TrinhDo)
               .AsQueryable();

             if (!string.IsNullOrWhiteSpace(search))
         {
             query = query.Where(x =>
             x.TenNhanVien.Contains(search) ||
             x.SoDienThoai.Contains(search) ||
             x.DiaChi.Contains(search) ||
             x.PhongBan.TenPhongBan.Contains(search)
         );
    }

    ViewBag.Search = search;
    return View(await query.ToListAsync());
}

        public async Task<IActionResult> Details(int id)
        {
            var nv = await _context.NhanViens
                .Include(x => x.PhongBan)
                .Include(x => x.ChucVu)
                .Include(x => x.TrinhDo)
                .FirstOrDefaultAsync(x => x.MaNhanVien == id);

            if (nv == null) return NotFound();

            return View(nv);
        }

        public IActionResult Create()
        {
            LoadDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nv)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown();
                return View(nv);
            }

            try
            {
                _context.Add(nv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể thêm dữ liệu");
                LoadDropdown();
                return View(nv);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            LoadDropdown(nv);
            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien nv)
        {
            if (id != nv.MaNhanVien) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdown(nv);
                return View(nv);
            }

            var nvDb = await _context.NhanViens.FindAsync(id);
            if (nvDb == null) return NotFound();

            try
            {
                nvDb.TenNhanVien = nv.TenNhanVien;
                nvDb.NgaySinh = nv.NgaySinh;
                nvDb.GioiTinh = nv.GioiTinh;
                nvDb.SoDienThoai = nv.SoDienThoai;
                nvDb.DiaChi = nv.DiaChi;
                nvDb.MaPhongBan = nv.MaPhongBan;
                nvDb.MaChucVu = nv.MaChucVu;
                nvDb.MaTrinhDo = nv.MaTrinhDo;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "Không thể cập nhật dữ liệu");
                LoadDropdown(nv);
                return View(nv);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nv = await _context.NhanViens
                .Include(x => x.PhongBan)
                .Include(x => x.ChucVu)
                .Include(x => x.TrinhDo)
                .FirstOrDefaultAsync(x => x.MaNhanVien == id);

            if (nv == null) return NotFound();

            return View(nv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            try
            {
                _context.NhanViens.Remove(nv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                var nvReload = await _context.NhanViens
                    .Include(x => x.PhongBan)
                    .Include(x => x.ChucVu)
                    .Include(x => x.TrinhDo)
                    .FirstOrDefaultAsync(x => x.MaNhanVien == id);

                if (nvReload == null) return NotFound();

                ModelState.AddModelError("", "Không thể xóa vì nhân viên đang có dữ liệu liên quan (hợp đồng, chấm công, lương...)");
                return View("Delete", nvReload);
            }
        }

        private void LoadDropdown(NhanVien? nv = null)
        {
            ViewBag.PhongBan = new SelectList(_context.PhongBans, "MaPhongBan", "TenPhongBan", nv?.MaPhongBan);
            ViewBag.ChucVu = new SelectList(_context.ChucVus, "MaChucVu", "TenChucVu", nv?.MaChucVu);
            ViewBag.TrinhDo = new SelectList(_context.TrinhDos, "MaTrinhDo", "TenTrinhDo", nv?.MaTrinhDo);
            var gioiTinhs = new List<string> { "Nam", "Nữ", "Khác" };
            ViewBag.GioiTinh = new SelectList(gioiTinhs, nv?.GioiTinh);

        }
    }
}