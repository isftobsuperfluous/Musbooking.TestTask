using Musbooking.TestTask.Enums;
using Musbooking.TestTask.Infrastructure.Entities;
using Musbooking.TestTask.ServiceResponses;
using Musbooking.TestTask.ServiceResponses.EquipmentServiceResponses;

namespace Musbooking.TestTask.ServiceAbstractions;

public interface IEquipmentService
{
    Task<ServiceResponse<Equipment?, string>> GetAsync(Guid id);
    
    Task<ServiceResponse<Equipment?, string>> GetBookedAsync(Guid id);
    
    Task<Guid> CreateAsync(string name, int amount);

    Task<ServiceResponse<Guid?, string>> UpdateAsync(Guid id, string? name, int? amount);

    Task<ServiceResponse<Guid?, string>> DeleteAsync(Guid id);
    
    Task<ServiceResponse<EquipmentBookedResponse, string>> BookingAsync(Guid id, int amount);
}