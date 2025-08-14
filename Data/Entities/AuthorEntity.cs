using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PoemGenerator.Monolith.Data.Entities;

public class AuthorEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public ICollection<PoemEntity> Poems { get; set; } = new List<PoemEntity>();

    public DateTime CreatedAt { get; set; }
}