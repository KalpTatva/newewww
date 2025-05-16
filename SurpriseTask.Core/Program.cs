using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurpriseTask.Repository.Implementation;
using SurpriseTask.Repository.Interfaces;
using SurpriseTask.Repository.Models;
using SurpriseTask.Service.Implementation;
using SurpriseTask.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseServices, CourseService>();

// Repository   
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));

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



// Handle 404s by redirecting to /Home/Privacy
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/Error404");
    }
    await Task.CompletedTask;
});



// Map routes with authorization
app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
        if (context.Request.Cookies.ContainsKey("auth_token"))
        {
            var token = context.Request.Cookies["auth_token"];

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out _);
                context.User = principal;

                var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
                
                if(!string.IsNullOrEmpty(emailClaim) && !string.IsNullOrEmpty(roleClaim)) {
                    context.Items["UserEmail"] = emailClaim;
                    context.Items["UserRole"] = roleClaim;
                    
                }else{
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }

            if (context.User.Identity != null &&!context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
        }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
