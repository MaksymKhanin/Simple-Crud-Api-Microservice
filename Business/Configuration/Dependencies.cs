// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using Business.Models;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddValidationService(this IServiceCollection services)
        {
            services.AddTransient<IPayloadValidationService, PayloadValidationService>();

            services.AddAutoMapper(typeof(PayloadData));

            return services;
        }
    }
}
