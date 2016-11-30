using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class EmailViewModel
    {
        public string ConfirmationMessage { get; set; }

        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string Recipient { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
