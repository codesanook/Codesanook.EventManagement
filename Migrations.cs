using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.ContentManagement;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Autoroute.Models;
using Codesanook.EventManagement.Models;
using YesSql.Sql;
using Codesanook.EventManagement.Indexes;
using System;

namespace Codesanook.EventManagement
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly IHtmlLocalizer<Migrations> H;

        public Migrations(
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager,
            IHtmlLocalizer<Migrations> localizer
        )
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            H = localizer;
        }

        public int Create()
        {
            CreateEventPart();
            CreateEventType();
            CreateEventPartIndex();
            return 1;
        }

        private void CreateEventPartIndex()
        {
            SchemaBuilder.CreateMapIndexTable<EventPartIndex>(
                table => table
                    .Column<DateTime>(nameof(EventPartIndex.StartUtc), c => c.NotNull())
            );

            // Document field is created here https://github.com/sebastienros/yessql/blob/main/src/YesSql.Core/Sql/SchemaBuilder.cs#L61
            SchemaBuilder.AlterIndexTable<EventPartIndex>(table => table
                .CreateIndex("IX_EventPartIndex_DocumentId_StartUtc", "DocumentId", nameof(EventPartIndex.StartUtc))
            );
        }

        private void CreateEventPart()
        {
            _contentDefinitionManager.AlterPartDefinition(
                nameof(EventPart),
                builder => builder
                    .Attachable()
                    .WithDescription("Provide an Event part for a content item")
            );
        }

        private void CreateEventType()
        {
            _contentDefinitionManager.AlterTypeDefinition(
                "Event",
                type => type
                    .WithPart(
                        "TitlePart",
                        part => part.WithPosition("0")
                    )
                    .WithPart(
                        nameof(EventPart),
                        part => part.WithPosition("1")
                    )
                    .WithPart(
                        "HtmlBodyPart",
                        part => part
                            .WithEditor("Wysiwyg")
                            .WithPosition("2")
                            .WithDisplayName("Details")
                    )
                    .WithPart(
                        "AutoroutePart",
                        part => part
                            .WithSettings(new AutoroutePartSettings()
                            {
                                Pattern = "/events/{{ ContentItem.ContentItemId }}",
                                AllowCustomPath = false,
                            })
                            .WithPosition("3")
                    )
                    .WithPart(
                        "PublishLaterPart",
                        part => part.WithPosition("4")
                    )
                    .Versionable(false)
                    .Creatable()
                    .Listable()
                    .Draftable()
            );
        }
    }
}
