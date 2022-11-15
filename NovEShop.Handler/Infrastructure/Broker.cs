using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Infrastructure
{
    public class Broker : IBroker
    {
        private readonly IMediator _mediator;

        public Broker(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command)
        {
            return await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<TCommandResult> Command<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        public async Task Command(ICommand command)
        {
            await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task Command(ICommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        public async Task PublishEvent(INotification notification)
        {
            await _mediator.Publish(notification);
        }

        public async Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query)
        {
            return await _mediator.Send(query).ConfigureAwait(false);
        }

        public async Task<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }

        public async Task Query(IQuery query)
        {
            await _mediator.Send(query).ConfigureAwait(false);
        }

        public async Task Query(IQuery query, CancellationToken cancellationToken)
        {
            await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }
    }
}
