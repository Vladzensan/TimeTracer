using MethodTracerLib;
using System;
using System.Threading;

namespace Serializers
{

    class First
    {
        public ITracer tracer { get; set; }

        public First(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void execute()
        {
            tracer.StartTrace();

            int a = 0;

            for(int i = 0; i < 10000000; i ++)
            {
                a += i * 2;
            }

            tracer.StopTrace();
        }
    }

    class Second
    {
        public ITracer tracer { get; set; }

        public Second(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void execute()
        {
            tracer.StartTrace();
            int a = 0;

            for (int i = 0; i < 2000000; i++)
            {
                a += i * 2;
            }

            a = inner(a);
            tracer.StopTrace();
        }

        public int inner(int a) {
            tracer.StartTrace();
            for (int i = 0; i < 20000; i++)
            {
                a += i * 2;
            }

            tracer.StopTrace();
            return a;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ITracer tracer = new Tracer();
            First f = new First(tracer);
            Second s = new Second(tracer);
            f.execute();
            s.execute();

            Thread thread = new Thread(() => new First(tracer).execute());
            thread.Start();
            thread.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            var consoleWriter = new ConsoleWriter();
            var fileWriter = new FileWriter("someFile");

            var jsonSerializer = new JsonSerializer();
            var xmlSerializer = new XmlSerializer();

            consoleWriter.Write(jsonSerializer, traceResult);
            consoleWriter.Write(xmlSerializer, traceResult);

            fileWriter.Write(jsonSerializer, traceResult);
            fileWriter.Write(xmlSerializer, traceResult);
            Console.ReadKey();
        }
    }
}
