using Microsoft.Extensions.DependencyInjection;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.Processing
{
    public class Mediator(IServiceProvider provider) : IMediator
    {
        private readonly IServiceProvider _provider = provider;
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _provider.GetService(handlerType)
                ?? throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

            var handleMethod = handlerType.GetMethod("Handle")
                ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");

            var parameters = new object[] { request, cancellationToken };

            var behaviors = _provider
                       .GetServices(typeof(IRequestPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse)))
                       .Cast<dynamic>()
                       .Reverse()
                       .Aggregate(
                           () => (Task<TResponse>)handleMethod.Invoke(handler, parameters)!,
                           (next, behavior) => () => behavior.Handle((dynamic)request, next, cancellationToken)
                       );

            return await behaviors();
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
            var handlers = _provider.GetServices(handlerType) ?? [];

            var handleMethod = handlerType.GetMethod("Handle")
                ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");

            foreach (var handler in handlers)
            {
                var task = handleMethod.Invoke(handler, [notification, cancellationToken]) as Task
                    ?? throw new InvalidOperationException($"Handler {handler!.GetType().Name} did not return a valid Task.");

                await task;
            }
        }
    }
}