namespace ProjectManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Task> Tasks { get; set; } = new();
    }
}
