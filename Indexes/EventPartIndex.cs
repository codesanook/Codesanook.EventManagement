using System;
using System.Linq;
using Codesanook.EventManagement.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Codesanook.EventManagement.Indexes
{
    public class EventPartIndex : MapIndex
    {
        public DateTime StartUtc { get; set; }
    }

    public class EventPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context
                .For<EventPartIndex>()
                .When(contentItem => contentItem.Has<EventPart>())
                .Map(contentItem =>
                {
                    // Store only published and latest versions
                    if (!contentItem.Published || !contentItem.Latest)
                    {
                        return null;
                    }

                    var eventPart = contentItem.As<EventPart>();
                    if (eventPart == null)
                    {
                        return null;
                    }

                    var startUtc = eventPart.Periods.OrderBy(p => p.StartUtc).First().StartUtc;
                    return new EventPartIndex()
                    {
                        StartUtc = startUtc
                    };
                });
        }
    }
}