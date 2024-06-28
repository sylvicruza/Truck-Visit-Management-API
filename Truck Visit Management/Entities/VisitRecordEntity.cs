namespace Truck_Visit_Management.Entities
{
    public class VisitRecordEntity
    {
        public int Id { get; set; }
        public string Status { get; set; } // Pre-Registered, At Gate, On Site, Completed
        public List<ActivityEntity> Activities { get; set; }
        public string TruckLicensePlate { get; set; }
        public DriverInformationEntity Driver { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }

        public VisitRecordEntity()
        {
            Activities = new List<ActivityEntity>();
        }
    }
}
