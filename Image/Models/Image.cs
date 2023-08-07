using System.ComponentModel.DataAnnotations;

namespace ImageCrud.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? FileName { get; set; }

        public bool IsDelete { get; set; }
    }
}
