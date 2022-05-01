using MediatR;
using RealEstate.Application.Models;

namespace RealEstate.Application.Queries.RealEstateAgentWithGarden
{
    public class TopTenRealEstateAgentsWithGardenQuery : IRequest<Response<IEnumerable<RealEstateAgentWithGardenViewModel>>>
    {
    }
}
