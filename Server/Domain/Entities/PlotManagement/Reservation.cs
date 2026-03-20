namespace API.Domain.Entities.PlotManagement
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid PlotId { get; set; }

        public Guid BeekeeperId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
