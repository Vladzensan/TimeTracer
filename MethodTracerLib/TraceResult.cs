using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace MethodTracerLib
{

    class TracedBlock
    {
        public Stopwatch watch = new Stopwatch();

        public MethodTrace methodTrace;

        public TracedBlock(MethodTrace methodTrace)
        {
            this.methodTrace = methodTrace;
        }
    }

    public class MethodTrace
    {
        public string methodName { get; set; }
        public string className { get; set; }
        public TimeSpan executionTime { get; set; }

        public List<MethodTrace> innerCalls = new List<MethodTrace>();
    }

    public class ThreadInfo
    {
        public TimeSpan executionTime = new TimeSpan(0);
        public List<MethodTrace> methods = new List<MethodTrace>();
    }
    public class TraceResult
    {
        public ConcurrentDictionary<int, ThreadInfo> threads = new ConcurrentDictionary<int, ThreadInfo>();

    }
}
