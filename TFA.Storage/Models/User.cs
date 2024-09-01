using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Login { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Topic.Author))]
        public ICollection<Topic> Topics { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
