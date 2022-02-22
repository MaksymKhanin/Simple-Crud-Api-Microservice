// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleApiTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class ApiController : ControllerBase
    {
        private readonly IPayloadValidationService _validationService;

        public ApiController(IPayloadValidationService validationService) =>
            (_validationService) = (validationService);

        [HttpPost]
        [Route("validation")]
        public async Task<IActionResult> SendPayload([FromForm] SendPayloadRequestDto request)  //Payload is a file. It comes from a form on react.
        {
            using var payload = new MemoryStream();

            await request.payload.CopyToAsync(payload);

            var result = await _validationService.ValidatePayload(payload);

            return result.Match<IActionResult>(payloadData => Ok(payloadData),
                validationFailures => BadRequest(validationFailures)
                );
        }
    }

    public record SendPayloadRequestDto(IFormFile payload);
}
