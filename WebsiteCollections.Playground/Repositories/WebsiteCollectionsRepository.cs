using MongoDB.Driver;
using WebsiteCollections.Playground.Models;

namespace WebsiteCollections.Playground.Repositories
{
    public class WebsiteCollectionsRepository : IWebsiteCollectionsRepository
    {
        private readonly IMongoCollection<WebsiteModel> _websiteCollection;
        private readonly ILogger<WebsiteCollectionsRepository> _logger;

        public WebsiteCollectionsRepository(IMongoCollection<WebsiteModel> websiteCollection, ILogger<WebsiteCollectionsRepository> logger)
        {
            _websiteCollection = websiteCollection;
            _logger = logger;
        }

        public async Task AddWebsite(WebsiteModel website)
        {
            try
            {
                await _websiteCollection.InsertOneAsync(website);
                _logger.LogDebug("Website added to the {Collection} collection", website.Collection);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error while adding website to the {Collection} collection", website.Collection);
                throw;
            }
        }

        public async Task<IEnumerable<WebsiteModel>> GetAllWebsiteCollections()
        {
            _logger.LogDebug("Retrieving all collections");
            try
            {
                return await _websiteCollection.Find(website => true).ToListAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error while retrieving all collections");
                throw;
            }
        }

        public async Task<IEnumerable<WebsiteModel>> GetWebsiteCollection(string collection)
        {
            _logger.LogDebug("Retrieving the {Collection} collection", collection);
            try
            {
                return await _websiteCollection.Find(website => website.Collection == collection).ToListAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error while retrieving the {Collection} collection", collection);
                throw;
            }
        }
    }
}
