using aLMS.Application.Common.Dtos;
using aLMS.Domain.AnswerEntity;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.ExerciseEntity;
using aLMS.Domain.GradeEntity;
using aLMS.Domain.LessonEntity;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.SubjectEntity;
using aLMS.Domain.TopicEntity;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aLMS.Application.Common.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<School, SchoolDto>();
            CreateMap<CreateSchoolDto, School>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty)); // Thay Ignore() tạm thời
            CreateMap<UpdateSchoolDto, School>();
            // Class
            CreateMap<Class, ClassDto>();
            CreateMap<CreateClassDto, Class>();
            CreateMap<UpdateClassDto, Class>();

            // Grade
            CreateMap<Grade, GradeDto>();
            CreateMap<CreateGradeDto, Grade>();
            CreateMap<UpdateGradeDto, Grade>();

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
        }
    }
}