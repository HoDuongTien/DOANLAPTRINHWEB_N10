using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebQLNhanSu.Data;
using WebQLNhanSu.Enums;
using WebQLNhanSu.Models;
using WebQLNhanSu.Services;

namespace WebQLNhanSu.Controllers
{
    [Authorize]
    public class NghiPhepController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public NghiPhepController(
            ApplicationDbContext context,
            IEmailSender emailSender,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nhanVien == null)
            {
                TempData["Error"] = "Ban chua co ho so nhan vien.";
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
                TempData["Error"] = "Ban chua co ho so nhan vien.";
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
                ModelState.AddModelError("", "Den ngay phai lon hon hoac bang tu ngay.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.TrangThaiNghiPhep = TrangThaiNghiPhep.ChoDuyet;

            _context.NghiPheps.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Gui don nghi phep thanh cong.";
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
            var nghiPhep = await _context.NghiPheps
                .Include(x => x.NhanVien)
                .FirstOrDefaultAsync(x => x.MaNghiPhep == id);

            if (nghiPhep == null)
            {
                return NotFound();
            }

            nghiPhep.TrangThaiNghiPhep = TrangThaiNghiPhep.DaDuyet;
            await _context.SaveChangesAsync();

            await SendLeaveStatusEmailAsync(nghiPhep, "Don nghi phep da duoc duyet");

            TempData["Success"] = "Da duyet don nghi phep.";
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> TuChoi(int id)
        {
            var nghiPhep = await _context.NghiPheps
                .Include(x => x.NhanVien)
                .FirstOrDefaultAsync(x => x.MaNghiPhep == id);

            if (nghiPhep == null)
            {
                return NotFound();
            }

            nghiPhep.TrangThaiNghiPhep = TrangThaiNghiPhep.TuChoi;
            await _context.SaveChangesAsync();

            await SendLeaveStatusEmailAsync(nghiPhep, "Don nghi phep cua ban da bi tu choi");

            TempData["Success"] = "Da tu choi don nghi phep.";
            return RedirectToAction("Manage");
        }

        private async Task SendLeaveStatusEmailAsync(NghiPhep nghiPhep, string subject)
        {
            if (nghiPhep.NhanVien == null || string.IsNullOrEmpty(nghiPhep.NhanVien.UserId))
            {
                return;
            }

            var user = await _userManager.FindByIdAsync(nghiPhep.NhanVien.UserId);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return;
            }

            var body = $@"
                <h3>Thong bao tu he thong quan ly nhan su</h3>
                <p>Xin chao <b>{nghiPhep.NhanVien.TenNhanVien}</b>,</p>
                <p>{subject}.</p>
                <p>Thoi gian nghi: {nghiPhep.TuNgay:dd/MM/yyyy} - {nghiPhep.DenNgay:dd/MM/yyyy}</p>
                <p>Vui long dang nhap he thong de xem chi tiet.</p>";

            try
            {
                await _emailSender.SendEmailAsync(user.Email, subject, body);
            }
            catch
            {
            }
        }
    }
}
