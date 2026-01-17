using aLMS.Application.Common.Dtos;
using aLMS.Domain.StudentFinalTermRecordEntity;

namespace aLMS.Application.Common.Interfaces
{
    public interface IFinalTermRecordRepository
    {
        Task<FinalTermRecordDto?> GetByIdAsync(Guid id);
        Task<List<FinalTermRecordDto>> GetByStudentIdAsync(Guid studentProfileId);
        Task<List<FinalTermRecordDto>> GetByClassIdAsync(Guid classId);
        Task AddAsync(StudentFinalTermRecord record);
        Task UpdateAsync(StudentFinalTermRecord record);
        Task<StudentFinalTermRecord?> GetEntityByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsForStudentAsync(Guid studentProfileId, Guid? classId = null);
    }
}