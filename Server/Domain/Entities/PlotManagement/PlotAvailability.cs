namespace API.Domain.Entities.PlotManagement
{
    public class PlotAvailability
    {
        public Guid PlotAvailabilityId { get; set; }
        public Guid PlotId { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; } = 2026;
        public AvailabilityStatus Status { get; set; }
    }
    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}



