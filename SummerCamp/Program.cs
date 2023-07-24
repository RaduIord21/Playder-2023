using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using SummerCamp.Infrastructure;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(ICoachRepository), typeof(CoachRepository));
builder.Services.AddScoped(typeof(IPlayerRepository), typeof(PlayerRepository));
builder.Services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
builder.Services.AddScoped(typeof(ISponsorRepository), typeof(SponsorRepository));
builder.Services.AddScoped(typeof(ICompetitionRepository), typeof(CompetitionRepository));
builder.Services.AddScoped(typeof(ICompetitionMatchRepository), typeof(CompetitionMatchRepository));
builder.Services.AddScoped(typeof(ICompetitionTeamRepository), typeof(CompetitionTeamRepository));
builder.Services.AddScoped(typeof(ITeamSponsorRepository), typeof(TeamSponsorsRepository));
builder.Services.AddScoped(typeof(IUserCredentialRepository), typeof(UserCredentialRepository));

builder.Services.AddDbContext<SummerCampDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SummerCamp")));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


//Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//session
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

//Session

//session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();