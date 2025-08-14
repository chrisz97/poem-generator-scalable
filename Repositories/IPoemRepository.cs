using PoemGenerator.Monolith.Data.Entities;

namespace PoemGenerator.Monolith.Repositories;

public interface IPoemRepository
{
    Task<AuthorEntity?> GetAuthorByNameAsync(string name);
    Task<PoemEntity> CreateAsync(PoemEntity poem);
    Task<PoemEntity?> GetByIdAsync(int id);
    Task<IEnumerable<PoemEntity>> GetAllAsync();
}