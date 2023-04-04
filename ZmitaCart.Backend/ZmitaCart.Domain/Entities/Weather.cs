using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Weather : Entity<int>
{
    public string? Day { get; set; }
    public int Temperature { get; set; }
}