using MethodTracerLib;
using Newtonsoft.Json;

namespace Serializers
{
    class JsonSerializer: ISerializer
    {

        public string Serialize(TraceResult traceResult)
        {
            return JsonConvert.SerializeObject(traceResult, Formatting.Indented);
        }

        public string GetExtension()
        {
            return ".json";
        }

    }
}
