var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddControllers();

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
app.MapControllerRoute(
    name: "book",
    pattern: "{controller=Books}/{action=GetBookDetails}/{id}"
    );

//app.MapControllerRoute(
//    name: "GetBookDetails",
//    pattern: "{BooksController}/{GetBookDetails}/{key?}");

app.UseSession();

app.UseAuthorization();
app.MapControllers();

app.MapRazorPages();

app.Run();