using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private static List<Product> products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", TenantId = "Tenant1" },
        new Product { Id = 2, Name = "Mouse", TenantId = "Tenant1" },
        new Product { Id = 3, Name = "Keyboard", TenantId = "Tenant2" }
    };

    [HttpGet]
    public IActionResult GetProducts()
    {
        var tenantId = User.FindFirst("TenantId")?.Value;

        var tenantProducts = products
            .Where(p => p.TenantId == tenantId)
            .ToList();

        return Ok(tenantProducts);
    }
}