﻿using Lucene.Net.QueryParsers.Flexible.Core;
using Lucene.Net.QueryParsers.Flexible.Core.Nodes;
using Lucene.Net.QueryParsers.Flexible.Core.Parser;
using Lucene.Net.QueryParsers.Flexible.Core.Processors;
using Lucene.Net.QueryParsers.Flexible.Standard.Parser;
using Lucene.Net.QueryParsers.Flexible.Standard.Processors;
using Lucene.Net.Search.Spans;
using Lucene.Net.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene.Net.QueryParsers.Flexible.Spans
{
    /// <summary>
    /// This test case demonstrates how the new query parser can be used.
    /// <para/>
    /// It tests queries likes "term", "field:term" "term1 term2" "term1 OR term2",
    /// which are all already supported by the current syntax parser (
    /// {@link StandardSyntaxParser}).
    /// <para/>
    /// The goals is to create a new query parser that supports only the pair
    /// "field:term" or a list of pairs separated or not by an OR operator, and from
    /// this query generate {@link SpanQuery} objects instead of the regular
    /// {@link Query} objects. Basically, every pair will be converted to a
    /// {@link SpanTermQuery} object and if there are more than one pair they will be
    /// grouped by an {@link OrQueryNode}.
    /// <para/>
    /// Another functionality that will be added is the ability to convert every
    /// field defined in the query to an unique specific field.
    /// <para/>
    /// The query generation is divided in three different steps: parsing (syntax),
    /// processing (semantic) and building.
    /// <para/>
    /// The parsing phase, as already mentioned will be performed by the current
    /// query parser: {@link StandardSyntaxParser}.
    /// <para/>
    /// The processing phase will be performed by a processor pipeline which is
    /// compound by 2 processors: {@link SpansValidatorQueryNodeProcessor} and
    /// {@link UniqueFieldQueryNodeProcessor}.
    /// <para/>
    /// 
    ///     {@link SpansValidatorQueryNodeProcessor}: as it's going to use the current 
    ///     query parser to parse the syntax, it will support more features than we want,
    ///     this processor basically validates the query node tree generated by the parser
    ///     and just let got through the elements we want, all the other elements as 
    ///     wildcards, range queries, etc...if found, an exception is thrown.
    ///     
    ///     {@link UniqueFieldQueryNodeProcessor}: this processor will take care of reading
    ///     what is the &quot;unique field&quot; from the configuration and convert every field defined
    ///     in every pair to this &quot;unique field&quot;. For that, a {@link SpansQueryConfigHandler} is
    ///     used, which has the {@link UniqueFieldAttribute} defined in it.
    /// 
    /// <para/>
    /// The building phase is performed by the {@link SpansQueryTreeBuilder}, which
    /// basically contains a map that defines which builder will be used to generate
    /// {@link SpanQuery} objects from {@link QueryNode} objects.
    /// </summary>
    /// <seealso cref="SpansQueryConfigHandler"/>
    /// <seealso cref="SpansQueryTreeBuilder"/>
    /// <seealso cref="SpansValidatorQueryNodeProcessor"/>
    /// <seealso cref="SpanOrQueryNodeBuilder"/>
    /// <seealso cref="SpanTermQueryNodeBuilder"/>
    /// <seealso cref="StandardSyntaxParser"/>
    /// <seealso cref="UniqueFieldQueryNodeProcessor"/>
    /// <seealso cref="IUniqueFieldAttribute"/>
    public class TestSpanQueryParser : LuceneTestCase
    {
        private QueryNodeProcessorPipeline spanProcessorPipeline;

        private SpansQueryConfigHandler spanQueryConfigHandler;

        private SpansQueryTreeBuilder spansQueryTreeBuilder;

        private ISyntaxParser queryParser = new StandardSyntaxParser();

        public TestSpanQueryParser()
        {
            // empty constructor
        }


        public override void SetUp()
        {
            base.SetUp();

            this.spanProcessorPipeline = new QueryNodeProcessorPipeline();
            this.spanQueryConfigHandler = new SpansQueryConfigHandler();
            this.spansQueryTreeBuilder = new SpansQueryTreeBuilder();

            // set up the processor pipeline
            this.spanProcessorPipeline
                .SetQueryConfigHandler(this.spanQueryConfigHandler);

            this.spanProcessorPipeline.Add(new WildcardQueryNodeProcessor());
            this.spanProcessorPipeline.Add(new SpansValidatorQueryNodeProcessor());
            this.spanProcessorPipeline.Add(new UniqueFieldQueryNodeProcessor());

        }

        public SpanQuery GetSpanQuery(/*CharSequence*/string query)
        {
            return GetSpanQuery("", query);
        }

        public SpanQuery GetSpanQuery(String uniqueField, /*CharSequence*/string query)
        {

            this.spanQueryConfigHandler.Set(SpansQueryConfigHandler.UNIQUE_FIELD, uniqueField);

            IQueryNode queryTree = this.queryParser.Parse(query, "defaultField");
            queryTree = this.spanProcessorPipeline.Process(queryTree);

            return (SpanQuery)this.spansQueryTreeBuilder.Build(queryTree); // LUCENENET TODO: Find way to remove cast

        }

        [Test]
        public void testTermSpans()
        {
            assertEquals(GetSpanQuery("field:term").toString(), "term");
            assertEquals(GetSpanQuery("term").toString(), "term");

            assertTrue(GetSpanQuery("field:term") is SpanTermQuery);
            assertTrue(GetSpanQuery("term") is SpanTermQuery);

        }

        [Test]
        public void testUniqueField()
        {
            assertEquals(GetSpanQuery("field", "term").toString(), "field:term");
            assertEquals(GetSpanQuery("field", "field:term").toString(), "field:term");
            assertEquals(GetSpanQuery("field", "anotherField:term").toString(),
            "field:term");

        }

        [Test]
        public void testOrSpans()
        {
            assertEquals(GetSpanQuery("term1 term2").toString(),
            "spanOr([term1, term2])");
            assertEquals(GetSpanQuery("term1 OR term2").toString(),
            "spanOr([term1, term2])");

            assertTrue(GetSpanQuery("term1 term2") is SpanOrQuery);
            assertTrue(GetSpanQuery("term1 term2") is SpanOrQuery);

        }

        [Test]
        public void testQueryValidator()
        {

            try
            {
                GetSpanQuery("term*");
                fail("QueryNodeException was expected, wildcard queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("[a TO z]");
                fail("QueryNodeException was expected, range queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("a~0.5");
                fail("QueryNodeException was expected, boost queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("a^0.5");
                fail("QueryNodeException was expected, fuzzy queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("\"a b\"");
                fail("QueryNodeException was expected, quoted queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("(a b)");
                fail("QueryNodeException was expected, parenthesized queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

            try
            {
                GetSpanQuery("a AND b");
                fail("QueryNodeException was expected, and queries should not be supported");

            }
            catch (QueryNodeException ex)
            {
                // expected exception
            }

        }
    }
}
