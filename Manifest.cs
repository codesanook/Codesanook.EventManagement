using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Codesanook.EventManagement",
    Author = "Codesanook team",
    Website = "https://www.codesanook.com",
    Version = "0.0.1",
    Description = "Codesanook.EventManagement",
    Dependencies = new[] {
        "OrchardCore.Contents",
        "OrchardCore.PublishLater",
    },
    Category = "Content Management"
)]
