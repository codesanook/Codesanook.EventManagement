using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using Codesanook.EventManagement.Drivers;
using Codesanook.EventManagement.Models;
using Codesanook.EventManagement.Settings;
using Codesanook.EventManagement.ViewModels;
using OrchardCore.Modules;
using Codesanook.EventManagement.Handlers;
using Codesanook.EventManagement.Indexes;
using YesSql.Indexes;

// https://github.com/meddbtech/topf-v1/tree/develop/src/Orchard.Web/Modules/Codesanook.EventManagement
namespace Codesanook.EventManagement
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TemplateOptions>(o =>
            {
                o.MemberAccessStrategy.Register<EventPartViewModel>();
            });

            services.AddContentPart<EventPart>().UseDisplayDriver<EventPartDisplayDriver>();

            services.AddScoped<IContentDisplayHandler, ContentDisplayHandler>();
            services.AddScoped<IContentTypePartDefinitionDisplayDriver, EventPartSettingsDisplayDriver>();
            services.AddScoped<IDataMigration, Migrations>();

            // services.AddWhereInputIndexPropertyProvider<EventPartIndex>();
            // services.AddSingleton<IIndexPropertyProvider, IndexPropertyProvider<IIndexType>>();
            // https://github.com/OrchardCMS/OrchardCore/blob/main/src/OrchardCore/OrchardCore.Data.YesSql/OrchardCoreBuilderExtensions.cs#L107
            services.AddSingleton<IIndexProvider, EventPartIndexProvider>();
        }
    }
}