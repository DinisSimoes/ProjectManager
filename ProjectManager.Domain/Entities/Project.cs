namespace ProjectManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerUserId { get; set; }
        public List<Task> Tasks { get; set; } = new();
    }
}
