namespace ArchyTECH.EntityFramework.Extensions.Auditing
{
    public interface IAuditParentEntity
    {
        int ParentEntityId { get; }
    }
}