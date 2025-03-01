var builder = DistributedApplication.CreateBuilder(args);

var config = builder.Configuration;
var username = config["MongoDB:Username"];
var password = config["MongoDB:Password"];
var usernameParam = builder.AddParameter("username", value: username!);
var passwordParam = builder.AddParameter("password", value: password!, secret: true);

var mongo = builder.AddMongoDB("mongodb", 27017, usernameParam, passwordParam)
    .WithImageTag("latest")
    .WithDataVolume();

var mongodb = mongo.AddDatabase(config["MongoDB:Database"]!);

builder.AddProject<Projects.WebsiteCollections_Playground>("websitecollections-playground")
    .WithReference(mongodb)
    .WaitFor(mongodb);

builder.Build().Run();