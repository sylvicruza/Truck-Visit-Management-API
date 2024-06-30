using Truck_Visit_Management.Enums;

namespace Truck_Visit_Management.Dtos
{
    public class ActivityDto
    {
        public string Type { get; set; }
        private string _unitNumber;

        public string UnitNumber
        {
            get => _unitNumber;
            set => _unitNumber = value?.Trim()?.Replace(" ", "").ToUpper(); // Capitalize and trim whitespace
        }
    }
}