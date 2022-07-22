using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using Codesanook.EventManagement.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Codesanook.EventManagement.ViewModels
{
    public class EventPartViewModel
    {
        public IReadOnlyList<EventPeriod> Periods { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int MaxAttendees { get; set; }

        public decimal? TicketPrice { get; set; }

        [BindNever]
        public ContentItem ContentItem { get; set; }

        [BindNever]
        public EventPart EventPart { get; set; }

    }
}
