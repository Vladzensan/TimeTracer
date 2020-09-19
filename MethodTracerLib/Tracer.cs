using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace MethodTracerLib
{
    public class Tracer : ITracer
    {

        private TraceResult traceResult = new TraceResult();
        private ConcurrentDictionary<int, Stack<TracedBlock>> threadBlocks = new ConcurrentDictionary<int, Stack<TracedBlock>>();
        public void StartTrace()
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            traceResult.threads.GetOrAdd(threadId, new ThreadInfo(threadId));
            Stack<TracedBlock> threadStack = threadBlocks.GetOrAdd(threadId, new Stack<TracedBlock>());

            StackTrace stackTrace = new StackTrace();
            TracedBlock newBlock = new TracedBlock(new MethodTrace(stackTrace.GetFrame(1).GetMethod()));

            UpdateParentInfo(threadStack, newBlock);

            threadStack.Push(newBlock);

            newBlock.watch.Restart();
        }

        private void UpdateParentInfo(Stack<TracedBlock> threadStack, TracedBlock tracedBlock)
        {
            if (threadStack.Count != 0)
            {
                MethodTrace parent = threadStack.Peek().methodTrace;
                parent.innerCalls.Add(tracedBlock.methodTrace);
            } else
            {
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

                ThreadInfo threadInfo;
                traceResult.threads.TryGetValue(threadId, out threadInfo);

                threadInfo.methods.Add(tracedBlock.methodTrace);

            }
        }

        public void StopTrace()
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stack<TracedBlock> threadStack;
            threadBlocks.TryGetValue(threadId, out threadStack);

            TracedBlock tracedBlock = threadStack.Pop();
            tracedBlock.watch.Stop();

            TimeSpan executionTime = tracedBlock.watch.Elapsed;
            tracedBlock.methodTrace.ExecutionTime = executionTime;

            ThreadInfo threadInfo;
            traceResult.threads.TryGetValue(threadId, out threadInfo);

            if (threadInfo.ExecutionTime == null)
            {
                threadInfo.ExecutionTime = new TimeSpan(0);
            }
            threadInfo.ExecutionTime = threadInfo.ExecutionTime.Add(executionTime);
        }

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

    }
}
