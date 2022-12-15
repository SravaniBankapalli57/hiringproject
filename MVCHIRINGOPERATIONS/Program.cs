using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();
//session prop start
builder.Services.AddSession
    (
    options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(60);
        options.Cookie.IsEssential = true;
    }
    );
builder.Services.AddHttpContextAccessor();
//session prop end
//for output cache
//builder.Services.AddOutputCaching();
//authentication
builder.Services.AddAuthentication();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseOutputCaching();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();