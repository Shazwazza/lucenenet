using Microsoft.DocAsCode.MarkdownLite;

namespace LuceneDocsPlugins
{
    public class LuceneNoteBlockToken : IMarkdownToken
    {
        public LuceneNoteBlockToken(IMarkdownRule rule, IMarkdownContext context, string label, SourceInfo sourceInfo)
        {
            Rule = rule;
            Context = context;
            Label = label;
            SourceInfo = sourceInfo;
        }

        public IMarkdownRule Rule { get; }

        public IMarkdownContext Context { get; }

        public string Label { get; }

        public SourceInfo SourceInfo { get; }
    }
}