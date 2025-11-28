using aLMS.Domain.AccountEntity;
using aLMS.Domain.AnswerEntity;
using aLMS.Domain.BehaviourEntity;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.DepartmentEntity;
using aLMS.Domain.ExerciseEntity;
using aLMS.Domain.LessonEntity;
using aLMS.Domain.ParentProfileEntity;
using aLMS.Domain.PermissionEntity;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.RoleEntity;
using aLMS.Domain.RolePermissionEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.StudentAnswerEntity;
using aLMS.Domain.StudentExerciseEntity;
using aLMS.Domain.StudentProfileEntity;
using aLMS.Domain.SubjectEntity;
using aLMS.Domain.TeacherProfileEntity;
using aLMS.Domain.TopicEntity;
using aLMS.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MediatR; 

namespace aLMS.Infrastructure.Common
{
    public class aLMSDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IPublisher? _publisher;

        // Constructor dùng cho runtime với DI (Dependency Injection)
        public aLMSDbContext(
            DbContextOptions<aLMSDbContext> options,
            IHttpContextAccessor? httpContextAccessor = null,
            IPublisher? publisher = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _publisher = publisher;
        }

        // Constructor dùng cho design-time (EF Core migrations)
        public aLMSDbContext(DbContextOptions<aLMSDbContext> options)
            : base(options)
        {
        }

        // DbSet cho các entity
        public DbSet<School> School { get; set; } = null!;
        public DbSet<Class> Classe { get; set; } = null!;
        public DbSet<Subject> Subject { get; set; } = null!;
        public DbSet<Topic> Topic { get; set; } = null!;
        public DbSet<Lesson> Lessons{ get; set; } = null!;
        public DbSet<Exercise> Exercises{ get; set; } = null!;
        public DbSet<Question> Questions{ get; set; } = null!;
        public DbSet<Answer> Answer { get; set; } = null!;
        public DbSet<StudentProfile> StudentProfile { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<TeacherProfile> TeacherProfile { get; set; } = null!;
        public DbSet<Department> Department { get; set; } = null!;
        public DbSet<ParentProfile> ParentProfile { get; set; } = null!;
        public DbSet<Account> Account { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!;
        public DbSet<Permission> Permission { get; set; } = null!;
        public DbSet<RolePermission> RolePermission { get; set; } = null!;
        public DbSet<StudentExercise> StudentExercise { get; set; } = null!;
        public DbSet<StudentAnswer> StudentAnswer { get; set; } = null!;
        public DbSet<Behaviour> Behaviour { get; set; } = null!;

        // Áp dụng cấu hình entity từ các file configuration trong assembly
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(aLMSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        // Kiểm tra xem có đang xử lý request HTTP không
        private bool IsUserWaitingOnline() => _httpContextAccessor?.HttpContext is not null;

        // Ghi đè SaveChangesAsync để xử lý domain events
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Lấy danh sách domain events từ các entity
            var domainEvents = ChangeTracker.Entries<Entity>()
                .SelectMany(entry => entry.Entity.PopDomainEvents())
                .ToList();

            if (IsUserWaitingOnline())
            {
                // Thêm domain events vào queue để xử lý offline nếu đang trong HTTP context
                //AddDomainEventsToOfflineProcessingQueue(domainEvents);
            }
            else
            {
                // Publish domain events trực tiếp nếu không trong HTTP context
                await PublishDomainEvents(domainEvents);
            }

            // Lưu thay đổi vào database
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Publish domain events thông qua MediatR
        private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
        {
            if (_publisher == null) return;

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken: default);
            }
        }

        // Thêm domain events vào queue để xử lý eventual consistency
        //private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
        //{
        //    if (_httpContextAccessor?.HttpContext == null) return;

        //    // Lấy hoặc tạo queue domain events từ HttpContext
        //    if (!_httpContextAccessor.HttpContext.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) ||
        //        value is not Queue<IDomainEvent> domainEventsQueue)
        //    {
        //        domainEventsQueue = new Queue<IDomainEvent>();
        //    }

        //    // Thêm các domain events vào queue
        //    foreach (var domainEvent in domainEvents)
        //    {
        //        domainEventsQueue.Enqueue(domainEvent);
        //    }

        //    // Lưu queue vào HttpContext
        //    _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
        //}
    }
}