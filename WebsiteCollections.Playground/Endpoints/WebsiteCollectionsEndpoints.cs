using MongoDB.Driver;
using WebsiteCollections.Playground.DTOs;
using WebsiteCollections.Playground.Models;
using WebsiteCollections.Playground.Services;

namespace WebsiteCollections.Playground.Endpoints
{
    public static class WebsiteCollectionsEndpoints
    {
        public static IEndpointRouteBuilder MapWebsiteCollectionsEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/collections", AddWebsiteAsync);

            routes.MapGet("/collections", GetAllCollectionsAsync);

            routes.MapGet("/collections/{collection}", GetCollectionAsync);

            return routes;
        }


        private static async Task<IResult> AddWebsiteAsync(WebsiteDto dto, IWebsiteCollectionsService service, ILogger<Program> logger)
        {
            logger.LogInformation("POST - Adding website to collection: {Collection}, with title: {Title}", dto.Collection, dto.Title);

            if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute))
            {
                return Results.BadRequest("The URL provided is not valid");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                var uri = new Uri(dto.Url);
                dto.Title = uri.Host.Replace("www.", "").Split('.')[0];
            }

            var website = new WebsiteModel()
            {
                Collection = dto.Collection,
                Url = dto.Url,
                Title = dto.Title
            };

            try
            {
                await service.AddWebsiteAsync(website);
                return Results.Created($"/collections/{dto.Collection}", website);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Results.Conflict("A website with the same URL already exists in the collection.");
            }
        }


        private static async Task<IResult> GetAllCollectionsAsync(IWebsiteCollectionsService service, ILogger<Program> logger)
        {
            logger.LogInformation("GET - Retrieving all website collections");
            try
            {
                var collections = await service.GetAllWebsiteCollectionsAsync();
                return Results.Ok(collections);
            }
            catch (Exception)
            {
                return Results.InternalServerError("An error occurred while retrieving all website collections");
            }
        }


        private static async Task<IResult> GetCollectionAsync(string collection, IWebsiteCollectionsService service, ILogger<Program> logger)
        {
            logger.LogInformation("GET - Retrieving website collection: \"{Collection}\"", collection);
            try
            {
                var websiteCollections = await service.GetWebsiteCollectionAsync(collection);
                return Results.Ok(websiteCollections);
            }
            catch (Exception)
            {
                return Results.InternalServerError("An error occurred while retrieving the website collection");
            }
        }
    }
}
