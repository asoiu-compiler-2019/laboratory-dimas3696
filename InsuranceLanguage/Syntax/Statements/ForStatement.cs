using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceLanguage.Syntax.Expressions;

namespace InsuranceLanguage.Syntax.Statements
{
    public class ForStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.ForStatement;
        public BlockStatement Block { get; }
        public SyntaxNode Initialization { get; }
        public Expression Condition { get; }
        public Expression Increment { get; }

        public ForStatement(SourceSpan sourceSpan, BlockStatement blockStatement, SyntaxNode initial, Expression cond, Expression inc)
            : base(sourceSpan)
        {
            Block = blockStatement;
            Initialization = initial;
            Increment = inc;
            Condition = cond;
        }

    }
}
