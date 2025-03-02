using WebsiteCollections.Playground.Models;
using WebsiteCollections.Playground.Repositories;

namespace WebsiteCollections.Playground.Services
{
    public class WebsiteCollectionsService : IWebsiteCollectionsService
    {
        private readonly IWebsiteCollectionsRepository _repository;
        private readonly ILogger<WebsiteCollectionsService> _logger;

        public WebsiteCollectionsService(IWebsiteCollectionsRepository repository, ILogger<WebsiteCollectionsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task AddWebsiteAsync(WebsiteModel website)
        {
            _logger.LogDebug("Service - Adding website: {@Website}", website);
            try
            {
                await _repository.AddWebsite(website);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<WebsiteModel>> GetWebsiteCollectionAsync(string collection)
        {
            _logger.LogDebug("Service - Getting website collection: {Collection}", collection);
            try
            {
                return await _repository.GetWebsiteCollection(collection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<WebsiteModel>> GetAllWebsiteCollectionsAsync()
        {
            _logger.LogDebug("Service - Getting all website collections");
            try
            {
                return await _repository.GetAllWebsiteCollections();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
