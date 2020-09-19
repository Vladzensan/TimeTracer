using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

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

        public string time;

        [JsonIgnore]
        [XmlIgnore]
        private TimeSpan executionTime;

        [JsonIgnore]
        [XmlIgnore]
        public TimeSpan ExecutionTime
        {
            get
            {
                return executionTime;
            }
            set
            {
                executionTime = value; 
                time = String.Format("{0:00}ms", value.Milliseconds);

            }

        }

        public List<MethodTrace> innerCalls = new List<MethodTrace>();

        public MethodTrace() { }
        public MethodTrace(MethodBase methodBase)
        {
            this.methodName = methodBase.Name;
            this.className = methodBase.DeclaringType.Name;
        }
    }

    public class ThreadInfo
    {
        public int id;

        public string time;

        [JsonIgnore]
        [XmlIgnore]
        private TimeSpan executionTime;

        [JsonIgnore]
        [XmlIgnore]
        public TimeSpan ExecutionTime
        {
            get
            {
                return executionTime;
            }
            set
            {
                executionTime = value;
                time = String.Format("{0:00}ms", value.Milliseconds);

            }

        }
        public List<MethodTrace> methods = new List<MethodTrace>();

        public ThreadInfo() { }
        public ThreadInfo(int id)
        {
            this.id = id;
        }
             
    }
    public class TraceResult
    {
        public ConcurrentDictionary<int, ThreadInfo> threads = new ConcurrentDictionary<int, ThreadInfo>();

    }
}
