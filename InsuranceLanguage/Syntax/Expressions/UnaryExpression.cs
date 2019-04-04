using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Expressions
{
    public class UnaryExpression : Expression
    {
        public Expression Argument { get; }
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public UnaryOperator Operator { get; }

        public UnaryExpression(SourceSpan sourceSpan, Expression argument, UnaryOperator unaryOperator) : base(sourceSpan)
        {
            Argument = argument;
            Operator = unaryOperator;
        }
    }

    public enum UnaryOperator
    {
        Default,
        Increment,
        Decrement,
        Not
    }
}
