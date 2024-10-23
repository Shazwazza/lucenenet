﻿using Lucene.Net.Util;
using System;

namespace Lucene.Net.Support.Threading
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /**
     * Base class for JSR166 Junit TCK tests.  Defines some constants,
     * utility methods and classes, as well as a simple framework for
     * helping to make sure that assertions failing in generated threads
     * cause the associated test that generated them to itself fail (which
     * JUnit does not otherwise arrange).  The rules for creating such
     * tests are:
     *
     * <ol>
     *
     * <li> All assertions in code running in generated threads must use
     * the forms {@link #threadFail}, {@link #threadAssertTrue}, {@link
     * #threadAssertEquals}, or {@link #threadAssertNull}, (not
     * <tt>fail</tt>, <tt>assertTrue</tt>, etc.) It is OK (but not
     * particularly recommended) for other code to use these forms too.
     * Only the most typically used JUnit assertion methods are defined
     * this way, but enough to live with.</li>
     *
     * <li> If you override {@link #setUp} or {@link #tearDown}, make sure
     * to invoke <tt>super.setUp</tt> and <tt>super.tearDown</tt> within
     * them. These methods are used to clear and check for thread
     * assertion failures.</li>
     *
     * <li>All delays and timeouts must use one of the constants <tt>
     * SHORT_DELAY_MS</tt>, <tt> SMALL_DELAY_MS</tt>, <tt> MEDIUM_DELAY_MS</tt>,
     * <tt> LONG_DELAY_MS</tt>. The idea here is that a SHORT is always
     * discriminable from zero time, and always allows enough time for the
     * small amounts of computation (creating a thread, calling a few
     * methods, etc) needed to reach a timeout point. Similarly, a SMALL
     * is always discriminable as larger than SHORT and smaller than
     * MEDIUM.  And so on. These constants are set to conservative values,
     * but even so, if there is ever any doubt, they can all be increased
     * in one spot to rerun tests on slower platforms.</li>
     *
     * <li> All threads generated must be joined inside each test case
     * method (or <tt>fail</tt> to do so) before returning from the
     * method. The <tt> joinPool</tt> method can be used to do this when
     * using Executors.</li>
     *
     * </ol>
     *
     * <p> <b>Other notes</b>
     * <ul>
     *
     * <li> Usually, there is one testcase method per JSR166 method
     * covering "normal" operation, and then as many exception-testing
     * methods as there are exceptions the method can throw. Sometimes
     * there are multiple tests per JSR166 method when the different
     * "normal" behaviors differ significantly. And sometimes testcases
     * cover multiple methods when they cannot be tested in
     * isolation.</li>
     *
     * <li> The documentation style for testcases is to provide as javadoc
     * a simple sentence or two describing the property that the testcase
     * method purports to test. The javadocs do not say anything about how
     * the property is tested. To find out, read the code.</li>
     *
     * <li> These tests are "conformance tests", and do not attempt to
     * test throughput, latency, scalability or other performance factors
     * (see the separate "jtreg" tests for a set intended to check these
     * for the most central aspects of functionality.) So, most tests use
     * the smallest sensible numbers of threads, collection sizes, etc
     * needed to check basic conformance.</li>
     *
     * <li>The test classes currently do not declare inclusion in
     * any particular package to simplify things for people integrating
     * them in TCK test suites.</li>
     *
     * <li> As a convenience, the <tt>main</tt> of this class (JSR166TestCase)
     * runs all JSR166 unit tests.</li>
     *
     * </ul>
     */
    public class JSR166TestCase : LuceneTestCase
    {
        ///**
        // * Runs all JSR166 unit tests using junit.textui.TestRunner
        // */
        //public static void main(String[] args)
        //{
        //    int iters = 1;
        //    if (args.length > 0)
        //        iters = Integer.parseInt(args[0]);
        //    Test s = suite();
        //    for (int i = 0; i < iters; ++i)
        //    {
        //        junit.textui.TestRunner.run(s);
        //        System.gc();
        //        System.runFinalization();
        //    }
        //    System.exit(0);
        //}

        ///**
        // * Collects all JSR166 unit tests as one suite
        // */
        //public static Test suite()
        //{
        //    TestSuite suite = new TestSuite("JSR166 Unit Tests");

        //    suite.addTest(new TestSuite(AbstractExecutorServiceTest.class));
        //    suite.addTest(new TestSuite(AbstractQueueTest.class));
        //    suite.addTest(new TestSuite(AbstractQueuedSynchronizerTest.class));
        //    suite.addTest(new TestSuite(ArrayBlockingQueueTest.class));
        //    suite.addTest(new TestSuite(AtomicBooleanTest.class));
        //    suite.addTest(new TestSuite(AtomicIntegerArrayTest.class));
        //    suite.addTest(new TestSuite(AtomicIntegerFieldUpdaterTest.class));
        //    suite.addTest(new TestSuite(AtomicIntegerTest.class));
        //    suite.addTest(new TestSuite(AtomicLongArrayTest.class));
        //    suite.addTest(new TestSuite(AtomicLongFieldUpdaterTest.class));
        //    suite.addTest(new TestSuite(AtomicLongTest.class));
        //    suite.addTest(new TestSuite(AtomicMarkableReferenceTest.class));
        //    suite.addTest(new TestSuite(AtomicReferenceArrayTest.class));
        //    suite.addTest(new TestSuite(AtomicReferenceFieldUpdaterTest.class));
        //    suite.addTest(new TestSuite(AtomicReferenceTest.class));
        //    suite.addTest(new TestSuite(AtomicStampedReferenceTest.class));
        //    suite.addTest(new TestSuite(ConcurrentHashMapTest.class));
        //    suite.addTest(new TestSuite(ConcurrentLinkedQueueTest.class));
        //    suite.addTest(new TestSuite(CopyOnWriteArrayListTest.class));
        //    suite.addTest(new TestSuite(CopyOnWriteArraySetTest.class));
        //    suite.addTest(new TestSuite(CountDownLatchTest.class));
        //    suite.addTest(new TestSuite(CyclicBarrierTest.class));
        //    suite.addTest(new TestSuite(DelayQueueTest.class));
        //    suite.addTest(new TestSuite(ExchangerTest.class));
        //    suite.addTest(new TestSuite(ExecutorsTest.class));
        //    suite.addTest(new TestSuite(ExecutorCompletionServiceTest.class));
        //    suite.addTest(new TestSuite(FutureTaskTest.class));
        //    suite.addTest(new TestSuite(LinkedBlockingQueueTest.class));
        //    suite.addTest(new TestSuite(LinkedListTest.class));
        //    suite.addTest(new TestSuite(LockSupportTest.class));
        //    suite.addTest(new TestSuite(PriorityBlockingQueueTest.class));
        //    suite.addTest(new TestSuite(PriorityQueueTest.class));
        //    suite.addTest(new TestSuite(ReentrantLockTest.class));
        //    suite.addTest(new TestSuite(ReentrantReadWriteLockTest.class));
        //    suite.addTest(new TestSuite(ScheduledExecutorTest.class));
        //    suite.addTest(new TestSuite(SemaphoreTest.class));
        //    suite.addTest(new TestSuite(SynchronousQueueTest.class));
        //    suite.addTest(new TestSuite(SystemTest.class));
        //    suite.addTest(new TestSuite(ThreadLocalTest.class));
        //    suite.addTest(new TestSuite(ThreadPoolExecutorTest.class));
        //    suite.addTest(new TestSuite(ThreadTest.class));
        //    suite.addTest(new TestSuite(TimeUnitTest.class));

        //    return suite;
        //}

        public static int SHORT_DELAY_MS;
        public static int SMALL_DELAY_MS;
        public static int MEDIUM_DELAY_MS;
        public static int LONG_DELAY_MS;

        /**
         * Returns the shortest timed delay. This could
         * be reimplemented to use for example a Property.
         */
        protected int getShortDelay()
        {
            return 50;
        }


        /**
         * Sets delays as multiples of SHORT_DELAY.
         */
        protected void setDelays()
        {
            SHORT_DELAY_MS = getShortDelay();
            SMALL_DELAY_MS = SHORT_DELAY_MS * 5;
            MEDIUM_DELAY_MS = SHORT_DELAY_MS * 10;
            LONG_DELAY_MS = SHORT_DELAY_MS * 50;
        }

        /**
         * Flag set true if any threadAssert methods fail
         */
        internal volatile bool threadFailed;

        /**
         * Initializes test to indicate that no thread assertions have failed
         */
        public override void SetUp()
        {
            base.SetUp();
            setDelays();
            threadFailed = false;
        }

        /**
         * Triggers test case failure if any thread assertions have failed
         */
        public override void TearDown()
        {
            assertFalse(threadFailed);
            base.TearDown();
        }

        /**
         * Fail, also setting status to indicate current testcase should fail
         */
        public void threadFail(string reason)
        {
            threadFailed = true;
            fail(reason);
        }

        /**
         * If expression not true, set status to indicate current testcase
         * should fail
         */
        public void threadAssertTrue(bool b)
        {
            if (!b)
            {
                threadFailed = true;
                assertTrue(b);
            }
        }

        /**
         * If expression not false, set status to indicate current testcase
         * should fail
         */
        public void threadAssertFalse(bool b)
        {
            if (b)
            {
                threadFailed = true;
                assertFalse(b);
            }
        }

        /**
         * If argument not null, set status to indicate current testcase
         * should fail
         */
        public void threadAssertNull(object x)
        {
            if (x != null)
            {
                threadFailed = true;
                assertNull(x);
            }
        }

        /**
         * If arguments not equal, set status to indicate current testcase
         * should fail
         */
        public void threadAssertEquals(long x, long y)
        {
            if (x != y)
            {
                threadFailed = true;
                assertEquals(x, y);
            }
        }

        /**
         * If arguments not equal, set status to indicate current testcase
         * should fail
         */
        public void threadAssertEquals(object x, object y)
        {
            if (x != y && (x == null || !x.equals(y)))
            {
                threadFailed = true;
                assertEquals(x, y);
            }
        }

        /**
         * threadFail with message "should throw exception"
         */
        public void threadShouldThrow()
        {
            //try
            //{
                threadFailed = true;
                fail("should throw exception");
            //}
            //catch (AssertionFailedError e)
            //{
            //    e.printStackTrace();
            //    throw e;
            //}
        }

        /**
         * threadFail with message "Unexpected exception"
         */
        public void threadUnexpectedException()
        {
            threadFailed = true;
            fail("Unexpected exception");
        }

        /**
         * threadFail with message "Unexpected exception", with argument
         */
        public void threadUnexpectedException(Exception ex)
        {
            threadFailed = true;
            ex.printStackTrace();
            fail("Unexpected exception: " + ex);
        }

        ///**
        // * Wait out termination of a thread pool or fail doing so
        // */
        //public void joinPool(ExecutorService exec)
        //{
        //    try
        //    {
        //        exec.shutdown();
        //        assertTrue(exec.awaitTermination(LONG_DELAY_MS, TimeUnit.MILLISECONDS));
        //    }
        //    catch (SecurityException ok)
        //    {
        //        // Allowed in case test doesn't have privs
        //    }
        //    catch (InterruptedException ie)
        //    {
        //        fail("Unexpected exception");
        //    }
        //}


        /**
         * fail with message "should throw exception"
         */
        public void shouldThrow()
        {
            fail("Should throw exception");
        }

        /**
         * fail with message "Unexpected exception"
         */
        public void unexpectedException()
        {
            fail("Unexpected exception");
        }


        // LUCENENET TODO: Complete port
    }
}