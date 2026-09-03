using System.ComponentModel.DataAnnotations;

namespace bookhub_api.Models
{
    public class Quote
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK + relation to User
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
