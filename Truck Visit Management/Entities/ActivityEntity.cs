namespace Truck_Visit_Management.Entities
{
    public class ActivityEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } // Delivery or Collection
        public string UnitNumber { get; set; } // Capitalized and no whitespaces
        public int VisitRecordId { get; set; }
        public VisitRecordEntity VisitRecord { get; set; }
    }
}