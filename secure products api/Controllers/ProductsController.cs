using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using secure_products_api.Models;
    
namespace secure_products_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Keyboard", Price = 1200m },
        new Product { Id = 2, Name = "Mouse",    Price = 650m  },
        new Product { Id = 3, Name = "Monitor",  Price = 8500m }
    };
    private static int _nextId = 4;

    [HttpGet]                       // anyone can read the list
    public IActionResult GetAll() => Ok(_products);

    [HttpGet("{id}")]               // anyone can read one product
    public IActionResult GetById(int id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }

    [Authorize(Roles = "Admin")]    // only admins can add
    [HttpPost]
    public IActionResult Create(Product input)
    {
        input.Id = _nextId++;
        _products.Add(input);
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [Authorize(Roles = "Admin")]    // only admins can update
    [HttpPut("{id}")]
    public IActionResult Update(int id, Product input)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p is null) return NotFound();
        p.Name = input.Name;
        p.Price = input.Price;
        return Ok(p);
    }

    [Authorize(Roles = "Admin")]    // only admins can delete
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p is null) return NotFound();
        _products.Remove(p);
        return NoContent();
    }
}
