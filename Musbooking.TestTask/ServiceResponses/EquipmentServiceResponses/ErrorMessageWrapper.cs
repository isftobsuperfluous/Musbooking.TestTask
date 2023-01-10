namespace Musbooking.TestTask.ServiceResponses.EquipmentServiceResponses;

public sealed class ErrorMessageWrapper
{
    public string Message { get; set; }

    public ErrorMessageWrapper(string message)
    {
        Message = message;
    }
}