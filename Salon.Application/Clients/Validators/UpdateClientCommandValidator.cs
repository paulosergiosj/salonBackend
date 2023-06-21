using FluentValidation;
using MongoDB.Bson;
using Salon.Domain.Clients.Repositories;

namespace Salon.Application.Clients.Validators
{
    public class UpdateClientCommandValidator : ClientCommandValidator
    {
        private readonly IClientRepository _clientRepository;

        public UpdateClientCommandValidator(IClientRepository clientRepository)
            : base(clientRepository)
        {
            _clientRepository = clientRepository;

            ValidateId();
        }
    }
}
