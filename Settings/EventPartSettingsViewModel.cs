using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Codesanook.EventManagement.Settings
{
    public class EventPartSettingsViewModel
    {
        public string MySetting { get; set; }

        [BindNever]
        public EventPartSettings EventPartSettings { get; set; }
    }
}
