namespace ProjectManager.Domain.Entities
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid ModifiedByUserId { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ChangeDescription { get; set; }
    }
}
