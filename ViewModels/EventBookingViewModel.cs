using System;
using System.Collections.Generic;
using Codesanook.EventManagement.Models;

namespace Codesanook.EventManagement.ViewModels
{
    public class EventBookingViewModel
    {
        public int Id { get; set; }
        public EventPart Event { get; set; }
        // public UserPart User { get; set; }
        public DateTime? BookingDateTimeUtc { get; set; }
        public EventBookingStatus Status { get; set; }
        public DateTime? PaidDateTimeUtc { get; set; }
        public string PaymentConfirmationAttachmentFileUrl { get; set; }
        public IList<EventAttendee> EventAttendees { get; set; }
        // https://stackoverflow.com/a/4872004/1872200
        public decimal TotalPrice => Math.Round((EventAttendees?.Count ?? 0) * (Event?.TicketPrice ?? 0), 2);

        public string GetTextFromNow()
        {

            var toDay = DateTime.Now;
            if (BookingDateTimeUtc.HasValue)
            {
                int daysDiff = (toDay.Date - BookingDateTimeUtc.Value.Date).Days;
                if (daysDiff > 1)
                {
                    return daysDiff + " days ago";
                }

                return "today";
            }

            return "";
        }

    }
}