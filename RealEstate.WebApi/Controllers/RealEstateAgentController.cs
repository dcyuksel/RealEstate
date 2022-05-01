using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Queries.RealEstateAgent;
using RealEstate.Application.Queries.RealEstateAgentWithGarden;
using System.Threading.Tasks;

namespace RealEstate.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateAgentController : BaseApiController
    {
        [HttpGet("TopTenRealEstateAgents")]
        public async Task<IActionResult> GetTopTenRealEstateAgents()
        {
            return Ok(await Mediator.Send(new RealEstateAgentsWithGardenQuery()));
        }

        [HttpGet("TopTenRealEstateAgentsWithGarden")]
        public async Task<IActionResult> GetTopTenRealEstateAgentsWithGarden()
        {
            return Ok(await Mediator.Send(new TopTenRealEstateAgentsWithGardenQuery()));
        }
    }
}
