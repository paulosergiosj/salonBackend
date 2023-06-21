using Microsoft.Extensions.DependencyInjection;
using Salon.Application.Tests.Clients.Loads;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Users.Loads;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Users.Entities;

namespace Salon.Application.Tests.Modules
{
    public class LoadDataModule : IModule
    {
        public ServiceCollection Register(ServiceCollection service)
        {
            service.AddScoped<IDataLoad<User>, UserDataLoad>();
            service.AddScoped<IDataLoad<Client>, ClientDataLoad>();

            return service;
        }
    }
}
