using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.DataSeed;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GymManagementSystem.BLL
{
    public static class GymServiceExtensions
    {
        public static IServiceCollection AddGymServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();

            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

            return services;
        }

        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            dbContext.Database.EnsureCreated();
            GymDataSeeder.Seed(dbContext);
        }
    }
}

