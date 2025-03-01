using MongoDB.Driver;
using Scalar.AspNetCore;
using WebsiteCollections.Playground.Endpoints;
using WebsiteCollections.Playground.Models;
using WebsiteCollections.Playground.Repositories;
using WebsiteCollections.Playground.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

// Read MongoDB settings from configuration
var connectionString = builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString");
var databaseName = builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName");
var collectionName = builder.Configuration.GetValue<string>("MongoDbSettings:CollectionName");

// Configure client, database, and collection
var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);
var websiteCollection = database.GetCollection<WebsiteModel>(collectionName);


// Register the MongoDB collection, repository, and service
builder.Services.AddSingleton<IMongoCollection<WebsiteModel>>(websiteCollection);
builder.Services.AddSingleton<IWebsiteCollectionsRepository, WebsiteCollectionsRepository>();
builder.Services.AddSingleton<IWebsiteCollectionsService, WebsiteCollectionsService>();

builder.Services
    .AddSingleton(sp => mongoClient)
    .AddHealthChecks()
    .AddMongoDb(
        name: "mongodb",
        timeout: TimeSpan.FromSeconds(3),
        tags: ["ready"]
    );

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapWebsiteCollectionsEndpoints();

//app.MapHealthChecks("/health");

app.Run();