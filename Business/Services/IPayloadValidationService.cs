// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using Business.Models;
using OneOf;

namespace Business.Services
{
    public interface IPayloadValidationService
    {
        Task<OneOf<PayloadData, ValidationFailedResult>> ValidatePayload(Stream payload);
    }
}
