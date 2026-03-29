using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebQLNhanSu.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "HR", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = "admin@gmail.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    var hrEmail = "hr@gmail.com";
    var hr = await userManager.FindByEmailAsync(hrEmail);

    if (hr == null)
    {
        hr = new IdentityUser
        {
            UserName = hrEmail,
            Email = hrEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(hr, "Hr@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(hr, "HR");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(hr, "HR"))
        {
            await userManager.AddToRoleAsync(hr, "HR");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
    {
        var db = context.RequestServices.GetRequiredService<ApplicationDbContext>();
        var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

        var user = await userManager.GetUserAsync(context.User);

        if (user != null)
        {
            var nv = await db.NhanViens.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (nv == null &&
                !context.Request.Path.StartsWithSegments("/Profile/Edit") &&
                !context.Request.Path.StartsWithSegments("/Identity"))
            {
                context.Response.Redirect("/Profile/Edit");
                return;
            }
        }
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();