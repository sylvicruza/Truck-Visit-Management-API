using System.ComponentModel;

namespace Truck_Visit_Management.Enums
{
    public enum VisitStatus
    {
        [Description("Pre-Registered")]
        PreRegistered,

        [Description("At Gate")]
        AtGate,

        [Description("On Site")]
        OnSite,

        [Description("Completed")]
        Completed
    }
}
