using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MvcLab1.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(50, ErrorMessage = "Название тега не может превышать 50 символов")]
        public string Name { get; set; }

        public string OwnerId { get; set; }

        // Доп. задание: аудит
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        public IdentityUser Owner { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}