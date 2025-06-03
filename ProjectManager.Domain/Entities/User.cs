namespace ProjectManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
    }
}
