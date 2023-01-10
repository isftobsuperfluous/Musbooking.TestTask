namespace Musbooking.TestTask.ServiceResponses.EquipmentServiceResponses;

public sealed class EquipmentBookedResponse
{
    public bool Ok { get; set; }
    public int Amount { get; set; }

    public EquipmentBookedResponse(bool ok, int amount = 0)
    {
        Ok = ok;
        Amount = amount;
    }
}