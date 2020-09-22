﻿using MethodTracerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TracerUnitTests
{
    [TestClass]
    public class TracerTest
    {

        private ITracer tracer = new Tracer();

        private const int SLEEP_TIME = 30;
        private const int THREADS_COUNT = 3;

        private void SingleMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(SLEEP_TIME);
            tracer.StopTrace();
        }

        private void MethodWithInnerMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(SLEEP_TIME);
            SingleMethod();
            tracer.StopTrace();
        }

        [TestMethod]
        public void TestSingleMethod()
        {
            SingleMethod();
            TraceResult traceResult = tracer.GetTraceResult();
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            ThreadInfo threadInfo;
            traceResult.threads.TryGetValue(threadId, out threadInfo);
            string time = threadInfo.methods[0].time;
            int countedTime = Int32.Parse(time.Substring(0, time.Length - 2));

            Assert.AreEqual(nameof(SingleMethod), threadInfo.methods[0].methodName);
            Assert.AreEqual(nameof(TracerTest), threadInfo.methods[0].className);
            Assert.AreEqual(0, threadInfo.methods[0].innerCalls.Count);
            Assert.IsTrue(countedTime >= SLEEP_TIME);
        }

        [TestMethod]
        public void TestMethodWithInnerMethod()
        {
            MethodWithInnerMethod();
            TraceResult traceResult = tracer.GetTraceResult();
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            ThreadInfo threadInfo;
            traceResult.threads.TryGetValue(threadId, out threadInfo);
            string time = threadInfo.methods[0].time;
            int countedTime = Int32.Parse(time.Substring(0, time.Length - 2));

            Assert.AreEqual(nameof(MethodWithInnerMethod), threadInfo.methods[0].methodName);
            Assert.AreEqual(nameof(SingleMethod), threadInfo.methods[0].innerCalls[0].methodName);
            Assert.AreEqual(nameof(TracerTest), threadInfo.methods[0].className);
            Assert.AreEqual(1, threadInfo.methods[0].innerCalls.Count);
            Assert.AreEqual(0, threadInfo.methods[0].innerCalls[0].innerCalls.Count);
            Assert.IsTrue(countedTime >= SLEEP_TIME * 2);
        }

        [TestMethod]
        public void TestSingleMethodInMultiThreads()
        {
            var threads = new List<Thread>();
            double expectedTotalElapsedTime = THREADS_COUNT * SLEEP_TIME;

            for (int i = 0; i < THREADS_COUNT; i++)
            {
                var newThread = new Thread(SingleMethod);
                threads.Add(newThread);
                newThread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            double actualTotalElapsedTime = 0;

            foreach (var threadItem in tracer.GetTraceResult().threads)
            {
                string time = threadItem.Value.time;
                int countedTime = Int32.Parse(time.Substring(0, time.Length - 2));
                actualTotalElapsedTime += countedTime;
            }

            Assert.IsTrue(actualTotalElapsedTime >= expectedTotalElapsedTime);
            Assert.AreEqual(THREADS_COUNT, tracer.GetTraceResult().threads.Count);
        }
    }
}
