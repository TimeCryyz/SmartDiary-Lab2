using System.ComponentModel.DataAnnotations;

namespace MvcLab1.Models
{
    public class Game
    {
        public Game()
        {
            Title = string.Empty;
            Genre = string.Empty;
            Platform = string.Empty;
            Developer = string.Empty;
            Description = string.Empty;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Название игры обязательно")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
        [Display(Name = "Название игры")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Жанр обязателен")]
        [StringLength(50)]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Платформа обязательна")]
        [Display(Name = "Платформа")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "Год выпуска обязателен")]
        [Range(1970, 2026, ErrorMessage = "Год выпуска должен быть от 1970 до 2026")]
        [Display(Name = "Год выпуска")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Разработчик обязателен")]
        [StringLength(100)]
        [Display(Name = "Разработчик")]
        public string Developer { get; set; }

        [Required(ErrorMessage = "Рейтинг обязателен")]
        [Range(0, 10, ErrorMessage = "Рейтинг должен быть от 0 до 10")]
        [Display(Name = "Рейтинг")]
        public double Rating { get; set; }

        [StringLength(1000)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Многопользовательская")]
        public bool IsMultiplayer { get; set; }

        [Display(Name = "Цена")]
        [Range(0, 10000, ErrorMessage = "Цена должна быть от 0 до 10000")]
        public decimal Price { get; set; }

        [Display(Name = "Дата добавления")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        // Метод для расчета возраста игры
        public int GetAge()
        {
            return DateTime.Now.Year - ReleaseYear;
        }

        // Метод для получения рейтинга в звездах
        public string GetRatingStars()
        {
            int starCount = (int)Math.Round(Rating / 2);
            return new string('★', starCount) + new string('☆', 5 - starCount);
        }
    }
}