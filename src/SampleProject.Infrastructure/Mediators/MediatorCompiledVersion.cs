using Microsoft.Extensions.DependencyInjection;
using SampleProject.Application.Configurations.Mediators;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SampleProject.Infrastructure.Mediators;

internal class MediatorCompiledVersion(IServiceProvider provider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Delegate> CompiledDelegateCache = new();

    private const string BehaviorParamName = "behavior";
    private const string HandlerParamName = "handler";
    private const string RequestParamName = "request";
    private const string NextParamName = "next";
    private const string CtParamName = "ct";
    
    public async Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInterfaceType = typeof(IRequestHandler<>).MakeGenericType(requestType);

        var handler = provider.GetRequiredService(handlerInterfaceType);

        var invoke = (Func<object, IRequest, CancellationToken, Task>)CompiledDelegateCache
            .GetOrAdd(handler.GetType(), t => CompileVoidHandlerDelegate(t, requestType));

        var pipeline = BuildVoidPipeline(
            requestType,
            request,
            HandlerDelegate,
            cancellationToken);

        await pipeline();
        return;

        Task HandlerDelegate()
        {
            return invoke(handler, request, cancellationToken);
        }
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult));

        var handler = provider.GetRequiredService(handlerInterfaceType);

        var invoke = (Func<object, IRequest<TResult>, CancellationToken, Task<TResult>>)CompiledDelegateCache
            .GetOrAdd(handler.GetType(), t => CompileResultHandlerDelegate<TResult>(t, requestType));

        RequestHandlerDelegate<TResult> handlerDelegate = () => invoke(handler, request, cancellationToken);

        var pipeline = BuildResultPipeline(
            requestType,
            request,
            handlerDelegate,
            cancellationToken);

        return await pipeline();
    }

    private RequestHandlerDelegate BuildVoidPipeline(Type requestType, object request, RequestHandlerDelegate handler,
        CancellationToken cancellationToken)
    {
        var behaviorInterfaceType = typeof(IPipelineBehavior<>).MakeGenericType(requestType);
        var behaviors = provider.GetServices(behaviorInterfaceType).Reverse();

        return behaviors.Aggregate(handler, (next, behavior) =>
        {
            var b = behavior!;
            var invoke = (Func<object, object, RequestHandlerDelegate, CancellationToken, Task>)
                CompiledDelegateCache.GetOrAdd(b.GetType(), t => CompileVoidBehaviorDelegate(t, requestType));

            return () => invoke(b, request, next, cancellationToken);
        });
    }

    private RequestHandlerDelegate<TResponse> BuildResultPipeline<TResponse>(Type requestType, object request,
        RequestHandlerDelegate<TResponse> handler, CancellationToken cancellationToken)
    {
        var behaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = provider.GetServices(behaviorInterfaceType).Reverse();

        return behaviors.Aggregate(handler, (next, behavior) =>
        {
            var b = behavior!;
            var invoke = (Func<object, object, RequestHandlerDelegate<TResponse>, CancellationToken, Task<TResponse>>)
                CompiledDelegateCache.GetOrAdd(b.GetType(), t => CompileResultBehaviorDelegate<TResponse>(t, requestType));

            return () => invoke(b, request, next, cancellationToken);
        });
    }

    private static Delegate CompileVoidHandlerDelegate(Type handlerType, Type requestType)
    {
        var method = handlerType.GetMethod(nameof(IRequestHandler<>.HandleAsync));
        if (method == null)
        {
            throw new InvalidOperationException($"HandleAsync not found on handler type '{handlerType.Name}'.");
        }

        var handlerParam = Expression.Parameter(typeof(object), HandlerParamName);
        var requestParam = Expression.Parameter(typeof(IRequest), RequestParamName);
        var ctParam = Expression.Parameter(typeof(CancellationToken), CtParamName);

        var call = Expression.Call(
            Expression.Convert(handlerParam, handlerType),
            method,
            Expression.Convert(requestParam, requestType),
            ctParam);

        return Expression
            .Lambda<Func<object, IRequest, CancellationToken, Task>>(
                call,
                handlerParam,
                requestParam,
                ctParam)
            .Compile();
    }

    private static Delegate CompileResultHandlerDelegate<TResult>(Type handlerType, Type requestType)
    {
        var method = handlerType.GetMethod(nameof(IRequestHandler<,>.HandleAsync));
        if (method == null)
        {
            throw new InvalidOperationException($"HandleAsync not found on handler type '{handlerType.Name}'.");
        }

        var handlerParam = Expression.Parameter(typeof(object), HandlerParamName);
        var requestParam = Expression.Parameter(typeof(IRequest<TResult>), RequestParamName);
        var ctParam = Expression.Parameter(typeof(CancellationToken), CtParamName);

        var call = Expression.Call(
            Expression.Convert(handlerParam, handlerType),
            method,
            Expression.Convert(requestParam, requestType),
            ctParam);

        return Expression
            .Lambda<Func<object, IRequest<TResult>, CancellationToken, Task<TResult>>>(
                call,
                handlerParam,
                requestParam,
                ctParam)
            .Compile();
    }

    private static Delegate CompileVoidBehaviorDelegate(Type behaviorType, Type requestType)
    {
        var method = behaviorType.GetMethod("HandleAsync");
        if (method == null)
        {
            throw new InvalidOperationException($"HandleAsync not found on behavior type '{behaviorType.Name}'.");
        }

        var behaviorParam = Expression.Parameter(typeof(object), BehaviorParamName);
        var requestParam = Expression.Parameter(typeof(object), RequestParamName);
        var nextParam = Expression.Parameter(typeof(RequestHandlerDelegate), NextParamName);
        var ctParam = Expression.Parameter(typeof(CancellationToken), CtParamName);

        var call = Expression.Call(
            Expression.Convert(behaviorParam, behaviorType),
            method,
            Expression.Convert(requestParam, requestType),
            nextParam,
            ctParam);

        return Expression
            .Lambda<Func<object, object, RequestHandlerDelegate, CancellationToken, Task>>(
                call,
                behaviorParam,
                requestParam,
                nextParam,
                ctParam)
            .Compile();
    }

    private static Delegate CompileResultBehaviorDelegate<TResponse>(Type behaviorType, Type requestType)
    {
        var method = behaviorType.GetMethod("HandleAsync");
        if (method == null)
        {
            throw new InvalidOperationException($"HandleAsync not found on behavior type '{behaviorType.Name}'.");
        }

        var behaviorParam = Expression.Parameter(typeof(object), BehaviorParamName);
        var requestParam = Expression.Parameter(typeof(object), RequestParamName);
        var nextParam = Expression.Parameter(typeof(RequestHandlerDelegate<TResponse>), NextParamName);
        var ctParam = Expression.Parameter(typeof(CancellationToken), CtParamName);

        var call = Expression.Call(
            Expression.Convert(behaviorParam, behaviorType),
            method,
            Expression.Convert(requestParam, requestType),
            nextParam,
            ctParam);

        return Expression
            .Lambda<Func<object, object, RequestHandlerDelegate<TResponse>, CancellationToken, Task<TResponse>>>(
                call,
                behaviorParam,
                requestParam,
                nextParam,
                ctParam)
            .Compile();
    }
}
