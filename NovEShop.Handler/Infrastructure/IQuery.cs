using MediatR;

namespace NovEShop.Handler.Infrastructure
{
    public interface IQuery<IQueryResult> : IRequest<IQueryResult>
    {
    }

    public interface IQuery : IRequest
    {
    }
}
