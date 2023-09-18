namespace AwesomeDevEvents.API.Models
{
    public class DevEventSpeakerInputModel   // DTO (dados que serão recebidos - cadastrados)
    {
        public string Name { get; set; }
        public string TalkTitle { get; set; }
        public string TalkDescription { get; set; }
        public string LinkedInProfile { get; set; }
    }
}
