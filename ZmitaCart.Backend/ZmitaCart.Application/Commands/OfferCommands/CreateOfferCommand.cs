using MediatR;
using Microsoft.AspNetCore.Http;
using ZmitaCart.Domain.Enums;

namespace ZmitaCart.Application.Commands.OfferCommands;

public record CreateOfferCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Condition { get; set; } = null!;
    public int CategoryId { get; set; }
    public IEnumerable<IFormFile>? Pictures { get; set; }
}