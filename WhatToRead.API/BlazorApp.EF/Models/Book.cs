using System.ComponentModel.DataAnnotations;

namespace BlazorApp.EF.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Title is too short")]
        public string Title { get; set; }
        public int Language_Id { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "Number of pages is too short or too long (1, 10000)")]
        public int NumberOfPages { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), minimum: "1/1/1600", maximum: "20.05.2024", ErrorMessage = "Invalid date")]
        public DateTime Publication_Date { get; set; }
        [Required]
        public int Publisher_Id { get; set; }
        [Required]
        public int Author_Id { get; set; }
    }
}
