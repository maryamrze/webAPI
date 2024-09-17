using A2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<A2DbContext>(options => options.UseSqlite(builder.Configuration["WebAPIConnection"]));
        builder.Services.AddScoped<IA2Repo, A2Repo>();
        builder.Services.AddControllers();
        builder.Services.AddControllers(options =>
        {
            options.OutputFormatters.Add(new CalendarOutputFormatter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, A2AuthHandler>
            ("MyAuthentication", null);
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("UserOnly",
                policy => policy.RequireClaim(ClaimTypes.Role, "normalUser"));
            options.AddPolicy("OrganizersOnly",
                policy => policy.RequireClaim(ClaimTypes.Role, "Organizer"));
            options.AddPolicy("AuthOnly", policy =>
            {
                policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                (c.Value == "normalUser" || c.Value == "Organizer")));
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();





    }
}