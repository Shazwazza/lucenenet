Apache Lucene.NET&trade; 4.8.0 API Docs
===============

---------------

<!-- LUCENENET TODO: Create the packages table of contents ... manually? Post processor?  -->


Apache Lucene is a high-performance, full-featured text search engine library.
Here's a simple example how to use Lucene for indexing and searching (using JUnit
to check if the results are what we expect):


<!-- LUCENENET TODO: Convert this to .NET  -->
```csharp
    Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_CURRENT);

    // Store the index in memory:
    Directory directory = new RAMDirectory();
    // To store an index on disk, use this instead:
    //Directory directory = FSDirectory.open("/tmp/testindex");
    IndexWriterConfig config = new IndexWriterConfig(Version.LUCENE_CURRENT, analyzer);
    IndexWriter iwriter = new IndexWriter(directory, config);
    Document doc = new Document();
    String text = "This is the text to be indexed.";
    doc.add(new Field("fieldname", text, TextField.TYPE_STORED));
    iwriter.addDocument(doc);
    iwriter.close();

    // Now search the index:
    DirectoryReader ireader = DirectoryReader.open(directory);
    IndexSearcher isearcher = new IndexSearcher(ireader);
    // Parse a simple query that searches for "text":
    QueryParser parser = new QueryParser(Version.LUCENE_CURRENT, "fieldname", analyzer);
    Query query = parser.parse("text");
    ScoreDoc[] hits = isearcher.search(query, null, 1000).scoreDocs;
    assertEquals(1, hits.length);
    // Iterate through the results:
    for (int i = 0; i < hits.length; i++) {
        Document hitDoc = isearcher.doc(hits[i].doc);
        assertEquals("This is the text to be indexed.", hitDoc.get("fieldname"));
    }
    ireader.close();
    directory.close();
```

The Lucene API is divided into several packages:

<!-- LUCENENET TODO: Fix links  -->

<ul>
<li>
<b>{@link org.apache.lucene.analysis}</b>
defines an abstract {@link org.apache.lucene.analysis.Analyzer Analyzer}
API for converting text from a {@link java.io.Reader}
into a {@link org.apache.lucene.analysis.TokenStream TokenStream},
an enumeration of token {@link org.apache.lucene.util.Attribute Attribute}s.&nbsp;
A TokenStream can be composed by applying {@link org.apache.lucene.analysis.TokenFilter TokenFilter}s
to the output of a {@link org.apache.lucene.analysis.Tokenizer Tokenizer}.&nbsp;
Tokenizers and TokenFilters are strung together and applied with an {@link org.apache.lucene.analysis.Analyzer Analyzer}.&nbsp;
<a href="../analyzers-common/overview-summary.html">analyzers-common</a> provides a number of Analyzer implementations, including 
<a href="../analyzers-common/org/apache/lucene/analysis/core/StopAnalyzer.html">StopAnalyzer</a>
and the grammar-based <a href="../analyzers-common/org/apache/lucene/analysis/standard/StandardAnalyzer.html">StandardAnalyzer</a>.</li>

<li>
<b>{@link org.apache.lucene.codecs}</b>
provides an abstraction over the encoding and decoding of the inverted index structure,
as well as different implementations that can be chosen depending upon application needs.

<li>
<b>{@link org.apache.lucene.document}</b>
provides a simple {@link org.apache.lucene.document.Document Document}
class.&nbsp; A Document is simply a set of named {@link org.apache.lucene.document.Field Field}s,
whose values may be strings or instances of {@link java.io.Reader}.</li>

<li>
<b>{@link org.apache.lucene.index}</b>
provides two primary classes: {@link org.apache.lucene.index.IndexWriter IndexWriter},
which creates and adds documents to indices; and {@link org.apache.lucene.index.IndexReader},
which accesses the data in the index.</li>

<li>
<b>{@link org.apache.lucene.search}</b>
provides data structures to represent queries (ie {@link org.apache.lucene.search.TermQuery TermQuery}
for individual words, {@link org.apache.lucene.search.PhraseQuery PhraseQuery} 
for phrases, and {@link org.apache.lucene.search.BooleanQuery BooleanQuery} 
for boolean combinations of queries) and the {@link org.apache.lucene.search.IndexSearcher IndexSearcher}
which turns queries into {@link org.apache.lucene.search.TopDocs TopDocs}.
A number of <a href="../queryparser/overview-summary.html">QueryParser</a>s are provided for producing
query structures from strings or xml.

<li>
<b>{@link org.apache.lucene.store}</b>
defines an abstract class for storing persistent data, the {@link org.apache.lucene.store.Directory Directory},
which is a collection of named files written by an {@link org.apache.lucene.store.IndexOutput IndexOutput}
and read by an {@link org.apache.lucene.store.IndexInput IndexInput}.&nbsp;
Multiple implementations are provided, including {@link org.apache.lucene.store.FSDirectory FSDirectory},
which uses a file system directory to store files, and {@link org.apache.lucene.store.RAMDirectory RAMDirectory}
which implements files as memory-resident data structures.</li>

<li>
<b>{@link org.apache.lucene.util}</b>
contains a few handy data structures and util classes, ie {@link org.apache.lucene.util.OpenBitSet OpenBitSet}
and {@link org.apache.lucene.util.PriorityQueue PriorityQueue}.</li>
</ul>
To use Lucene, an application should:
<ol>
<li>
Create {@link org.apache.lucene.document.Document Document}s by
adding
{@link org.apache.lucene.document.Field Field}s;</li>

<li>
Create an {@link org.apache.lucene.index.IndexWriter IndexWriter}
and add documents to it with {@link org.apache.lucene.index.IndexWriter#addDocument(Iterable) addDocument()};</li>

<li>
Call <a href="../queryparser/org/apache/lucene/queryparser/classic/QueryParserBase.html#parse(java.lang.String)">QueryParser.parse()</a>
to build a query from a string; and</li>

<li>
Create an {@link org.apache.lucene.search.IndexSearcher IndexSearcher}
and pass the query to its {@link org.apache.lucene.search.IndexSearcher#search(org.apache.lucene.search.Query, int) search()}
method.</li>
</ol>
Some simple examples of code which does this are:
<ul>
<li>
&nbsp;<a href="../demo/src-html/org/apache/lucene/demo/IndexFiles.html">IndexFiles.java</a> creates an
index for all the files contained in a directory.</li>

<li>
&nbsp;<a href="../demo/src-html/org/apache/lucene/demo/SearchFiles.html">SearchFiles.java</a> prompts for
queries and searches an index.</li>
</ul>

<!-- LUCENENET TODO: Fix this  -->

To demonstrate these, try something like:
<blockquote><tt>> <b>java -cp lucene-core.jar:lucene-demo.jar:lucene-analyzers-common.jar org.apache.lucene.demo.IndexFiles -index index -docs rec.food.recipes/soups</b></tt>
<br><tt>adding rec.food.recipes/soups/abalone-chowder</tt>
<br><tt>&nbsp; </tt>[ ... ]

<p><tt>> <b>java -cp lucene-core.jar:lucene-demo.jar:lucene-queryparser.jar:lucene-analyzers-common.jar org.apache.lucene.demo.SearchFiles</b></tt>
<br><tt>Query: <b>chowder</b></tt>
<br><tt>Searching for: chowder</tt>
<br><tt>34 total matching documents</tt>
<br><tt>1. rec.food.recipes/soups/spam-chowder</tt>
<br><tt>&nbsp; </tt>[ ... thirty-four documents contain the word "chowder" ... ]

<p><tt>Query: <b>"clam chowder" AND Manhattan</b></tt>
<br><tt>Searching for: +"clam chowder" +manhattan</tt>
<br><tt>2 total matching documents</tt>
<br><tt>1. rec.food.recipes/soups/clam-chowder</tt>
<br><tt>&nbsp; </tt>[ ... two documents contain the phrase "clam chowder"
and the word "manhattan" ... ]
<br>&nbsp;&nbsp;&nbsp; [ Note: "+" and "-" are canonical, but "AND", "OR"
and "NOT" may be used. ]</blockquote>