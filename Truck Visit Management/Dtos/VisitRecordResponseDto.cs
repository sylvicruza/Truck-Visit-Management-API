namespace Truck_Visit_Management.Dtos
{
    public class VisitRecordResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();
        public string TruckLicensePlate { get; set; }
        public DriverInformationDto Driver { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
