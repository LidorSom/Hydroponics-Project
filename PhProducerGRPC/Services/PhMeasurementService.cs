using Grpc.Core;

namespace PhProducerGRPC.Services
{
    public class PhMeasurementService : PhProducerGRPC.PhMeasurementService.PhMeasurementServiceBase
    {
        public PhMeasurementService() { }

        public override Task<PhMessageResponse> PhMeasurement(PhMessage request, ServerCallContext context)
        {
            return Task.FromResult<PhMessageResponse>(new PhMessageResponse() { Ok= true , PhValueGot = request.PhValue});
        }
    }
}
