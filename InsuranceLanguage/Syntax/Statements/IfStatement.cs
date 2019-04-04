using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceLanguage.Syntax.Expressions;

namespace InsuranceLanguage.Syntax.Statements
{
    public class IfStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public BlockStatement Block { get; }
        public ElseStatement ElseStatement { get; }
        public Expression Predicate { get; }

        public IfStatement(SourceSpan sourceSpan, BlockStatement blockStatement, ElseStatement elseStatement, Expression predicate) 
            : base(sourceSpan)
        {
            Block = blockStatement;
            ElseStatement = elseStatement;
            Predicate = predicate;
        }
    }
}
