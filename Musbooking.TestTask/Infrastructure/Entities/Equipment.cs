namespace Musbooking.TestTask.Infrastructure.Entities;

public sealed class Equipment
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }

    public Equipment(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}