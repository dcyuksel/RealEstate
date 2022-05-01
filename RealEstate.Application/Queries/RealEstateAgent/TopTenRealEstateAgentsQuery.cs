using MediatR;
using RealEstate.Application.Models;

namespace RealEstate.Application.Queries.RealEstateAgent
{
    public class RealEstateAgentsWithGardenQuery : IRequest<Response<IEnumerable<RealEstateAgentWithGardenViewModel>>>
    {
    }
}
