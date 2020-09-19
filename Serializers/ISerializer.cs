using MethodTracerLib;

namespace Serializers
{
    interface ISerializer
    {
        string Serialize(TraceResult traceResult);
        string GetExtension();
    }
}
