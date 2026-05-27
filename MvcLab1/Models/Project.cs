using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcLab1.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название проекта обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно содержать от 3 до 100 символов")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [RegularExpression("^([A-Fa-f0-9]{6})$", ErrorMessage = "Цвет должен быть в формате HEX (RRGGBB)")]
        public string Color { get; set; } = "808080";

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Доп. задание: аудит
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public IdentityUser Owner { get; set; }

        public ICollection<DiaryTask> Tasks { get; set; } = new List<DiaryTask>();
    }
}