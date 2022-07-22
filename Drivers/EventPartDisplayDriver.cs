using System.Threading.Tasks;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.Views;
using Codesanook.EventManagement.Models;
using Codesanook.EventManagement.ViewModels;
using Mapster.Adapters;
using Mapster;
using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System;

namespace Codesanook.EventManagement.Drivers
{
    public class EventPartDisplayDriver : ContentPartDisplayDriver<EventPart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IStringLocalizer S;


        public EventPartDisplayDriver(IContentDefinitionManager contentDefinitionManager, IStringLocalizer<EventPartDisplayDriver> localizer)
        {
            _contentDefinitionManager = contentDefinitionManager;
            S = localizer;
        }
        public override IDisplayResult Display(EventPart part, BuildPartDisplayContext context) =>
            Initialize<EventPartViewModel>(GetDisplayShapeType(context), m => BuildViewModel(m, part, context.TypePartDefinition))
                .Location("Detail", "Content:10")
                .Location("Summary", "Content:10");

        public override IDisplayResult Edit(EventPart part, BuildPartEditorContext context) =>
            Initialize<EventPartViewModel>(GetEditorShapeType(context), m => BuildViewModel(m, part, context.TypePartDefinition));

        public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
        {
            var viewModel = new EventPartViewModel();
            var updateResult = await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

            // Use of Fluent Validator 
            // https://docs.fluentvalidation.net/en/latest/built-in-validators.html
            // Built-in Validators
            /*
            - NotNull Validator
            - NotEmpty Validator
            - NotEqual Validator
            - Equal Validator
            - Length Validator
            - MaxLength Validator
            - MinLength Validator
            - Less Than Validator
            - Less Than Or Equal Validator
            - Greater Than Validator
            - Greater Than Or Equal Validator
            - Predicate Validator
            - Regular Expression Validator
            - Email Validator
            - Credit Card Validator
            - Enum Validator
            - Enum Name Validator
            - Empty Validator
            - Null Validator
            - ExclusiveBetween Validator InclusiveBetween Validator
            - ScalePrecision Validator
            */

            // Custom Validators
            // https://docs.fluentvalidation.net/en/latest/custom-validators.html
            var ticketPriceValidationResult = ValidateTicketPrice(viewModel);
            if (!ticketPriceValidationResult)
            {
                context.Updater.ModelState.AddModelError(
                    Prefix,
                    nameof(viewModel.TicketPrice),
                    "A ticket's price must be greater than 0 for a paid ticket"
                );
            }

            // var eventDateTimeValidationResult = ValidEventDateTime(viewModel);
            // if (!eventDateTimeValidationResult)
            // {
            //     context.Updater.ModelState.AddModelError(
            //         Prefix,
            //         nameof(viewModel.StartUtc),
            //         "Start UTC must come before Finish UTC"
            //     );

            //     context.Updater.ModelState.AddModelError(
            //         Prefix,
            //         nameof(viewModel.FinishUtc),
            //         "Finish UTC must come after Start UTC"
            //     );
            // }

            if (updateResult && ticketPriceValidationResult)
            {
                // part.StartUtc = viewModel.StartUtc;
                // part.FinishUtc = viewModel.FinishUtc;
                part.Location = viewModel.Location;
                part.MaxAttendees = viewModel.MaxAttendees;
                part.TicketPrice = viewModel.TicketPrice;
                part.Periods = viewModel.Periods;
            }

            return Edit(part, context);
        }

        // private bool ValidEventDateTime(EventPartViewModel viewModel)
        // {
        //     return viewModel.StartUtc < viewModel.FinishUtc;
        // }

        private bool ValidateTicketPrice(EventPartViewModel viewModel)
        {
            if (!viewModel.TicketPrice.HasValue) return true;
            if (viewModel.TicketPrice.HasValue && viewModel.TicketPrice.Value > 0) return true;
            return false;
        }

        private void BuildViewModel(EventPartViewModel viewModel, EventPart part, ContentTypePartDefinition contentTypePartDefinition)
        {
            viewModel.Location = part.Location;
            viewModel.MaxAttendees = part.MaxAttendees;
            viewModel.TicketPrice = part.TicketPrice;

            // var settings = contentTypePartDefinition.GetSettings<EventPartSettings>();
            // Cannot set default in constructure because Orchard deserialize add existing/not replace.
            if ((part.Periods?.Count ?? 0) == 0)
            {
                viewModel.Periods = new List<EventPeriod>(){
                    new EventPeriod(){ StartUtc = DateTime.UtcNow, FinishUtc= DateTime.UtcNow }
                };
            }
            else
            {
                viewModel.Periods = part.Periods;
            }

            viewModel.ContentItem = part.ContentItem;
            viewModel.EventPart = part;
        }
    }
}
