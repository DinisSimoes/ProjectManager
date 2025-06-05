using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public Enums.TaskStatus Status { get; set; }

        public TaskPriority Priority { get; private set; }

        public List<TaskHistory> History { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

        public Task(TaskPriority priority)
        {
            Priority = priority;
        }

        public void UpdateStatus(Enums.TaskStatus newStatus, User modifiedBy)
        {
            if (Status != newStatus)
            {
                History.Add(new TaskHistory
                {
                    TaskId = this.Id,
                    ModificationDate = DateTime.UtcNow,
                    ModifiedByUserId = modifiedBy.Id,
                    ChangeDescription = $"Status changed from {Status} to {newStatus}"
                });
                Status = newStatus;
            }
        }

        public void UpdateDetails(string newTitle, string newDescription, DateTime newDueDate, User modifiedBy)
        {
            var changes = new List<string>();

            if (Title != newTitle) changes.Add($"Title: '{Title}' -> '{newTitle}'");
            if (Description != newDescription) changes.Add("Description updated");
            if (DueDate != newDueDate) changes.Add($"DueDate: {DueDate} -> {newDueDate}");

            if (changes.Any())
            {
                History.Add(new TaskHistory
                {
                    TaskId = this.Id,
                    ModificationDate = DateTime.UtcNow,
                    ModifiedByUserId = modifiedBy.Id,
                    ChangeDescription = string.Join("; ", changes)
                });

                Title = newTitle;
                Description = newDescription;
                DueDate = newDueDate;
            }
        }

        public void AddComment(string text, User user)
        {
            var comment = new Comment
            {
                TaskId = this.Id,
                UserId = user.Id,
                Content = text,
                CreatedAt = DateTime.UtcNow
            };

            Comments.Add(comment);

            History.Add(new TaskHistory
            {
                TaskId = this.Id,
                ModificationDate = DateTime.UtcNow,
                ModifiedByUserId = user.Id,
                ChangeDescription = $"Comment added: '{text}'"
            });
        }
    }
}
