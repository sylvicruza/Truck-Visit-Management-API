using System.Text.Json.Serialization;
using Truck_Visit_Management.Enums;

namespace Truck_Visit_Management.Dtos
{
    public class VisitRecordDto
    {
        public int Id { get; set; }
        public VisitStatus Status { get; set; }
        public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>(); // Initialize the list

        private string _truckLicensePlate; // Private field for encapsulation

        public string TruckLicensePlate
        {
            get => _truckLicensePlate;
            set => _truckLicensePlate = value?.Trim()?.Replace(" ", "").ToUpper(); // Capitalize and trim whitespace
        }

        public DriverInformationDto Driver { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }


        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(TruckLicensePlate))
            {
                errors.Add("TruckLicensePlate is required.");
            }

            foreach (var activity in Activities)
            {
                if (string.IsNullOrWhiteSpace(activity.UnitNumber))
                {
                    errors.Add("UnitNumber in activities is required.");
                    break; // Stop further checking if one activity is invalid
                }
            }

            // Additional validations as needed...

            return errors;
        }
    }
}
