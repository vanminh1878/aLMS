using aLMS.Application.Common.Dtos;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.AnswerEntity;
using aLMS.Domain.BehaviourEntity;
using aLMS.Domain.ClassEntity;
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
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aLMS.Application.Common.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //School
            CreateMap<School, SchoolDto>();
            CreateMap<CreateSchoolDto, School>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty)); // Thay Ignore() tạm thời
            CreateMap<UpdateSchoolDto, School>();
            // Class
            CreateMap<Class, ClassDto>();
            CreateMap<CreateClassDto, Class>();
            CreateMap<UpdateClassDto, Class>();

           
            // Subject
            CreateMap<Subject, SubjectDto>();
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<UpdateSubjectDto, Subject>();

            // Topic
            CreateMap<Topic, TopicDto>();
            CreateMap<CreateTopicDto, Topic>();
            CreateMap<UpdateTopicDto, Topic>();

            // Lesson
            CreateMap<Lesson, LessonDto>();
            CreateMap<CreateLessonDto, Lesson>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateLessonDto, Lesson>();

            //Exercise
            CreateMap<Exercise, ExerciseDto>();

            CreateMap<CreateExerciseDto, Exercise>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<UpdateExerciseDto, Exercise>();
            // Question
            CreateMap<Question, QuestionDto>();
            CreateMap<CreateQuestionDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());
            CreateMap<UpdateQuestionDto, Question>()
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            // Answer
            CreateMap<Answer, AnswerDto>();
            CreateMap<CreateAnswerDto, Answer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionId, opt => opt.Ignore());
            CreateMap<UpdateAnswerDto, Answer>()
                .ForMember(dest => dest.QuestionId, opt => opt.Ignore());

            //StudentExercise
            CreateMap<StudentExercise, StudentExerciseDto>()
                .ForMember(dest => dest.ExerciseTitle, opt => opt.MapFrom(src => src.Exercise.Title));

            CreateMap<StudentAnswer, StudentAnswerDto>();

            // Role
            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleDto, Role>().ForMember(d => d.Id, o => o.Ignore());
            CreateMap<UpdateRoleDto, Role>();

            //Permission
            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>().ForMember(d => d.Id, o => o.Ignore());
            CreateMap<UpdatePermissionDto, Permission>();

            // RolePermission
            CreateMap<RolePermission, RolePermissionDto>()
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.RoleName))
                .ForMember(d => d.PermissionName, o => o.MapFrom(s => s.Permission.PermissionName));

            // Account
            CreateMap<Account, AccountDto>();
            CreateMap<RegisterDto, Account>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore());
            CreateMap<UpdateAccountDto, Account>();

            // User
            CreateMap<User, UserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Account != null ? s.Account.Username : null));

            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.Account, o => o.Ignore());

            CreateMap<UpdateUserDto, User>();

            // StudentProfile
            CreateMap<StudentProfile, StudentProfileDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Name))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
                .ForMember(d => d.SchoolName, o => o.MapFrom(s => s.School != null ? s.School.Name : null))
                .ForMember(d => d.ClassName, o => o.MapFrom(s => s.Class != null ? s.Class.ClassName : null));

            CreateMap<CreateStudentProfileDto, StudentProfile>();
            CreateMap<UpdateStudentProfileDto, StudentProfile>();

            // TeacherProfile
            CreateMap<TeacherProfile, TeacherProfileDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Name))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.DepartmentName : null));

            CreateMap<CreateTeacherProfileDto, TeacherProfile>();
            CreateMap<UpdateTeacherProfileDto, TeacherProfile>();

            // ParentProfile
            CreateMap<ParentProfile, ParentProfileDto>()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.User.Name))
                .ForMember(d => d.ParentEmail, o => o.MapFrom(s => s.User.Email))
                .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.Name))
                .ForMember(d => d.StudentEmail, o => o.MapFrom(s => s.Student.Email));

            CreateMap<CreateParentProfileDto, ParentProfile>();
            CreateMap<UpdateParentProfileDto, ParentProfile>();

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<UpdateDepartmentDto, Department>();

            // Behaviour
            CreateMap<Behaviour, BehaviourDto>()
                .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.Name));
        }
    }
}