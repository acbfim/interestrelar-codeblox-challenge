using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Application;
using Interestrelar.Application.Contract;
using Interestrelar.Auth.Application;
using Interestrelar.Auth.Contracts;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigServiceCollectionExtensions
    {

        public static IServiceCollection AddMyDependencyGroup(
             this IServiceCollection services)
        {
            services.AddScoped<IGeral, Geral>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}