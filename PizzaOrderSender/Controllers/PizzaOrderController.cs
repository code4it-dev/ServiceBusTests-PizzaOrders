using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrderSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaOrderController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(IEnumerable<PizzaOrder> order)
        {
            return Ok();
        }
    }
}