using System.Text.Json;
using PoemGenerator.Models;
using PoemGenerator.Monolith.Data.Entities;
using PoemGenerator.Monolith.Repositories;

namespace PoemGenerator.Monolith.Services
{
    internal class PoemService(IPoemRepository repository, ILogger<PoemService> logger) : IPoemService
    {
        public record PoetryResponse(string Title, string Author, string[] Lines, int LineCount);

        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://poetrydb.org/")
        };

        private static readonly Bogus.Faker Faker = new();
        private readonly IPoemRepository _repository = repository;
        private readonly ILogger<PoemService> _logger = logger;

        public async Task<Poem> CreatePoem(CreatePoemRequest request)
        {
            // Find or fake poetry based on the request length
            var poetry = await FindPoetry(request.Length) ?? await FakePoetryAsync(request.Length);

            // Persist it
            var savedEntity = await PersistPoemAsync(poetry);

            _logger.LogInformation("Created poem with ID {Id}", savedEntity.Id);

            // Map to API model
            return new Poem(savedEntity.Id, savedEntity.Title, savedEntity.Content, savedEntity.Author);
        }

        private async Task<PoetryResponse?> FindPoetry(int length)
        {
            var response = await _httpClient.GetAsync($"/linecount,random/{length};1");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No poetry found for length {Length}, status {Status}", length, response.StatusCode);
                return null;
            }

            try
            {
                var poetryResponse = await response.Content.ReadFromJsonAsync<PoetryResponse[]>();
                return poetryResponse?.FirstOrDefault();
            }
            catch (JsonException ex)
            {
                // Unfortunately the PoetryDB API does not return status 404 if no poetry is found.
                // Instead if returns {"status":404,"reason":"Not found"} with a 200 OK status.
                _logger.LogError(ex, "Failed to deserialize poetry response for length {Length}", length);
            }

            return null;
        }

        private async Task<PoetryResponse> FakePoetryAsync(int length)
        {
            var delay = Faker.Random.Int(1_000, 2_000);
            _logger.LogInformation("Faking a poem with delay of {Delay}ms", delay);
            await Task.Delay(delay);

            var poetry = new PoetryResponse(
                Title: Faker.Lorem.Sentence(),
                Author: Faker.Name.FullName(),
                Lines: Faker.Lorem.Lines(length).Split('\n'),
                LineCount: length
            );

            return poetry;
        }

        private async Task<Poem> PersistPoemAsync(PoetryResponse poetry)
        {
            var author = await _repository.GetAuthorByNameAsync(poetry.Author);

            var poemEntity = new PoemEntity
            {
                Title = poetry.Title,
                Content = string.Join("\n", poetry.Lines),
                Author = author ?? new AuthorEntity { Name = poetry.Author, CreatedAt = DateTime.UtcNow },
                CreatedAt = DateTime.UtcNow
            };

            var savedEntity = await _repository.CreateAsync(poemEntity);

            return new Poem(savedEntity.Id, savedEntity.Title, savedEntity.Content, savedEntity.Author.Name);
        }

        public async Task<Poem> GetPoem(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Poem with ID {id} not found.");
            }

            return new Poem(entity.Id, entity.Title, entity.Content, entity.Author.Name);
        }

        public async Task<IEnumerable<Poem>> GetAllPoems()
        {
            var poemEntities = await _repository.GetAllAsync();

            // Map to API models
            return poemEntities.Select(entity => new Poem(entity.Id, entity.Title, entity.Content, entity.Author.Name));
        }
    }
}