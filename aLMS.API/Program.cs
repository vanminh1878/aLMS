using aLMS.Application.Common.Interfaces;
using aLMS.Infrastructure.AccountInfra;
using aLMS.Infrastructure.AnswerInfra;
using aLMS.Infrastructure.BehaviourInfra;
using aLMS.Infrastructure.ClassInfra;
using aLMS.Infrastructure.Common;
using aLMS.Infrastructure.DepartmentInfra;
using aLMS.Infrastructure.ExerciseInfra;
using aLMS.Infrastructure.GradeInfra;
using aLMS.Infrastructure.LessonInfra;
using aLMS.Infrastructure.ParentProfileInfra;
using aLMS.Infrastructure.PermissionInfra;
using aLMS.Infrastructure.QuestionInfra;
using aLMS.Infrastructure.RoleInfra;
using aLMS.Infrastructure.RolePermissionInfra;
using aLMS.Infrastructure.SchoolInfra;
using aLMS.Infrastructure.StudentAnswerInfra;
using aLMS.Infrastructure.StudentExerciseInfra;
using aLMS.Infrastructure.StudentProfileInfra;
using aLMS.Infrastructure.SubjectInfra;
using aLMS.Infrastructure.TeacherProfileInfra;
using aLMS.Infrastructure.TopicInfra;
using aLMS.Infrastructure.UserInfra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Text;


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
                builder.Services.AddScoped<IUsersRepository, UsersRepository>();
                builder.Services.AddScoped<IRoleRepository, RoleRepository>();
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
                builder.Services.AddScoped<IGradeRepository, GradeRepository>();
                builder.Services.AddScoped<IClassRepository, ClassRepository>();
                builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
                builder.Services.AddScoped<ITopicRepository, TopicRepository>();
                builder.Services.AddScoped<ILessonRepository, LessonRepository>();
                builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
                builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
                builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
                builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
                builder.Services.AddScoped<ITeacherProfileRepository, TeacherProfileRepository>();
                builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
                builder.Services.AddScoped<IParentProfileRepository, ParentProfileRepository>();
                builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
                builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
                builder.Services.AddScoped<IStudentExerciseRepository, StudentExerciseRepository>();
                builder.Services.AddScoped<IStudentAnswerRepository, StudentAnswerRepository>();
                builder.Services.AddScoped<IBehaviourRepository, BehaviourRepository>();
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