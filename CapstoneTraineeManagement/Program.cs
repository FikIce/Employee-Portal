using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using CapstoneTraineeManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// This line reads the "SmtpSettings" section from your appsettings.json
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// ---- Interface Bindings ----
builder.Services.AddScoped<ILookUpService, LookUpService>();
builder.Services.AddScoped<IEnrollmentLogService, EnrollmentLogService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<ITraineeService, TraineeService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddTransient<IEmailService, EmailService>();

// --- Connection String ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();