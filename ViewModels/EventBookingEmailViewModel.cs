using Codesanook.EventManagement.Models;
using System.Collections.Generic;

namespace Codesanook.EventManagement.ViewModels
{
    public class EventBookingEmailViewModel
    {
        public string SiteName { get; set; }
        public EventPart Event { get; set; }
        // public BasicUserProfilePart UserProfile { get; set; }
        public dynamic AdditionalInformation { get; set; }
        public string ContactEmail { get; set; }
        public IReadOnlyCollection<BankAccount> BankAccounts { get; set; }
    }
}