using MediatR;

namespace NovEShop.Handler.Infrastructure
{
    public interface ICommand<TCommandResult> : IRequest<TCommandResult>
    {
    }

    public interface ICommand : IRequest
    {
    }
}
