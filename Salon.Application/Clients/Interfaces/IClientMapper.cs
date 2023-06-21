using Salon.Domain.Clients.Contracts;
using Salon.Domain.Clients.Entities;
using System;
using System.Linq.Expressions;

namespace Salon.Application.Clients.Interfaces
{
    public interface IClientMapper
    {
        Client MapCommandToEntity(ClientCommand command);
        Expression<Func<Client, ClientResponse>> MapResponse();
    }
}