using System;
using System.ComponentModel.DataAnnotations;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Codesanook.EventManagement.Models
{
    public class EventPart : ContentPart
    {
        public EventPart()
        {
        }

        [Required]
        public string Location { get; set; }

        public IReadOnlyList<EventPeriod> Periods { get; set; }

        [Required]
        public int MaxAttendees { get; set; }

        public decimal? TicketPrice { get; set; }

        public string GetFormattedSchedule(EventPeriod Period)
        {
            const string dateFormat = "MMM d, yyyy HH:mm";
            return Period.StartUtc.ToString(dateFormat) + " - " + Period.FinishUtc.ToString("HH:mm");
        }
    }
}
