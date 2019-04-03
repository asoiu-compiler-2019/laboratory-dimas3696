using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax
{
    class SourceDoc : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Document;
        public override SyntaxKind Kind => SyntaxKind.SourceDocument;
        public IEnumerable<SyntaxNode> SyntaxNodes { get; }
        public SourceCode SourceCode;

        public SourceDoc(SourceSpan sourceSpan, SourceCode sourceCode, IEnumerable<SyntaxNode> syntaxNodes) : base(sourceSpan)
        {
            this.SourceCode = sourceCode;
            this.SyntaxNodes = syntaxNodes;
        }
    }
}
