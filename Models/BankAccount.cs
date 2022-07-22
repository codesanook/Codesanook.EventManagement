using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Codesanook.EventManagement.Models
{
    public class BankAccount
    {
        private const string V = "Account number";

        public virtual int Id { get; set; }

        [DisplayName("Bank name")]
        [Required]
        public virtual string BankName { get; set; }

        [DisplayName("Account name")]
        [Required]
        public virtual string AccountName { get; set; }

        [DisplayName(V)]
        [RegularExpression(@"\d{10}", ErrorMessage = "Format is number 10 digit")]
        [Required]
        public virtual string AccountNumber { get; set; }

        [DisplayName("Branch name")]
        [Required]
        public virtual string BranchName { get; set; }
    }
}