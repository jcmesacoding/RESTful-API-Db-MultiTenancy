namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string TenantId { get; set; }
}