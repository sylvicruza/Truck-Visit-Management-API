using System.Text.Json.Serialization;
using Truck_Visit_Management.Enums;

namespace Truck_Visit_Management.Dtos
{
    public class VisitRecordRequestDto
    {
        public string Status { get; set; }
        public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();

        private string _truckLicensePlate; // Private field for encapsulation

        public string TruckLicensePlate
        {
            get => _truckLicensePlate;
            set => _truckLicensePlate = value?.Trim()?.Replace(" ", "").ToUpper(); // Capitalize and trim whitespace
        }
        public DriverInformationDto Driver { get; set; }


        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedTime { get; set; }
        [JsonIgnore]
        public string? UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedTime { get; set; }


        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(TruckLicensePlate))
            {
                errors.Add("TruckLicensePlate is required.");
            }

            if (Activities == null || Activities.Count == 0)
            {
                errors.Add("At least one activity is required.");
            }
            else
            {
                foreach (var activity in Activities)
                {
                    if (string.IsNullOrWhiteSpace(activity.UnitNumber))
                    {
                        errors.Add("UnitNumber in activities is required.");
                        break; // Stop further checking if one activity is invalid
                    }

                    if (activity.Type != "Delivery" && activity.Type != "Collection")
                    {
                        errors.Add("Invalid activity type. Type must be either 'Delivery' or 'Collection'.");
                    }
                }
            }

            // Check if Status is one of the allowed values
            if (Status != "Pre-Registered" &&
                Status != "At Gate" &&
                Status != "On Site" &&
                Status != "Completed")
            {
                errors.Add("Invalid status value. Status must be either 'Pre-Registered' or 'At Gate' or 'On Site' or 'Completed'.");
            }

            // Additional validations as needed...

            return errors;
        }

    }
}
