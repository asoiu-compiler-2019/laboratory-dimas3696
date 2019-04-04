using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Statements
{
    public class ElseStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.ElseStatement;
        public BlockStatement BlockStatement { get; }

        public ElseStatement(SourceSpan sourceSpan, BlockStatement blockStatement) : base(sourceSpan)
        {
            BlockStatement = blockStatement;
        }
    }
}
