using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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

        public MethodTrace(MethodBase methodBase)
        {
            this.methodName = methodBase.Name;
            this.className = methodBase.DeclaringType.Name;
        }
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
