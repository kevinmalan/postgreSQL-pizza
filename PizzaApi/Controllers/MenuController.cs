using Microsoft.AspNetCore.Mvc;
using PizzaApi.Services.Interfaces;

namespace PizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController(IMenuService menuService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await menuService.GetPizzaMenuAsync());
    }
}