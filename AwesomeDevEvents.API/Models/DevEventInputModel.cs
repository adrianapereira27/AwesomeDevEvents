namespace AwesomeDevEvents.API.Models
{
    public class DevEventInputModel  // DTO (dados que serão recebidos - cadastrados)
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
