using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SentMail.Models
{
    public class EmailEntity
    {

        [ValidateNever]
        public string FromEmailAddress { get; set; }  
        public string ToEmailAddress { get; set; }
        public string CCAddress { get; set; }
        public string Subject { get; set; }
        public string EmailBodyMessage  { get; set; }
        public string ValueSend  { get; set; }
        public string OptionSend { get; set; }

        [ValidateNever]
        public string AttachmentURL  { get; set; }
    }
}
