using FluentValidation;
using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Clients.Repositories;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using Salon.Domain.ServiceOrders.Repositories;
using System;

namespace Salon.Application.ServiceOrders.Validators
{
    public class ServiceOrderCommandValidator : AbstractValidator<ServiceOrderCommand>
    {
        private const string INVALID_CLIENTID = "Invalid Client!";
        private const string INVALID_DATE = "Can't create Order from Future";
        private const string FIELD_EMPTY = "Field {0} can't be empty!";
        private const string INVALID_ITEM = "There is an Invalid Item!";
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IRepository<Item> _itemRepository;
        public ServiceOrderCommandValidator(
            IServiceOrderRepository serviceOrderRepository,
            IClientRepository clientRepository,
            IRepository<Item> itemRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _clientRepository = clientRepository;
            _itemRepository = itemRepository;

            ValidateClient();
            ValidateDate();
            ValidateItems();
        }

        private void ValidateClient()
        {
            RuleFor(x => x)
                .Custom(async (command, context) =>
                {
                    RuleFor(command => command.ClientId).NotEmpty().WithMessage(string.Format(FIELD_EMPTY, nameof(command.ClientId)));

                    if (!string.IsNullOrEmpty(command.ClientId) && ObjectId.TryParse(command.ClientId, out var idParsed))
                    {
                        var client = await _clientRepository.GetByIdAsync(idParsed);
                        if (client == null)
                        {
                            context.AddFailure(INVALID_CLIENTID);
                        }
                    }
                    else
                    {
                        context.AddFailure(INVALID_CLIENTID);
                    }
                });
        }

        private void ValidateDate()
        {
            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    RuleFor(command => command.Date).NotEmpty().WithMessage(string.Format(FIELD_EMPTY, nameof(command.Date)));
                    RuleFor(command => command.Date).GreaterThan(DateTime.Now).WithMessage(INVALID_DATE);
                });
        }

        private void ValidateItems()
        {
            RuleFor(x => x)
               .Custom(async (command, context) =>
               {
                   RuleFor(command => command.Items).NotNull().NotEmpty().WithMessage(string.Format(FIELD_EMPTY, nameof(command.Items)));

                   foreach (var item in command.Items)
                   {
                       if (ObjectId.TryParse(item.Key, out var idParsed))
                       {
                           if (!await _itemRepository.ExistAsync(idParsed))
                               context.AddFailure(INVALID_ITEM);
                           continue;
                       }
                       context.AddFailure(INVALID_ITEM);
                   }
               });
        }
    }
}
