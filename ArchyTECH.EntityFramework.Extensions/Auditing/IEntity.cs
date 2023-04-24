namespace ArchyTECH.EntityFramework.Extensions.Auditing
{
    public interface IEntity
    {
        int Id { get; }
        DateTime ModifiedDate { get; set; }
        string ModifierId { get; set; }
    }
}