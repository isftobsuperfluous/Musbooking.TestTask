using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Enums;
using Musbooking.TestTask.Infrastructure;
using Musbooking.TestTask.Infrastructure.Entities;
using Musbooking.TestTask.ServiceAbstractions;
using Musbooking.TestTask.ServiceResponses;
using Musbooking.TestTask.ServiceResponses.EquipmentServiceResponses;
using Musbooking.TestTask.ServiceResponses.ErrorMessages;

namespace Musbooking.TestTask.Services;

public sealed class EquipmentService : IEquipmentService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly InMemoryDbContext _inMemoryDbContext;
    
    public EquipmentService(ApplicationDbContext dbContext, InMemoryDbContext inMemoryDbContext)
    {
        _dbContext = dbContext;
        _inMemoryDbContext = inMemoryDbContext;
    }

    public async Task<ServiceResponse<Equipment?, string>> GetAsync(Guid id)
    {
        var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment is null)
        {
            return new ServiceResponse<Equipment?, string>(null, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Error);
        }

        return new ServiceResponse<Equipment?, string>(equipment, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Succeeded);
    }

    public async Task<ServiceResponse<Equipment?, string>> GetBookedAsync(Guid id)
    {
        var equipment = await _inMemoryDbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment is null)
        {
            return new ServiceResponse<Equipment?, string>(null, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Error);
        }

        return new ServiceResponse<Equipment?, string>(equipment, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Succeeded);
    }

    public async Task<Guid> CreateAsync(string name, int amount)
    {
        var equipmentEntity = new Equipment(name, amount);

        await _dbContext.Equipment.AddAsync(equipmentEntity);
        await _dbContext.SaveChangesAsync();

        return equipmentEntity.Id;
    }

    public async Task<ServiceResponse<Guid?, string>> UpdateAsync(Guid id, string? name, int? amount)
    {
        var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment is null)
        {
            return new ServiceResponse<Guid?, string>(null, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Error);
        }

        equipment.Name = name ?? equipment.Name;
        equipment.Amount = amount ?? equipment.Amount;
        _dbContext.Equipment.Update(equipment);

        await _dbContext.SaveChangesAsync();

        return new ServiceResponse<Guid?, string>(equipment.Id, null, ServiceResponseStatus.Succeeded);
    }

    public async Task<ServiceResponse<Guid?, string>> DeleteAsync(Guid id)
    {
        var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment is null)
        {
            return new ServiceResponse<Guid?, string>(id, EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Error);
        }

        _dbContext.Equipment.Remove(equipment);
        await _dbContext.SaveChangesAsync();

        return new ServiceResponse<Guid?, string>(null, null, ServiceResponseStatus.Succeeded);
    }

    public async Task<ServiceResponse<EquipmentBookedResponse, string>> BookingAsync(Guid id, int amount)
    {
        var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment is null)
        {
            return new ServiceResponse<EquipmentBookedResponse, string>(
                new EquipmentBookedResponse(false), 
                EquipmentServiceErrorMessages.EquipmentNotFound, 
                ServiceResponseStatus.Error);
        }
        
        if (amount != 0)
        {
            var bookedEquipment = await _inMemoryDbContext.Equipment.FirstOrDefaultAsync(x => x.Id == id);
            if (amount > 0)
            {
                if (equipment.Amount >= amount)
                {
                    if (bookedEquipment is null)
                    {
                        await _inMemoryDbContext.Equipment.AddAsync(new Equipment(equipment.Name, amount)
                        {
                            Id = id
                        });
                        
                    }
                    else
                    {
                        bookedEquipment.Amount += amount;
                    }
                    
                    equipment.Amount -= amount;
                    _dbContext.Equipment.Update(equipment);
                    await _dbContext.SaveChangesAsync();
                    await _inMemoryDbContext.SaveChangesAsync();

                    return new ServiceResponse<EquipmentBookedResponse, string>(
                        new EquipmentBookedResponse(true, equipment.Amount), null, ServiceResponseStatus.Succeeded);
                }

                return new ServiceResponse<EquipmentBookedResponse, string>(
                    new EquipmentBookedResponse(false, equipment.Amount), 
                    EquipmentServiceErrorMessages.RequestedMoreThanAvailable,
                    ServiceResponseStatus.Error);
            }
            
            if (bookedEquipment is null)
            {
                return new ServiceResponse<EquipmentBookedResponse, string>(
                    new EquipmentBookedResponse(false), EquipmentServiceErrorMessages.EquipmentNotFound, ServiceResponseStatus.Error);
            }

            if (bookedEquipment.Amount < Math.Abs(amount))
            {
                return new ServiceResponse<EquipmentBookedResponse, string>(
                    new EquipmentBookedResponse(false, bookedEquipment.Amount), EquipmentServiceErrorMessages.ReturnedMoreThanRequested, ServiceResponseStatus.Error);
            }

            equipment.Amount -= amount;
            _inMemoryDbContext.Remove(bookedEquipment);
            await _dbContext.SaveChangesAsync();
            await _inMemoryDbContext.SaveChangesAsync();

            return new ServiceResponse<EquipmentBookedResponse, string>(
                new EquipmentBookedResponse(true, equipment.Amount), null, ServiceResponseStatus.Succeeded);
        }

        return new ServiceResponse<EquipmentBookedResponse, string>(
            new EquipmentBookedResponse(true, equipment.Amount), null, ServiceResponseStatus.Succeeded);
    }
}