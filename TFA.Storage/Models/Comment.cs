using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string Text { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
