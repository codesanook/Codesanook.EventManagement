using System;
using System.ComponentModel.DataAnnotations;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Codesanook.EventManagement.Models
{
    /// <summary>
    ///  Not a  content part model
    /// </summary>
    public class EventBookingRecord
    {
        private IList<EventAttendee> eventAttendees;
        public EventBookingRecord() =>
            eventAttendees = new List<EventAttendee>();

        public virtual int Id { get; set; }
        public virtual EventPart Event { get; set; }

        // TODO public virtual IUser User { get; set; }
        public virtual DateTime? BookingDateTimeUtc { get; set; }

        // Todo use enum but enum string 
        public virtual EventBookingStatus Status { get; set; }
        public virtual DateTime? PaidDateTimeUtc { get; set; }

        // TO Azure file storage or USE our media module
        public virtual string PaymentConfirmationAttachmentFileKey { get; set; }

        // Uni-directional mapping and get only
        public virtual IList<EventAttendee> EventAttendees => eventAttendees;
        public void AddEventAttendee(EventAttendee eventAttendee) => eventAttendees.Add(eventAttendee);
    }
}