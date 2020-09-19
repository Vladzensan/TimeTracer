using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MethodTracerLib
{
    class Tracer : ITracer
    {
        private class TracedBlock
        {

        }

        private ConcurrentDictionary<int, List<TracedBlock>> threadBlocks = new ConcurrentDictionary<int, List<TracedBlock>>();

        public TraceResult GetTraceResult()
        {
            throw new NotImplementedException();
        }

        public void StartTrace()
        {
            throw new NotImplementedException();
        }

        public void StopTrace()
        {
            throw new NotImplementedException();
        }
    }
}
