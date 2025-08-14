using System.ComponentModel.DataAnnotations;

namespace PoemGenerator.Monolith.Data.Entities;

public class PoemEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required AuthorEntity Author { get; set; }

    public DateTime CreatedAt { get; set; }
}
