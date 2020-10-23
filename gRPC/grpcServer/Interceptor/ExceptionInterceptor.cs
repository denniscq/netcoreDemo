

namespace grpcServer.Interceptor
{
    #region using

    using System;
    using Grpc.Core;
    using System.Threading.Tasks;
    using Grpc.Core.Interceptors;

    #endregion

    public class ExceptionInterceptor : Interceptor
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
