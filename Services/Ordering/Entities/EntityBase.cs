namespace Ordering.Entities
{
    public abstract class EntityBase
    {
        // protected set --> only current class or derived class can set
        public int Id { get; protected set; }
        // few audit props
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
