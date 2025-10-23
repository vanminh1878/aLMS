using System.Globalization;
using System.Text;
using aLMS.Infrastructure.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;


namespace aLMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cấu hình Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog(); // Sử dụng Serilog thay cho logging mặc định

                // Đăng ký DbContext
                builder.Services.AddDbContext<aLMSDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

                // Đăng ký MediatR
                builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(aLMSDbContext).Assembly));

                // Đăng ký IHttpContextAccessor
                builder.Services.AddHttpContextAccessor();

                // Đăng ký các repository
                //builder.Services.AddScoped<IUsersRepository, UsersRepository>();
                //builder.Services.AddScoped<IRolesRepository, RolesRepository>();
                //builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
                //builder.Services.AddScoped<ISchoolsRepository, SchoolsRepository>();
                //builder.Services.AddScoped<IGradesRepository, GradesRepository>();
                //builder.Services.AddScoped<IClassesRepository, ClassesRepository>();
                //builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
                //builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
                //builder.Services.AddScoped<ILessonsRepository, LessonsRepository>();
                //builder.Services.AddScoped<IExercisesRepository, ExercisesRepository>();
                //builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();
                //builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();
                //builder.Services.AddScoped<IStudentProfilesRepository, StudentProfilesRepository>();
                //builder.Services.AddScoped<ITeacherProfilesRepository, TeacherProfilesRepository>();
                //builder.Services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
                //builder.Services.AddScoped<IParentProfilesRepository, ParentProfilesRepository>();
                //builder.Services.AddScoped<IPermissionsRepository, PermissionsRepository>();
                //builder.Services.AddScoped<IRolePermissionsRepository, RolePermissionsRepository>();
                //builder.Services.AddScoped<IStudentExercisesRepository, StudentExercisesRepository>();
                //builder.Services.AddScoped<IStudentAnswersRepository, StudentAnswersRepository>();
                //builder.Services.AddScoped<IBehavioursRepository, BehavioursRepository>();
                //builder.Services.AddScoped<IAuthService, AuthService>();
                //builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

                // Thêm CORS cho Angular frontend
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAngularApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
                });

                // Thêm authentication với JWT
                //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //    .AddJwtBearer(options =>
                //    {
                //        options.TokenValidationParameters = new TokenValidationParameters
                //        {
                //            ValidateIssuer = true,
                //            ValidateAudience = true,
                //            ValidateLifetime = true,
                //            ValidateIssuerSigningKey = true,
                //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                //            ValidAudience = builder.Configuration["Jwt:Audience"],
                //            IssuerSigningKey = new SymmetricSecurityKey(
                //                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                //        };
                //        options.Events = new JwtBearerEvents
                //        {
                //            OnAuthenticationFailed = context =>
                //            {
                //                Log.Error("Authentication failed: {Message}", context.Exception.Message);
                //                return Task.CompletedTask;
                //            }
                //        };
                //    });

                // Thêm localization
                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                // Thêm controllers và Swagger
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Cấu hình middleware
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture("vi-VN"),
                    SupportedCultures = new[] { new CultureInfo("vi-VN") },
                    SupportedUICultures = new[] { new CultureInfo("vi-VN") }
                });

                app.UseCors("AllowAngularApp");
                app.UseAuthentication();
                app.UseAuthorization();
                //app.UseMiddleware<EventualConsistencyMiddleware>();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}