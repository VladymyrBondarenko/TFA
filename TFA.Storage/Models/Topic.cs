using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models
{
    public class Topic
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(ForumId))]
        public Forum Forum { get; set; }

        [Required]
        public Guid ForumId { get; set; }

        [InverseProperty(nameof(Comment.Topic))]
        public ICollection<Comment> Comments { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
