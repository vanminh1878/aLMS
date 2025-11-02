using aLMS.Application.Common.Interfaces;
using aLMS.Application.Common.Jwt;
using aLMS.Application.Common.Mappings;
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

                // Đăng ký MediatR - đăng ký assembly chứa các handlers (aLMS.Application)
                builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(aLMS.Application.SchoolServices.Commands.CreateSchool.CreateSchoolCommandHandler).Assembly));

                // Đăng ký IHttpContextAccessor
                builder.Services.AddHttpContextAccessor();

                // Đăng ký các repository với DI, truyền aLMSDbContext và connectionString
                builder.Services.AddScoped<IUsersRepository>(provider =>
                    new UsersRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IRoleRepository>(provider =>
                    new RoleRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IAccountRepository>(provider =>
                    new AccountRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<ISchoolRepository>(provider =>
                    new SchoolRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IGradeRepository>(provider =>
                    new GradeRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IClassRepository>(provider =>
                    new ClassRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<ISubjectRepository>(provider =>
                    new SubjectRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<ITopicRepository>(provider =>
                    new TopicRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<ILessonRepository>(provider =>
                    new LessonRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IExerciseRepository>(provider =>
                    new ExerciseRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IQuestionRepository>(provider =>
                    new QuestionRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IAnswerRepository>(provider =>
                    new AnswerRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IStudentProfileRepository>(provider =>
                    new StudentProfileRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<ITeacherProfileRepository>(provider =>
                    new TeacherProfileRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IDepartmentRepository>(provider =>
                    new DepartmentRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IParentProfileRepository>(provider =>
                    new ParentProfileRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IPermissionRepository>(provider =>
                    new PermissionRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IRolePermissionRepository>(provider =>
                    new RolePermissionRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IStudentExerciseRepository>(provider =>
                    new StudentExerciseRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IStudentAnswerRepository>(provider =>
                    new StudentAnswerRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));

                builder.Services.AddScoped<IBehaviourRepository>(provider =>
                    new BehaviourRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                //builder.Services.AddScoped<IAuthService, AuthService>();
                //builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


                // Thêm CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000") // FE của bạn
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // nếu dùng JWT trong cookie/localStorage
                    });
                });


                // Thêm localization
                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                // Thêm controllers và Swagger
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

                // Đăng ký JwtSettings
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
                builder.Services.AddScoped<IJwtService, JwtService>();

                // Đăng ký Authentication + Authorization
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Log.Error("JWT Authentication failed: {Message}", context.Exception.Message);
                                return Task.CompletedTask;
                            }
                        };
                    });

                builder.Services.AddAuthorization();

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

                app.UseCors("AllowReactApp");
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