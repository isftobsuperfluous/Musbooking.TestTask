using Musbooking.TestTask.Enums;

namespace Musbooking.TestTask.ServiceResponses;

public sealed class ServiceResponse<TResult, TError>
{
    public TResult? Result { get; set; }
    public TError? Error { get; set; }
    public ServiceResponseStatus Status { get; set; }

    public ServiceResponse(TResult? result, TError? error, ServiceResponseStatus status)
    {
        Result = result;
        Error = error;
        Status = status;
    }
}