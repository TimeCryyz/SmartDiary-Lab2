using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcLab1.Models
{
    // Переименовано, чтобы избежать конфликта с System.Threading.Tasks.TaskStatus
    public enum DiaryTaskStatus
    {
        Новая,
        ВРаботе,
        Выполнена
    }

    public enum DiaryTaskPriority
    {
        Низкий,
        Средний,
        Высокий
    }

    public class DiaryTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок задачи обязателен")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Заголовок должен содержать от 3 до 200 символов")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }

        public DiaryTaskStatus Status { get; set; } = DiaryTaskStatus.Новая;
        public DiaryTaskPriority Priority { get; set; } = DiaryTaskPriority.Средний;

        public int? ProjectId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();

        // Вычисляемое свойство: просрочена ли задача
        public bool IsOverdue => Deadline.HasValue && Deadline.Value < DateTime.UtcNow && Status != DiaryTaskStatus.Выполнена;

        // Кастомная валидация для дедлайна
        public static ValidationResult? ValidateDeadline(DateTime? deadline, ValidationContext context)
        {
            if (deadline.HasValue)
            {
                var task = (DiaryTask)context.ObjectInstance;

                if (deadline.Value < DateTime.UtcNow)
                {
                    return new ValidationResult("Дедлайн не может быть в прошлом");
                }

                if (deadline.Value < task.CreatedAt)
                {
                    return new ValidationResult("Дедлайн не может быть раньше даты создания задачи");
                }
            }
            return ValidationResult.Success;
        }
    }
}