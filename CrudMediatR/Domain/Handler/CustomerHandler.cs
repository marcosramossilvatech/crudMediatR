﻿using CrudMediatR.Domain.Command;
using CrudMediatR.Domain.Entity;
using CrudMediatR.Infra;
using CrudMediatR.Notications;
using MediatR;

namespace CrudMediatR.Domain.Handler
{
    public class CustomerHandler :
            IRequestHandler<CustomerCreateCommand, string>,
            IRequestHandler<CustomerUpdateCommand, string>,
            IRequestHandler<CustomerDeleteCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;

        public CustomerHandler(IMediator mediator, ICustomerRepository customerRepository)
        {
            _mediator = mediator;
            _customerRepository = customerRepository;
        }

        public async Task<string> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
            await _customerRepository.Save(customer);

            await _mediator.Publish(new CustomerActionNotification
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Action = ActionNotification.Criado
            }, cancellationToken);

            return await Task.FromResult("Cliente registrado com sucesso");
        }

        public async Task<string> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
            await _customerRepository.Update(request.Id, customer);

            await _mediator.Publish(new CustomerActionNotification
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Action = ActionNotification.Atualizado
            }, cancellationToken);

            return await Task.FromResult("Cliente atualizado com sucesso");
        }

        public async Task<string> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var client = await _customerRepository.GetById(request.Id);
            await _customerRepository.Delete(request.Id);

            await _mediator.Publish(new CustomerActionNotification
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Action = ActionNotification.Excluido
            }, cancellationToken);

            return await Task.FromResult("Cliente excluido com sucesso");
        }
    }
}
