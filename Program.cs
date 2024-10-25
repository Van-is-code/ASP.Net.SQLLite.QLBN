var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cho phép cập nhật lại trang web nếu tệp *.cshtml bị sửa nội dung
// https://stackoverflow.com/questions/53639969/net-core-mvc-page-not-refreshing-after-changes#answer-65805255
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-8.0
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminPatient}/{action=Index}/{id?}");

app.Run();
// 2024.10.02 14h49: Tệp mã nguồn này về cơ bản cũng không phải động chạm gì nhiều
// giữ nguyên như khi nó được tạo ra từ dotnet new mvc