using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebQLNhanSu.Migrations
{
    /// <inheritdoc />
    public partial class InitClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaLamViecs",
                columns: table => new
                {
                    MaCa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioBatDau = table.Column<TimeSpan>(type: "time", nullable: false),
                    GioKetThuc = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViecs", x => x.MaCa);
                    table.CheckConstraint("CK_CaLamViec_Gio", "[GioKetThuc] > [GioBatDau]");
                });

            migrationBuilder.CreateTable(
                name: "CauHinhLuongs",
                columns: table => new
                {
                    MaCauHinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LuongCoBanMacDinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HeSoTangCa = table.Column<double>(type: "float", nullable: false),
                    PhuCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHinhLuongs", x => x.MaCauHinh);
                });

            migrationBuilder.CreateTable(
                name: "ChucVus",
                columns: table => new
                {
                    MaChucVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChucVu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LuongCoBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVus", x => x.MaChucVu);
                    table.CheckConstraint("CK_ChucVu_Luong", "[LuongCoBan] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "LoaiHopDongs",
                columns: table => new
                {
                    MaLoaiHopDong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiHopDong = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiHopDongs", x => x.MaLoaiHopDong);
                });

            migrationBuilder.CreateTable(
                name: "PhongBans",
                columns: table => new
                {
                    MaPhongBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhongBan = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongBans", x => x.MaPhongBan);
                });

            migrationBuilder.CreateTable(
                name: "TrinhDos",
                columns: table => new
                {
                    MaTrinhDo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTrinhDo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDos", x => x.MaTrinhDo);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaPhongBan = table.Column<int>(type: "int", nullable: false),
                    MaChucVu = table.Column<int>(type: "int", nullable: false),
                    MaTrinhDo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                    table.CheckConstraint("CK_NV_GioiTinh", "[GioiTinh] IN (N'Nam', N'Nữ', N'Khác')");
                    table.ForeignKey(
                        name: "FK_NhanViens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NhanViens_ChucVus_MaChucVu",
                        column: x => x.MaChucVu,
                        principalTable: "ChucVus",
                        principalColumn: "MaChucVu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhanViens_PhongBans_MaPhongBan",
                        column: x => x.MaPhongBan,
                        principalTable: "PhongBans",
                        principalColumn: "MaPhongBan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhanViens_TrinhDos_MaTrinhDo",
                        column: x => x.MaTrinhDo,
                        principalTable: "TrinhDos",
                        principalColumn: "MaTrinhDo",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BangLuongs",
                columns: table => new
                {
                    MaBangLuong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: false),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    LuongCoBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Thuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KhauTru = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[LuongCoBan] + [Thuong] - [KhauTru]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangLuongs", x => x.MaBangLuong);
                    table.CheckConstraint("CK_BL_Nam", "[Nam] >= 2000");
                    table.CheckConstraint("CK_BL_Thang", "[Thang] BETWEEN 1 AND 12");
                    table.CheckConstraint("CK_BL_Tien", "[LuongCoBan] >= 0 AND [Thuong] >= 0 AND [KhauTru] >= 0");
                    table.ForeignKey(
                        name: "FK_BangLuongs_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChamCongs",
                columns: table => new
                {
                    MaChamCong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: false),
                    MaCa = table.Column<int>(type: "int", nullable: false),
                    NgayLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioVao = table.Column<TimeSpan>(type: "time", nullable: true),
                    GioRa = table.Column<TimeSpan>(type: "time", nullable: true),
                    SoGioLam = table.Column<double>(type: "float", nullable: false),
                    GioTangCa = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChamCongs", x => x.MaChamCong);
                    table.CheckConstraint("CK_ChamCong_Gio", "[SoGioLam] >= 0");
                    table.CheckConstraint("CK_ChamCong_OT", "[GioTangCa] >= 0");
                    table.ForeignKey(
                        name: "FK_ChamCongs_CaLamViecs_MaCa",
                        column: x => x.MaCa,
                        principalTable: "CaLamViecs",
                        principalColumn: "MaCa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChamCongs_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HopDongs",
                columns: table => new
                {
                    MaHopDong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: false),
                    MaLoaiHopDong = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Luong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Đang hiệu lực")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDongs", x => x.MaHopDong);
                    table.CheckConstraint("CK_HopDong_Ngay", "[NgayKetThuc] IS NULL OR [NgayKetThuc] >= [NgayBatDau]");
                    table.ForeignKey(
                        name: "FK_HopDongs_LoaiHopDongs_MaLoaiHopDong",
                        column: x => x.MaLoaiHopDong,
                        principalTable: "LoaiHopDongs",
                        principalColumn: "MaLoaiHopDong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopDongs_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSuNguoiDungs",
                columns: table => new
                {
                    MaLichSu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    HanhDong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuNguoiDungs", x => x.MaLichSu);
                    table.ForeignKey(
                        name: "FK_LichSuNguoiDungs_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NghiPheps",
                columns: table => new
                {
                    MaNghiPhep = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrangThaiNghiPhep = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaNhanVien = table.Column<int>(type: "int", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NghiPheps", x => x.MaNghiPhep);
                    table.CheckConstraint("CK_NghiPhep_Ngay", "[DenNgay] >= [TuNgay]");
                    table.ForeignKey(
                        name: "FK_NghiPheps_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CaLamViecs",
                columns: new[] { "MaCa", "GioBatDau", "GioKetThuc", "TenCa" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 8, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), "Ca Sáng" },
                    { 2, new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 21, 0, 0, 0), "Ca Chiều" },
                    { 3, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 8, 0, 0, 0), "Ca Đêm" },
                    { 4, new TimeSpan(0, 8, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), "Ca Hành Chính" }
                });

            migrationBuilder.InsertData(
                table: "CauHinhLuongs",
                columns: new[] { "MaCauHinh", "HeSoTangCa", "LuongCoBanMacDinh", "PhuCap" },
                values: new object[,]
                {
                    { 1, 2.2000000000000002, 100000m, 50000m },
                    { 2, 1.3, 200000m, 100000m },
                    { 3, 1.3999999999999999, 300000m, 150000m },
                    { 4, 2.5, 400000m, 200000m }
                });

            migrationBuilder.InsertData(
                table: "ChucVus",
                columns: new[] { "MaChucVu", "LuongCoBan", "TenChucVu" },
                values: new object[,]
                {
                    { 1, 5000000m, "Nhân Viên" },
                    { 2, 8000000m, "Trưởng Phòng" },
                    { 3, 15000000m, "Giám Đốc" },
                    { 4, 3000000m, "Thực Tập Sinh" },
                    { 5, 12000000m, "Phó Giám Đốc" }
                });

            migrationBuilder.InsertData(
                table: "LoaiHopDongs",
                columns: new[] { "MaLoaiHopDong", "TenLoaiHopDong" },
                values: new object[,]
                {
                    { 1, "Hợp Đồng Thử Việc" },
                    { 2, "Hợp Đồng Chính Thức" },
                    { 3, "Hợp Đồng Thời Vụ" },
                    { 4, "Hợp Đồng Cộng Tác Viên" },
                    { 5, "Hợp Đồng Thực Tập" }
                });

            migrationBuilder.InsertData(
                table: "PhongBans",
                columns: new[] { "MaPhongBan", "TenPhongBan" },
                values: new object[,]
                {
                    { 1, "Phòng Nhân Sự" },
                    { 2, "Phòng Kỹ Thuật" },
                    { 3, "Phòng Kinh Doanh" },
                    { 4, "Phòng Tài Chính" },
                    { 5, "Phòng Marketing" },
                    { 6, "Phòng Hành Chính" }
                });

            migrationBuilder.InsertData(
                table: "TrinhDos",
                columns: new[] { "MaTrinhDo", "TenTrinhDo" },
                values: new object[,]
                {
                    { 1, "Cao Đẳng" },
                    { 2, "Đại Học" },
                    { 3, "Thạc Sĩ" },
                    { 4, "Tiến Sĩ" },
                    { 5, "Không Có" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BangLuongs_MaNhanVien_Thang_Nam",
                table: "BangLuongs",
                columns: new[] { "MaNhanVien", "Thang", "Nam" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChamCongs_MaCa",
                table: "ChamCongs",
                column: "MaCa");

            migrationBuilder.CreateIndex(
                name: "IX_ChamCongs_MaNhanVien",
                table: "ChamCongs",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChucVus_TenChucVu",
                table: "ChucVus",
                column: "TenChucVu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_MaLoaiHopDong",
                table: "HopDongs",
                column: "MaLoaiHopDong");

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_MaNhanVien",
                table: "HopDongs",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNguoiDungs_MaNhanVien",
                table: "LichSuNguoiDungs",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiHopDongs_TenLoaiHopDong",
                table: "LoaiHopDongs",
                column: "TenLoaiHopDong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NghiPheps_MaNhanVien",
                table: "NghiPheps",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaChucVu",
                table: "NhanViens",
                column: "MaChucVu");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaPhongBan",
                table: "NhanViens",
                column: "MaPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaTrinhDo",
                table: "NhanViens",
                column: "MaTrinhDo");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_SoDienThoai",
                table: "NhanViens",
                column: "SoDienThoai",
                unique: true,
                filter: "[SoDienThoai] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_UserId",
                table: "NhanViens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhongBans_TenPhongBan",
                table: "PhongBans",
                column: "TenPhongBan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrinhDos_TenTrinhDo",
                table: "TrinhDos",
                column: "TenTrinhDo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BangLuongs");

            migrationBuilder.DropTable(
                name: "CauHinhLuongs");

            migrationBuilder.DropTable(
                name: "ChamCongs");

            migrationBuilder.DropTable(
                name: "HopDongs");

            migrationBuilder.DropTable(
                name: "LichSuNguoiDungs");

            migrationBuilder.DropTable(
                name: "NghiPheps");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CaLamViecs");

            migrationBuilder.DropTable(
                name: "LoaiHopDongs");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChucVus");

            migrationBuilder.DropTable(
                name: "PhongBans");

            migrationBuilder.DropTable(
                name: "TrinhDos");
        }
    }
}
