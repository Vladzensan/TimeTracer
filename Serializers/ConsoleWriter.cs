using MethodTracerLib;
using System;

namespace Serializers
{
    class ConsoleWriter : IWriter
    {
        public void Write(ISerializer serializer, TraceResult traceResult)
        {
            Console.WriteLine(serializer.Serialize(traceResult));
        }
    }
}
