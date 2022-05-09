using MediatR;
using RealEstate.Application.Extensions;
using RealEstate.Application.Interfaces.Shared;
using RealEstate.Application.Models;

namespace RealEstate.Application.Queries.RealEstateAgent
{
    public class TopTenRealEstateAgentWithGardenQueryHandler : IRequestHandler<RealEstateAgentsWithGardenQuery, Response<IEnumerable<RealEstateAgentWithGardenViewModel>>>
    {
        private readonly IUrlService urlService;
        private readonly IHttpClientWrapperAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> httpClientWrapperAsync;
        private readonly IThrottledHttpClientServiceAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> throttledHttpClientServiceAsync;

        public TopTenRealEstateAgentWithGardenQueryHandler(
            IUrlService urlService,
            IHttpClientWrapperAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> httpClientWrapperAsync,
            IThrottledHttpClientServiceAsync<RealEstateApiResponseModel<RealEstateAgentWithGardenApiResponseModel>> throttledHttpClientServiceAsync)
        {
            this.urlService = urlService;
            this.httpClientWrapperAsync = httpClientWrapperAsync;
            this.throttledHttpClientServiceAsync = throttledHttpClientServiceAsync;
        }

        public async Task<Response<IEnumerable<RealEstateAgentWithGardenViewModel>>> Handle(RealEstateAgentsWithGardenQuery request, CancellationToken cancellationToken)
        {
            var agents = await GetRealEstateAgents();
            var topTenAgents = agents.TakeTopNItems(10, realEstateAgent => realEstateAgent.MakelaarId);
            var topTenAgentsViewModels = topTenAgents.Select(realEstateAgent =>
                                                                                new RealEstateAgentWithGardenViewModel
                                                                                {
                                                                                    AgentId = realEstateAgent.MakelaarId,
                                                                                    Name = realEstateAgent.MakelaarNaam
                                                                                });

            return new Response<IEnumerable<RealEstateAgentWithGardenViewModel>>(topTenAgentsViewModels);
        }

        private async Task<IReadOnlyList<RealEstateAgentWithGardenApiResponseModel>> GetRealEstateAgents()
        {
            // The api returns 15 items no matter page size is
            var firstPageUrl = this.urlService.GetRealEstateAgentUrl(1, 15);
            var firstPage = await httpClientWrapperAsync.GetAsync(firstPageUrl);

            var urls = new List<string>();
            for (var i = 2; i <= firstPage.Paging.AantalPaginas; i++)
            {
                urls.Add(urlService.GetRealEstateAgentUrl(i, 15));
            }

            var response = await throttledHttpClientServiceAsync.GetAsync(urls);
            var agents = new List<RealEstateAgentWithGardenApiResponseModel>();
            agents.AddRange(firstPage.Objects);
            agents.AddRange(response.SelectMany(x => x.Objects).ToArray());

            return agents;
        }
    }
}
