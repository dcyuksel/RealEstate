using MediatR;
using RealEstate.Application.Extensions;
using RealEstate.Application.Interfaces.Shared;
using RealEstate.Application.Models;

namespace RealEstate.Application.Queries.RealEstateAgentWithGarden
{
    public class TopTenRealEstateAgentsWithGardenQueryHandler : IRequestHandler<TopTenRealEstateAgentsWithGardenQuery, Response<IEnumerable<RealEstateAgentWithGardenViewModel>>>
    {
        private readonly IUrlService urlService;
        private readonly IThrottledHttpClientServiceAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> throttledHttpClientServiceAsync;

        public TopTenRealEstateAgentsWithGardenQueryHandler(
            IUrlService urlService,
            IThrottledHttpClientServiceAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> throttledHttpClientServiceAsync)
        {
            this.urlService = urlService;
            this.throttledHttpClientServiceAsync = throttledHttpClientServiceAsync;
        }

        public async Task<Response<IEnumerable<RealEstateAgentWithGardenViewModel>>> Handle(TopTenRealEstateAgentsWithGardenQuery request, CancellationToken cancellationToken)
        {
            var agentsWithGarden = await GetRealEstateAgentsWithGarden();
            var topTenAgentsWithGarden = agentsWithGarden.TakeTopNItems(10, realEstateAgent => realEstateAgent.MakelaarId);
            var topTenAgentsWithGardenViewModels = topTenAgentsWithGarden.Select(realEstateAgent =>
                                                                                    new RealEstateAgentWithGardenViewModel
                                                                                    {
                                                                                        AgentId = realEstateAgent.MakelaarId,
                                                                                        Name = realEstateAgent.MakelaarNaam
                                                                                    });

            return new Response<IEnumerable<RealEstateAgentWithGardenViewModel>>(topTenAgentsWithGardenViewModels);
        }

        private async Task<IReadOnlyList<RealEstateAgentWithGardenApiResponseModel>> GetRealEstateAgentsWithGarden()
        {
            // The api returns 15 items no matter page size is
            var firstPageUrl = urlService.GetRealEstateAgentWithGardenUrl(1, 15);
            var firstPage = await throttledHttpClientServiceAsync.GetAsync(firstPageUrl);

            var urls = new List<string>();
            for (var i = 2; i <= firstPage.Paging.AantalPaginas; i++)
            {
                urls.Add(urlService.GetRealEstateAgentWithGardenUrl(i, 15));
            }

            var response = await throttledHttpClientServiceAsync.GetAsync(urls);
            var agentsWithGarden = new List<RealEstateAgentWithGardenApiResponseModel>();
            agentsWithGarden.AddRange(firstPage.Objects);
            agentsWithGarden.AddRange(response.SelectMany(x => x.Objects).ToArray());

            return agentsWithGarden;
        }
    }
}
