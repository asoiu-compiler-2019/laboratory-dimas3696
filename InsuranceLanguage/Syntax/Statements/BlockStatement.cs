using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Statements
{
    public class BlockStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.BlockStatement;
        public IEnumerable<SyntaxNode> Nodes { get; }

        public BlockStatement(SourceSpan sourceSpan, IEnumerable<SyntaxNode> nodes) : base(sourceSpan)
        {
            Nodes = nodes;
        }
    }
}
