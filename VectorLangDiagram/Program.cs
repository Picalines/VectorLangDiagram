using ElectronNET.API;
using ElectronNET.API.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Electron
builder.WebHost.UseElectron(args);
builder.Services.AddElectron();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if (HybridSupport.IsElectronActive)
{
    Task.Run(async () =>
    {
        var appPath = await Electron.App.GetAppPathAsync();

        var electronWindowOptions = new BrowserWindowOptions()
        {
            Title = "VectorLangDiagram",

            Icon = Path.Join(appPath, "bin", "wwwroot", "icon.png"),

            Center = true,
            MinWidth = 500,
            MinHeight = 400,
        };

        Electron.Menu.SetApplicationMenu(Array.Empty<MenuItem>());

        await Electron.WindowManager.CreateWindowAsync(electronWindowOptions);
    });
}

app.Run();
