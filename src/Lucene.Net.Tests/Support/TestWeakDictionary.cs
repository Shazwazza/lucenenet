﻿#if !FEATURE_CONDITIONALWEAKTABLE_ENUMERATOR
using Lucene.Net.Attributes;
using Lucene.Net.Util;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Assert = Lucene.Net.TestFramework.Assert;

namespace Lucene.Net.Support
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

    [TestFixture]
    public class TestWeakDictionary : LuceneTestCase
    {
        [Test, LuceneNetSpecific]
        public void A_TestBasicOps()
        {
            IDictionary<object, object> weakDictionary = TestWeakDictionaryBehavior.CreateDictionary();// new SupportClass.TjWeakHashTable();
            Hashtable realHashTable = new Hashtable();

            SmallObject[] so = new SmallObject[100];
            for (int i = 0; i < 20000; i++)
            {
                SmallObject key = new SmallObject(i);
                SmallObject value = key;
                so[i / 200] = key;
                realHashTable.Add(key, value);
                weakDictionary.Add(key, value);
            }

            Assert.AreEqual(weakDictionary.Count, realHashTable.Count);

            ICollection keys = (ICollection)realHashTable.Keys;

            foreach (SmallObject key in keys)
            {
                Assert.AreEqual(((SmallObject)realHashTable[key]).i,
                                ((SmallObject)weakDictionary[key]).i);

                Assert.IsTrue(realHashTable[key].Equals(weakDictionary[key]));
            }


            ICollection values1 = (ICollection)weakDictionary.Values;
            ICollection values2 = (ICollection)realHashTable.Values;
            Assert.AreEqual(values1.Count, values2.Count);

            realHashTable.Remove(new SmallObject(10000));
            weakDictionary.Remove(new SmallObject(10000));
            Assert.AreEqual(weakDictionary.Count, 20000);
            Assert.AreEqual(realHashTable.Count, 20000);

            for (int i = 0; i < so.Length; i++)
            {
                realHashTable.Remove(so[i]);
                weakDictionary.Remove(so[i]);
                Assert.AreEqual(weakDictionary.Count, 20000 - i - 1);
                Assert.AreEqual(realHashTable.Count, 20000 - i - 1);
            }

            //After removals, compare the collections again.
            ICollection keys2 = (ICollection)realHashTable.Keys;
            foreach (SmallObject o in keys2)
            {
                Assert.AreEqual(((SmallObject)realHashTable[o]).i,
                                ((SmallObject)weakDictionary[o]).i);
                Assert.IsTrue(realHashTable[o].Equals(weakDictionary[o]));
            }
        }

        [Test, LuceneNetSpecific]
        [Slow]
        public void B_TestOutOfMemory()
        {
            var wht = TestWeakDictionaryBehavior.CreateDictionary();
            int OOMECount = 0;

            for (int i = 0; i < 1024 * 24 + 32; i++) // total requested Mem. > 24GB
            {
                try
                {
                    wht.Add(new BigObject(i), i);
                    if (i % 1024 == 0) Console.WriteLine("Requested Mem: " + i.ToString() + " MB");
                    OOMECount = 0;
                }
                catch (Exception oom) when (oom.IsOutOfMemoryError())
                {
                    if (OOMECount++ > 10) throw new Exception("Memory Allocation Error in B_TestOutOfMemory");
                    //Try Again. GC will eventually release some memory.
                    Console.WriteLine("OOME WHEN i=" + i.ToString() + ". Try Again");
                    System.Threading.Thread.Sleep(10);
                    i--;
                    continue;
                }
            }

            GC.Collect();
            Console.WriteLine("Passed out of memory exception.");
        }

        private long GetMemUsageInKB()
        {
            return System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024;
        }

        [Test, LuceneNetSpecific]
        [Slow]
        public void C_TestMemLeakage()
        {

            var wht = TestWeakDictionaryBehavior.CreateDictionary(); //new SupportClass.TjWeakHashTable();

            GC.Collect();
            long initialMemUsage = GetMemUsageInKB();

            Console.WriteLine("Initial MemUsage=" + initialMemUsage);
            for (int i = 0; i < 10000; i++)
            {
                wht.Add(new BigObject(i), i);
                if (i % 100 == 0)
                {
                    long mu = GetMemUsageInKB();
                    Console.WriteLine(i.ToString() + ") MemUsage=" + mu);
                }
            }

            GC.Collect();
            long memUsage = GetMemUsageInKB();
            Assert.IsFalse(memUsage > initialMemUsage * 2, "Memory Leakage.MemUsage = " + memUsage);
        }
    }
}
#endif