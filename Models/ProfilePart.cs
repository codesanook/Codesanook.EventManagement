

using System;
using System.ComponentModel.DataAnnotations;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Codesanook.EventManagement.Models
{
    public enum Occupation
    {
        Staff,
        Other
    }
    public class UserProfile
    {
        public string OfficeName { get; set; }
        public string MobilePhoneNumber { get; set; }
        public Occupation Occupation { get; set; }
    }
}