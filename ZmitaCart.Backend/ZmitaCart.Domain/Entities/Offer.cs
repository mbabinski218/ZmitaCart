using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Domain.Entities;

public class Offer : AggregateRoot<int>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Condition { get; set; } = null!;
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual List<UserOffer> Favorites { get; set; }  = new();
    public virtual List<Bought> Bought { get; set; } = new();
    public virtual List<Picture> Pictures { get; set; }  = new();
    public virtual List<CategoryOffer> CategoryOffers { get; set; }  = new(); //TODO remove

    public Offer(string title, string description, decimal price, int quantity, bool isAvailable, DateTimeOffset createdAt, 
        string condition, int categoryId, int userId)
    {
        Title = title;
        Description = description;
        Price = price;
        Quantity = quantity;
        IsAvailable = isAvailable;
        CreatedAt = createdAt;
        Condition = condition;
        CategoryId = categoryId;
        UserId = userId;
    }
    
    public static Offer Create(string title, string description, decimal price, int quantity, bool isAvailable, DateTimeOffset createdAt, 
        string condition, int categoryId, int userId, User user, Category category, IEnumerable<FileStream>? picturesFiles)
    {
        var offer = new Offer(title, description, price, quantity, isAvailable, createdAt, condition, categoryId, userId)
        {
            User = user,
            Category = category
        };

        offer.AddDomainEvent(new OfferCreated(offer, picturesFiles));

        return offer;
    }
}