using PoemGenerator.Models;

namespace PoemGenerator.Monolith.Services
{
    public interface IPoemService
    {
        Task<Poem> CreatePoem(CreatePoemRequest request);
        Task<Poem> GetPoem(int id);
        Task<IEnumerable<Poem>> GetAllPoems();
    }
}