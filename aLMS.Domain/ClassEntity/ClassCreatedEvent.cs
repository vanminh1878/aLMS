using aLMS.Domain.Common;

public record ClassCreatedEvent(Guid ClassId, string ClassName, string Grade, string SchoolYear, Guid SchoolId) : IDomainEvent;

public record ClassUpdatedEvent(Guid ClassId, string ClassName, string Grade, string SchoolYear, Guid SchoolId) : IDomainEvent;

public record ClassSoftDeletedEvent(Guid ClassId) : IDomainEvent;