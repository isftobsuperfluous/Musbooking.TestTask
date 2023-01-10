namespace Musbooking.TestTask.ServiceResponses.ErrorMessages;

public static class EquipmentServiceErrorMessages
{
    public const string EquipmentNotFound = "Equipment not found";
    public const string RequestedMoreThanAvailable = "Requested equipment quantity is more than available";
    public const string ReturnedMoreThanRequested = "Amount of equipment being returned is more than requested";
}