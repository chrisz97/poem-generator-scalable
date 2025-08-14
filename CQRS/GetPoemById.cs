using MediatR;
using PoemGenerator.Models;

namespace PoemGenerator.Monolith.Cqrs.Queries;

public record GetPoemByIdQuery(int Id) : IRequest<Poem>;
