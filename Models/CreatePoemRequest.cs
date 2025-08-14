using System.ComponentModel.DataAnnotations;

namespace PoemGenerator.Models;

public record CreatePoemRequest
{
    [Range(10, 1000)]
    public int Length { get; init; }
}
