using MethodTracerLib;

namespace Serializers
{
    interface IWriter
    {
        void Write(ISerializer serializer, TraceResult traceResult);
    }
}
