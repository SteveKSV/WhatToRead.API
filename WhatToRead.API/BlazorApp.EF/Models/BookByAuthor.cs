using System.ComponentModel.DataAnnotations;

namespace BlazorApp.EF.Models
{
    public class BookByAuthor
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Title is too short")]
        public string Title { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Number of pages is too short or too long (1, 10000)")]
        public int NumberOfPages { get; set; }

        [DataType(DataType.Date)]
        [Range(typeof(DateTime), minimum: "1/1/1600", maximum: "20.05.2023", ErrorMessage = "Invalid date")]
        public DateTime Publication_Date { get; set; } = DateTime.Now;

        [Required]
        [MinLength(5, ErrorMessage = "Author's name is too short")]
        public string Author_Name { get; set; }
    }
}
