using Microsoft.EntityFrameworkCore;

namespace MvcLab1.Models
{
    [PrimaryKey(nameof(TaskId), nameof(TagId))]
    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }

        public DiaryTask Task { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}