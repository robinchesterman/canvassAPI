
namespace Canvass.Api.Models
{
    public class CommunicationMessage
    {
        public string Destination { get; set; }
        public string Body { get; set; }
        public string Medium { get; set; }
    }

    public class EmailMessage : CommunicationMessage
    {
        public EmailMessage()
        {
            Medium = "email";
        }
        public string Subject { get; set; }
    }

    public class TextMessage : CommunicationMessage
    {
        public TextMessage()
        {
            Medium = "textmessage";
        }
    }

    public class RegistrationEmail : EmailMessage
    {
        public string ConfirmationUrl { get; set; }
    }
}