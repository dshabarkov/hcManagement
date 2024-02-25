using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using HumanCapitalManagement.Services;
using HumanCapitalManagement.Services.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseMetricsWebTracking()
    .UseMetrics(options =>
    {
        options.EndpointOptions = endpointsOptions =>
        {
            endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
            endpointsOptions.EnvironmentInfoEndpointEnabled = false;
        };
    });

// Add services to the container.
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.AddMetrics();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//builder.Services.AddHttpClient<IDataService, DataService>(c =>
//        c.BaseAddress = new Uri("https://localhost:44345/"));
//builder.Services.AddHttpClient<ILoginService, LoginService>(c =>
//        c.BaseAddress = new Uri("https://localhost:44305/"));

builder.Services.AddHttpClient<IDataService, DataService>(c =>
        c.BaseAddress = new Uri("http://humancapitalmanagement-dataapi-1:80/"));
builder.Services.AddHttpClient<ILoginService, LoginService>(c =>
        c.BaseAddress = new Uri("http://humancapitalmanagement-loginapi-1:80/"));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        if (context.Response.Headers.ContainsKey("Cache-Control"))
        {
            context.Response.Headers["Cache-Control"] = "no-cache,no-store";
        }
        else
        {
            context.Response.Headers.Add("Cache-Control", "no-cache,no-store");
        }
        if (context.Response.Headers.ContainsKey("Pragma"))
        {
            context.Response.Headers["Pragma"] = "no-cache";
        }
        else
        {
            context.Response.Headers.Add("Pragma", "no-cache");
        }
        return Task.FromResult(0);
    });
    await next.Invoke();
});

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}");

app.Run();