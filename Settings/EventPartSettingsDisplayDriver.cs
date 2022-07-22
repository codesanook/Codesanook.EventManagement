using System;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Codesanook.EventManagement.Models;

namespace Codesanook.EventManagement.Settings
{
    public class EventPartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, IUpdateModel updater)
        {
            if (!String.Equals(nameof(EventPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<EventPartSettingsViewModel>("EventPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<EventPartSettings>();

                model.MySetting = settings.MySetting;
                model.EventPartSettings = settings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!String.Equals(nameof(EventPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new EventPartSettingsViewModel();

            if (await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.MySetting))
            {
                context.Builder.WithSettings(new EventPartSettings { MySetting = model.MySetting });
            }

            return Edit(contentTypePartDefinition, context.Updater);
        }
    }
}
