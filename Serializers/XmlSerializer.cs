using MethodTracerLib;
using System.Collections.Generic;
using System.IO;

namespace Serializers
{
    class XmlSerializer: ISerializer
    {

        public string Serialize(TraceResult traceResult)
        {
            List<ThreadInfo> items = new List<ThreadInfo>(traceResult.threads.Values);

            System.Xml.Serialization.XmlSerializer xmlSerializer =
                new System.Xml.Serialization.XmlSerializer(items.GetType());


            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, items);
                return textWriter.ToString();
            }
        }

        public string GetExtension()
        {
            return ".xml";
        }
    }
}

