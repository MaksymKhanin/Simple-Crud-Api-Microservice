// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using AutoMapper;
using Business.Models;
using OneOf;

namespace Business.Services
{
    internal class PayloadValidationService : IPayloadValidationService
    {
        private readonly ICollection<ValidationFailure> _validationErrors = new List<ValidationFailure>();
        private readonly IMapper _mapper;

        public PayloadValidationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<OneOf<PayloadData, ValidationFailedResult>> ValidatePayload(Stream payload)
        {
            var validationResult = Validate(payload);

            if (validationResult.IsValid)
            {
                //return some validation result. Probably deserialized data.
                //For simplicity I return here just an empty object;

                var deserialized = new PayloadDataType1();

                var payloadData = _mapper.Map<PayloadData>(deserialized);

                return payloadData;
            }
            else
            {
                return new ValidationFailedResult(validationResult.ValidationFailures.Select(vf => vf.Message).ToArray());
            }
        }

        public ValidationResult Validate(Stream payloadToValidate)
        {
            try
            {
                // here comes some validation
            }
            catch (Exception e) // maybe some more specific exception
            {
                return new ValidationResult(new[]
                {
                    new ValidationFailure(FailureMessages.InvalidFormat, e)
                });
            }

            return new ValidationResult(_validationErrors.ToArray());
        }
    }

    public record ValidationResult(ValidationFailure[] ValidationFailures)
    {
        public bool IsValid => !ValidationFailures.Any();
    }

    public record ValidationFailure(string Message, Exception? Exception = null);

    public static class FailureMessages
    {
        public const string InvalidFormat = "Validation failed. The payload is malformed";
    }
}
