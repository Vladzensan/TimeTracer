using MethodTracerLib;
using System.IO;

namespace Serializers
{
    class FileWriter: IWriter
    {
        private const string DEFAULT_FILENAME = "TraceResult";
        private string fileName;

        public FileWriter(string fileName)
        {
            this.fileName = (fileName == null? DEFAULT_FILENAME : fileName);
        }
        public void Write(ISerializer serializer, TraceResult traceResult)
        {
            using (StreamWriter sw = new StreamWriter(fileName + serializer.GetExtension()))
            {
                sw.WriteLine(serializer.Serialize(traceResult));
            }
        }
    }
}
