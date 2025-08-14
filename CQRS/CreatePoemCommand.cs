using MediatR;
using PoemGenerator.Models;

namespace PoemGenerator.Monolith.Cqrs.Commands;

public record CreatePoemCommand(int Length) : IRequest<Poem>;
