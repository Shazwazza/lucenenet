﻿// LUCENENET TODO: Port issues - missing dependencies

//using Lucene.Net.Support;

//namespace Lucene.Net.Analysis.ICU.Segmentation
//{
//    /*
//     * Licensed to the Apache Software Foundation (ASF) under one or more
//     * contributor license agreements.  See the NOTICE file distributed with
//     * this work for additional information regarding copyright ownership.
//     * The ASF licenses this file to You under the Apache License, Version 2.0
//     * (the "License"); you may not use this file except in compliance with
//     * the License.  You may obtain a copy of the License at
//     *
//     *     http://www.apache.org/licenses/LICENSE-2.0
//     *
//     * Unless required by applicable law or agreed to in writing, software
//     * distributed under the License is distributed on an "AS IS" BASIS,
//     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     * See the License for the specific language governing permissions and
//     * limitations under the License.
//     */

//    /// <summary>
//    /// Class that allows for tailored Unicode Text Segmentation on
//    /// a per-writing system basis.
//    /// <para/>
//    /// @lucene.experimental
//    /// </summary>
//    public abstract class ICUTokenizerConfig
//    {
//        /// <summary>
//        /// Sole constructor. (For invocation by subclass 
//        /// constructors, typically implicit.)
//        /// </summary>
//        public ICUTokenizerConfig() { }
//        /// <summary>
//        /// Return a breakiterator capable of processing a given script.
//        /// </summary>
//        public abstract Icu.BreakIterator GetBreakIterator(int script);
//        /// <summary>
//        /// Return a token type value for a given script and BreakIterator rule status.
//        /// </summary>
//        public abstract string GetType(int script, int ruleStatus);
//        /// <summary>
//        /// true if Han, Hiragana, and Katakana scripts should all be returned as Japanese
//        /// </summary>
//        public abstract bool CombineCJ { get; }
//    }
//}
