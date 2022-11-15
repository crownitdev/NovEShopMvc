using MediatR;

namespace NovEShop.Handler.Infrastructure
{
    public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult> where TQuery : ICommand<TQueryResult>
    {
    }

    public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery> where TQuery : IQuery
    {
    }
}
