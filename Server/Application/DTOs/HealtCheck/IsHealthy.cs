namespace API.Application.DTOs.HealtCheck
{
    public class IsHealthy
    {
        public string status { get; set; } = string.Empty;

        public IsHealthy()
        {
            status = "Pollinator is healthy!";
        }
    }
}
