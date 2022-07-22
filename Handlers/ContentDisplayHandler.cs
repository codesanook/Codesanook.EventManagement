using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Shapes;
using OrchardCore.DisplayManagement.Zones;

namespace Codesanook.EventManagement.Handlers
{
    public class ContentDisplayHandler : IContentDisplayHandler
    {
        public async Task BuildDisplayAsync(ContentItem contentItem, BuildDisplayContext context)
        {
            if (contentItem.ContentType == "Event" && context.DisplayType != "SummaryAdmin")
            {
                // ContentMeta cshtml
                // var contentMetaShape = await context.New.ContentMeta(ContentItem: contentItem);
                var contentMetaShape = await context.New.Event__Meta(ContentItem: contentItem);
                var contentTopLevelZone = context.Shape as IZoneHolding;
                var metaLocalZone = contentTopLevelZone.Zones["Meta"];
                await metaLocalZone.AddAsync(contentMetaShape, "1");
            }
        }

        public Task BuildEditorAsync(ContentItem contentItem, BuildEditorContext context) => Task.CompletedTask;

        public Task UpdateEditorAsync(ContentItem contentItem, UpdateEditorContext context) => Task.CompletedTask;
    }
}