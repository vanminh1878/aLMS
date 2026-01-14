using aLMS.Application.Common.Interfaces;
using aLMS.Application.Common.Jwt;
using aLMS.Application.Common.Mappings;
using aLMS.Infrastructure.AccountInfra;
using aLMS.Infrastructure.AnswerInfra;
using aLMS.Infrastructure.BehaviourInfra;
using aLMS.Infrastructure.ClassInfra;
using aLMS.Infrastructure.ClassSubjectInfra;
using aLMS.Infrastructure.ClassSubjectTeacherInfra;
using aLMS.Infrastructure.Common;
using aLMS.Infrastructure.DepartmentInfra;
using aLMS.Infrastructure.ExerciseInfra;
using aLMS.Infrastructure.LessonInfra;
using aLMS.Infrastructure.NotificationInfra;
using aLMS.Infrastructure.ParentProfileInfra;
using aLMS.Infrastructure.PermissionInfra;
using aLMS.Infrastructure.QuestionInfra;
using aLMS.Infrastructure.RoleInfra;
using aLMS.Infrastructure.RolePermissionInfra;
using aLMS.Infrastructure.SchoolInfra;
using aLMS.Infrastructure.StatisticsInfra;
using aLMS.Infrastructure.StudentAnswerInfra;
using aLMS.Infrastructure.StudentClassEnrollmentInfra;
using aLMS.Infrastructure.StudentEvaluationInfra;
using aLMS.Infrastructure.StudentExerciseInfra;
using aLMS.Infrastructure.StudentProfileInfra;
using aLMS.Infrastructure.StudentQualityEvaluationInfra;
using aLMS.Infrastructure.StudentSubjectCommentInfra;
using aLMS.Infrastructure.SubjectInfra;
using aLMS.Infrastructure.TeacherProfileInfra;
using aLMS.Infrastructure.TimetableInfra;
using aLMS.Infrastructure.TopicInfra;
using aLMS.Infrastructure.UserInfra;
using aLMS.Infrastructure.VirtualClassroomInfra;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                builder.Host.UseSerilog(); 

       
                builder.Services.AddDbContext<aLMSDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            
                builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(aLMS.Application.SchoolServices.Commands.CreateSchool.CreateSchoolCommandHandler).Assembly));

       
                builder.Services.AddHttpContextAccessor();

                builder.Services.AddScoped<IClassSubjectRepository>(provider =>
                    new ClassSubjectRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
                builder.Services.AddScoped<ITimetableRepository>(provider =>
                    new TimetableRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
                builder.Services.AddScoped<IClassSubjectTeacherRepository>(provider =>
                   new ClassSubjectTeacherRepository(
                       provider.GetRequiredService<aLMSDbContext>(),
                       provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
                builder.Services.AddScoped<IUsersRepository>(provider =>
                    new UsersRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<IStudentClassEnrollmentRepository>(provider =>
                    new StudentClassEnrollmentRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<IStatisticsRepository>(provider =>
                    new StatisticsRepository(
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
                builder.Services.AddScoped<IVirtualClassroomRepository>(provider =>
                    new VirtualClassroomRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<INotificationRepository>(provider =>
                    new NotificationRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<IStudentEvaluationRepository>(provider =>
                    new StudentEvaluationRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<IStudentSubjectCommentRepository>(provider =>
                    new StudentSubjectCommentRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));
                builder.Services.AddScoped<IStudentQualityEvaluationRepository>(provider =>
                    new StudentQualityEvaluationRepository(
                        provider.GetRequiredService<aLMSDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                    ));



                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); 
                    });
                });


       
                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

         
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

     
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
                builder.Services.AddScoped<IJwtService, JwtService>();
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>().Value;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtSettings.Issuer,

                            ValidateAudience = true,
                            ValidAudience = jwtSettings.Audience,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMinutes(1)
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                               
                                if (context.Exception is SecurityTokenExpiredException)
                                    context.Response.Headers.Add("Token-Expired", "true");

                                Log.Error(context.Exception, "JWT Authentication failed");
                                return Task.CompletedTask;
                            },
                            OnChallenge = context =>
                            {
                                context.HandleResponse();
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = System.Text.Json.JsonSerializer.Serialize(new { error = "Unauthorized" });
                                return context.Response.WriteAsync(result);
                            }
                        };
                    });

                builder.Services.AddAuthorization();

                var app = builder.Build();

               
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