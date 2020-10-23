using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Grpc
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return base.UnaryServerHandler(request, context, continuation);
            }
            catch (Exception ex)
            {
                var data = new Metadata();
                data.Add("message", ex.Message);
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown"), data);
            }
        }
    }
}
